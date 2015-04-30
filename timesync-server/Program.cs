using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace timesyncserver
{
	public class MainClass
	{
		private const int ListenPort = 11000;

		public static void Main(string[] args)
		{
			Console.WriteLine("Timesync server");

			UdpClient udpServer = new UdpClient(11000);

			while (true)
			{
				var remoteEP = new IPEndPoint(IPAddress.Any, 11000); 
				byte[] data = udpServer.Receive(ref remoteEP); // listen on port 11000
				if (Encoding.ASCII.GetString(data) != "howdy")
				{
					continue;
				}
				
				// send now()
				byte[] dataToSend = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
				udpServer.Send(dataToSend, dataToSend.Length, remoteEP);

				// receive remote now()
				byte[] receivedData = udpServer.Receive(ref remoteEP);
				var receivedDt = DateTime.FromBinary(BitConverter.ToInt64(receivedData, 0));
				TimeSpan t2 = DateTime.UtcNow - receivedDt;

				Console.WriteLine("receive data from {0}: {1}", remoteEP, receivedDt.ToString("R"));

				// send t2
				byte[] t2ForSend = BitConverter.GetBytes(t2.TotalMilliseconds);
				udpServer.Send(t2ForSend, t2ForSend.Length, remoteEP);	
			}
		}
	}
}
