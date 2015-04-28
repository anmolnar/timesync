using System;
using System.Net.Sockets;
using System.Net;

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
				var receivedDt = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
				double t1 = (DateTime.UtcNow - receivedDt).TotalMilliseconds;

				byte[] dataToSend = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
				udpServer.Send(dataToSend, dataToSend.Length, remoteEP); // reply back

				byte[] t2Data = udpServer.Receive(ref remoteEP); // listen on port 11000
				var t2 = BitConverter.ToDouble(t2Data, 0);

				Console.WriteLine("receive data from {0}: t1 = {1}, t2 = {2}, d = {3}", remoteEP, t1, t2, (t2 - t1) / 2);
			}
		}
	}
}
