// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using NemsisProcess;

Console.WriteLine("Hello, World!");
var config = KafkaConsumerConfig.GetConfig();
string topic = "NemsisFile"; // Replace with your topic name
using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

consumer.Subscribe(topic);
while (true)
{
    try
    {
        var consumeResult = consumer.Consume();
        Console.WriteLine($"Received message: {consumeResult.Message.Value}");

        // Process the message here
        consumer.Commit(consumeResult);
    }
    catch (ConsumeException e)
    {
        Console.WriteLine($"Error consuming message: {e.Error.Reason}");
    }
}