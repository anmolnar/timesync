using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text;

namespace timesyncclient
{
	public class MainClass
	{
		private static byte[] handshake = Encoding.ASCII.GetBytes("howdy");

		public static void Main(string[] args)
		{
			Console.WriteLine("Timesync client");

			var client = new UdpClient();
			IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000); // endpoint where server is listening
			client.Connect(ep);
			

			for (int i = 0; i < 10; i++)
			{
				// handshake
				client.Send(handshake, handshake.Length);

				// receive t1
				byte[] data = client.Receive(ref ep);
				var receivedDt = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
				double t1 = (DateTime.UtcNow - receivedDt).TotalMilliseconds;
				
				// send now()
				byte[] dataToSend = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
				client.Send(dataToSend, dataToSend.Length); // reply back

				// receive t2
				byte[] t2Data = client.Receive(ref ep); // listen on port 11000
				var t2 = BitConverter.ToDouble(t2Data, 0);

				Console.WriteLine("receive data from {0}: t1 = {1}, t2 = {2}, d = {3}", ep, t1, t2, (t2 - t1) / 2);				

				Thread.Sleep(1000);
			}

			Console.WriteLine("Done");
			Console.Read();
		}
	}
}
