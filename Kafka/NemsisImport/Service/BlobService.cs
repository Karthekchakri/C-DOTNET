using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NemsisImport.Common;
using NemsisImport.Interface;
using NemsisImport.Models;


//using Document_Viewer.Api.Configuration;
using Microsoft.Extensions.Options;
//using RIEHR.Facades.Storage.Models;
//using RIEHR.Utility.Service.GlobalConfiguration;
using System.Diagnostics.CodeAnalysis;
using Azure;

namespace NemsisImport.Service;



public class BlobService : IBlobService
{
    public BlobStorageSettings _settings = new BlobStorageSettings();
    public BlobServiceClient _blobServiceClient;
    BlobContainerClient _blobContainerClient;
    BlobContainerClient _blobDestinationClient;
    BlobClient _blobClient;
    string connectionString = "DefaultEndpointsProtocol=https;AccountName=nemsisstorageacc;AccountKey=2v3BjBV6lhzlfqn5MKg/a5x7WymzRgxuhF0v5TTygOSeoYy8eI3KwddpJlCH+zHya4Betbnn9W2Q+ASte16Dnw==;EndpointSuffix=core.windows.net";
    string containerName = "source";
    string destContainerName = "processed";
    string sourcePath = "input.xml";
    string destinationPath = "input.xml";
    string filePath = "C:\\NEMSIS\\input.xml";
    public BlobService(

        )
    {
        _settings.ContainerName = "source";
        _blobServiceClient = new BlobServiceClient(connectionString);
        _blobContainerClient = new BlobContainerClient(connectionString, containerName);
        _blobDestinationClient = new BlobContainerClient(connectionString, destContainerName);
        _blobClient = _blobContainerClient.GetBlobClient(sourcePath);

    }

    public async Task ListContainers()
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=nemsisstorageacc;AccountKey=2v3BjBV6lhzlfqn5MKg/a5x7WymzRgxuhF0v5TTygOSeoYy8eI3KwddpJlCH+zHya4Betbnn9W2Q+ASte16Dnw==;EndpointSuffix=core.windows.net";
        var blobServiceClient = new BlobServiceClient(connectionString);

        await foreach (var containerItem in blobServiceClient.GetBlobContainersAsync())
        {
            Console.WriteLine($"Container Name: {containerItem.Name}");
        }
    }

    public async Task ListBlobsWithMetadataAsync()
    {
        await foreach (var blobItem in _blobContainerClient.FindBlobsByTagsAsync(@"""TagBlobType"" = 'xml'"))
        {
            var blobClient = _blobContainerClient.GetBlobClient(blobItem.BlobName);
            var blobProperties = await blobClient.GetPropertiesAsync();

            var t = blobClient.GetTags();
            Console.WriteLine($"Blob ContainerName: {blobItem.BlobContainerName}");
            Console.WriteLine($"Blob Url: {blobClient.Uri}");
            Console.WriteLine($"Blob Name: {blobItem.BlobName}");
            Console.WriteLine($"Blob Container Createdate: {blobProperties.Value.CreatedOn}");
            Console.WriteLine($"Blob Container LastAccessed: {blobProperties.Value.VersionId}");



            foreach (var metadata in blobProperties.Value.Metadata)
            {
                Console.WriteLine($"  Metadata: {metadata.Key} - {metadata.Value}");

            }

            Console.WriteLine();
        }
    }

    public async Task GetAllBlobsinContainer()
    {
        await foreach (var blobItem in _blobContainerClient.GetBlobsAsync())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }
    }

    public async Task<bool> MoveFileAsync()
    {

        var sourceBlobClient = _blobContainerClient.GetBlobClient(sourcePath);
        var destinationBlobClient = _blobDestinationClient.GetBlobClient(destinationPath);

        var copyOp = await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
        await copyOp.WaitForCompletionAsync();
        Console.WriteLine($"Moved the blob input.xml from {containerName} to {destContainerName}");
        return await sourceBlobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.None);
    }

    public async Task UploadBlobAsync()
    {
        using (var fileStream = File.OpenRead(filePath))
        {

            // Upload the blob to the container
            await _blobClient.UploadAsync(fileStream);
            await _blobClient.SetTagsAsync(new Dictionary<string, string>
    {
        { "TagBlobType", "xml" }
    });
        }

        Console.WriteLine("Saved the blob input.xml  with indexTag");
    }


}
