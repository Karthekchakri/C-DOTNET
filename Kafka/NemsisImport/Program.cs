// See https://aka.ms/new-console-template for more information

using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Confluent.Kafka;
using NemsisImport;
using Microsoft.Extensions.DependencyInjection;
using NemsisImport.Interface;
using NemsisImport.Service;


Console.WriteLine("Hello, World!");
#region kafka
//var config = KafkaProducerConfig.GetConfig();
//using var producer = new ProducerBuilder<Null, string>(config).Build();
//string topic = "NemsisFile"; // Replace with your topic name
#endregion

var services = new ServiceCollection();
services.AddSingleton<IBlobService, BlobService>();
services.AddSingleton<IFileExtractService, ProcessBlob>();

var serviceProvider = services.BuildServiceProvider();
var fileProcessing = serviceProvider.GetRequiredService<IFileExtractService>();
await fileProcessing.GetFiles();
return;

#region blob Changes
//Read blob and write message
string connectionString = "DefaultEndpointsProtocol=https;AccountName=nemsisstorageacc;AccountKey=2v3BjBV6lhzlfqn5MKg/a5x7WymzRgxuhF0v5TTygOSeoYy8eI3KwddpJlCH+zHya4Betbnn9W2Q+ASte16Dnw==;EndpointSuffix=core.windows.net";
string containerName = "source";

BlobContainerClient containerClient = new BlobContainerClient(connectionString, containerName);
var blobServiceClient = new BlobServiceClient(connectionString);

await foreach (var containerItem in blobServiceClient.GetBlobContainersAsync())
{
    Console.WriteLine($"Container Name: {containerItem.Name}");
}

// List all blobs in the container
Console.WriteLine("Listing blobs...");
await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
{
    Console.WriteLine($"\t{blobItem.Name} - {blobItem.Properties.ContentLength} bytes - {blobItem.Properties.LastModified}");
}
#endregion

//Read the azure storage blob
#region kafka changes
//string message = "Nemsis from blob";
//var deliveryReport = producer.ProduceAsync(topic, new Message<Null, string> { Value = message }).Result;
//Console.WriteLine($"Produced message to {deliveryReport.Topic} partition {deliveryReport.Partition} @ offset {deliveryReport.Offset}");
#endregion
