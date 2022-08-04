using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

string queue = "queuePub";
var factory = new ConnectionFactory
{
	HostName = "localhost"
};
using var connection = factory.CreateConnection();
var channel = connection.CreateModel();
try
{
	channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, null);
	BuildAndRunWorker(channel, "Listening 1");
	BuildAndRunWorker(channel, "Listening 2");

	Console.WriteLine(" Press [enter] to exit.");
	Console.ReadLine();
}
finally
{
	if (channel != null)
	{
		channel.Dispose();
	}
}

void BuildAndRunWorker(IModel channel, string workerName)
{
	var consumer = new EventingBasicConsumer(channel);
	consumer.Received +=  async ( model, ea) =>
	{
		try
		{
			byte[] bytes = ea.Body.ToArray();
			string body = Encoding.UTF8.GetString(bytes);
			Console.WriteLine($" {workerName} - Received {body} ");
			await Task.Delay(1000);
			channel.BasicAck(ea.DeliveryTag, multiple: false);
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
		}
	};
	channel.BasicConsume(queue, autoAck: false, consumer);

}