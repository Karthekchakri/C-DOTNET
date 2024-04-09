using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using System.IO;
using Azure;
using Azure.Storage.Blobs.Specialized;
using System.Threading;
using System;



class Program
{
    private static Pageable<BlobContainerItem> containers;
   

    static void Main()
    {
        #region connectionConfig
        // Azurite storage emulator uses default endpoints, so no need to specify an endpoint
        string accountName = "devstoreaccount1";
        string connectionString = "<<EmulatorKey>>";
        #endregion

        #region Initialization
        BlobClient _blobClient;
        BlobClient _blobFolderClient;
        
        string abcContainerName = "abc";
        string defContainerName = "def";
        string xyzContainerName = "xyz";
       
        string sourcePath = "/Incoming/input.xml";
        string destinationPath = "/Outgoing/input.xml";
        

        string filePath = "C:\\Input\\input.xml";

        var _blobServiceClient = new BlobServiceClient(connectionString);
        var _abcBlobContainerClient = new BlobContainerClient(connectionString, abcContainerName);
        var _xyzBlobContainerClient = new BlobContainerClient(connectionString, xyzContainerName);

        //_blobClient = _blobContainerClient.GetBlobClient("/Input/"+sourcePath);



        #endregion

        #region List Of Containers
        containers = _blobServiceClient.GetBlobContainers();

        foreach (var containerItem in containers)
        {

            Console.WriteLine($"Container Name: {containerItem.Name}");
            //Console.WriteLine($"Container Name: {containerItem.VersionId}");
            Console.WriteLine($"Container Name: {containerItem.Properties.LastModified.ToString()}");

        }
        #endregion

        #region Create Containers
                
        if(!(containers.Where(c => c.Name == abcContainerName).ToList().Count > 0))
        _blobServiceClient.CreateBlobContainer(abcContainerName, PublicAccessType.BlobContainer);
        if(!(containers.Where(c => c.Name == defContainerName).ToList().Count > 0))
        _blobServiceClient.CreateBlobContainer(defContainerName, PublicAccessType.BlobContainer);
        if (!(containers.Where(c => c.Name == xyzContainerName).ToList().Count > 0))
            _blobServiceClient.CreateBlobContainer(xyzContainerName, PublicAccessType.BlobContainer);

        #endregion

        #region List all ContainerItems
        foreach (var blobItem in _abcBlobContainerClient.GetBlobs())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }
        foreach (var blobItem in _xyzBlobContainerClient.GetBlobs())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }
        #endregion

        #region Upload Blob to container
        using (var fileStream = File.OpenRead(filePath))
        {
            _blobClient = _abcBlobContainerClient.GetBlobClient(sourcePath);
            // Upload the blob to the container
            _blobClient.Upload(fileStream);
            _blobClient.SetTags(new Dictionary<string, string>
            {
                { "TagBlobType", "xml" }
            });
        }
        using (var fileStream = File.OpenRead(filePath))
        {
            _blobClient = _xyzBlobContainerClient.GetBlobClient(sourcePath);
            // Upload the blob to the container
            _blobClient.Upload(fileStream);
            _blobClient.SetTags(new Dictionary<string, string>
            {
                { "TagBlobType", "xml" }
            });
        }
        #endregion

        #region List all ContainerItems
        foreach (var blobItem in _abcBlobContainerClient.GetBlobs())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }
        foreach (var blobItem in _abcBlobContainerClient.GetBlobs())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }
        #endregion

        #region Move folder to folder
        var blobContainerClient = _blobServiceClient.GetBlobContainerClient(abcContainerName);
        var sourceBlobClient = blobContainerClient.GetBlobClient(sourcePath);
        var destinationBlobClient = blobContainerClient.GetBlobClient(destinationPath);

        var copyOp =  destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri, null,default);
         copyOp.Wait();

         sourceBlobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.None, null, default);

        foreach (var blobItem in _abcBlobContainerClient.GetBlobs())
        {
            Console.WriteLine($"Blob Name: {blobItem.Name}");
        }

        #endregion

        #region Delete
        var blob = _abcBlobContainerClient.GetBlobClient(destinationPath);
        var blob1 = _xyzBlobContainerClient.GetBlobClient(destinationPath);
        blob.Delete(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: default);
        blob1.Delete(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots, cancellationToken: default);
        #endregion

    }



}
