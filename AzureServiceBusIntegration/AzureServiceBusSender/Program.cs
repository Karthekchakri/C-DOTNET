// See https://aka.ms/new-console-template for more information
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using AzureServiceBusSender;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

Console.WriteLine("Hello, World!");

// name of your Service Bus queue
// the client that owns the connection and can be used to create senders and receivers
ServiceBusClient client;

// the sender used to publish messages to the queue
ServiceBusSender sender;

// number of messages to be sent to the queue
const int numOfMessages = 3;

// The Service Bus client types are safe to cache and use as a singleton for the lifetime
// of the application, which is best practice when messages are being published or read
// regularly.
//
// Set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
// If you use the default AmqpTcp, ensure that ports 5671 and 5672 are open.
var clientOptions = new ServiceBusClientOptions
{
    TransportType = ServiceBusTransportType.AmqpWebSockets
};
//TODO: Replace the "<NAMESPACE-NAME>" and "<QUEUE-NAME>" placeholders.
client = new ServiceBusClient(
    "https://azservicebus.servicebus.windows.net",
    new DefaultAzureCredential(),
    clientOptions);
sender = client.CreateSender("inputqueue");

// create a batch 
using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

for (int i = 1; i <= numOfMessages; i++)
{
    // try adding a message to the batch
    if (!messageBatch.TryAddMessage(new ServiceBusMessage( System.Security.SecurityElement.Escape(await constructMessageBody()))))
    {
        // if it is too large for the batch
        throw new Exception($"The message {i} is too large to fit in the batch.");
    }
}

try
{
    // Use the producer client to send the batch of messages to the Service Bus queue
    await sender.SendMessagesAsync(messageBatch);
    Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
}
finally
{
    // Calling DisposeAsync on client types is required to ensure that network
    // resources and other unmanaged objects are properly cleaned up.
    await sender.DisposeAsync();
    await client.DisposeAsync();
}

async Task<string> constructMessageBody()
{

    return JsonSerializer.Serialize(new Payload()
    {
       Data1 = "{TestData1}",
        Data2 = "{TestData2}"
    }
    );

}

Console.WriteLine("Press any key to end the application");
Console.ReadKey();
