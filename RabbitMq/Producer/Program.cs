using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "hello",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);

    string message = "Hello World!";
    var count = 0;
    Console.WriteLine("Producing.");


    while (true)
    {
        var body = Encoding.UTF8.GetBytes(message + "-" + count++);
        // produce
        channel.BasicPublish(exchange: "",
                             routingKey: "hello",
                             basicProperties: null,
                             body: body);

        System.Threading.Thread.Sleep(10);
    }
}