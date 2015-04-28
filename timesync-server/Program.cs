using System;
using System.Net.Sockets;
using System.Net;

namespace timesyncserver
{
	class MainClass
	{
		private const int ListenPort = 11000;

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			var listener = new UdpClient (ListenPort);
			var ipEndpoint = new IPEndPoint (IPAddress.Any, ListenPort);

		}
	}
}
