using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace timesyncclient
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Timesync client");

			var client = new UdpClient();
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); // endpoint where server is listening
			client.Connect(ep);

			for (int i = 0; i < 10; i++)
			{
				// send data
				byte[] dataToSend = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
				client.Send(dataToSend, dataToSend.Length);

				// then receive data
				byte[] receivedData = client.Receive(ref ep);
				var receivedDt = DateTime.FromBinary(BitConverter.ToInt64(receivedData, 0));
				TimeSpan t2 = DateTime.UtcNow - receivedDt;

				Console.WriteLine("receive data from {0}: {1}", ep, receivedDt.ToString("R"));

				byte[] t2ForSend = BitConverter.GetBytes(t2.TotalMilliseconds);
				client.Send(t2ForSend, t2ForSend.Length);

				Thread.Sleep(1000);
			}

			Console.WriteLine("Done");
			Console.Read();
		}
	}
}
