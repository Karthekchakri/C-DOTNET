// See https://aka.ms/new-console-template for more information
using Confluent.Kafka;
using KafkaExample;

//Console.WriteLine("Hello, World!");

 //var config = KafkaProducerConfig.GetConfig();

    //using var producer = new ProducerBuilder<Null, string>(config).Build();
    //string topic = "NemsisFile"; // Replace with your topic name
    //string message = "Hello, Kafka!";
    //var deliveryReport = producer.ProduceAsync(topic, new Message<Null, string> { Value = message }).Result;
    //Console.WriteLine($"Produced message to {deliveryReport.Topic} partition {deliveryReport.Partition} @ offset {deliveryReport.Offset}");

    var config1 = KafkaConsumerConfig.GetConfig();
    string topic1 = "NemsisFile"; // Replace with your topic name
    using var consumer = new ConsumerBuilder<Ignore, string>(config1).Build();

    consumer.Subscribe(topic1);
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



