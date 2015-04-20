using System;
using BTZ.Common;
using System.Net.Sockets;
using System.IO;
using Newtonsoft.Json;


namespace BTZ.App.Communication
{
	public class RemoteConnection
	{
		static readonly string AppServiceUri = "tcp://192.168.1.3:55566/appservice";
		public RemoteConnection ()
		{
			
		}


		public string Request(BaseDto payload)
		{

			TcpClient client = new TcpClient ("192.168.1.3", 55566);

			if (!client.Connected) {
				return "";
			}

			var stream = client.GetStream ();

			StreamReader reader = new StreamReader (stream);
			StreamWriter writer = new StreamWriter (stream);
			writer.AutoFlush = true;

			writer.WriteLine (JsonConvert.SerializeObject (payload));

			return reader.ReadLine ();
		}

	}
}

