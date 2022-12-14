using RabbitMQ.Client;
using System.Text;

namespace Producer;
internal static class Program
{
	private static void Main(string[] args)
	{
		var factory = new ConnectionFactory()
		{
			HostName = "localhost"
		};
		using IConnection connection = factory.CreateConnection();
		string queueName = "queuePub";
		IModel channel1 = SetupChannel(connection);
		IModel channel2 = SetupChannel(connection);
		BuildPublishers(channel1, queueName, "gossip1");
		BuildPublishers(channel2, queueName, "gossip2");
		Console.WriteLine(" Press [enter] to exit.");
		Console.ReadLine();
	}

	private static void BuildPublishers(IModel channel, string queue, string publisherName)
	{

		Task.Run(() =>
		{
			int count = 0;
			while (true)
			{
				string message = $" Message {count++} from {publisherName}";
				
				var body = Encoding.UTF8.GetBytes(message);
				channel.BasicPublish("exchangePub", "", null, body);
				Console.WriteLine($"{publisherName} - Sent {count}", message);
				Task.Delay(1000).Wait();
			}
		});
	}

	private static IModel SetupChannel(IConnection connection)
	{
		var channel = connection.CreateModel();
		channel.QueueDeclare("waiter", false, false, false, null);
		channel.QueueDeclare("logs", false, false, false, null);
		channel.QueueDeclare("bill", false, false, false, null);

		channel.ExchangeDeclare("exchangePub", "fanout");
		channel.QueueBind("waiter", "exchangePub","");
		channel.QueueBind("logs", "exchangePub", "");
		channel.QueueBind("bill", "exchangePub", "");

		return channel;
	}
}
