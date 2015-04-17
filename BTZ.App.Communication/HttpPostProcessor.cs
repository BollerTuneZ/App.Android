using System;
using BTZ.App.Infrastructure;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Android.Util;

namespace BTZ.App.Communication
{
	public class HttpPostProcessor : IHttpPostProcessor
	{
		static readonly string Tag = "HttpPostProcessor";

		public HttpPostProcessor ()
		{
		}
		#region IHttpPostProcessor implementation

		public string PostAction (string url, object payload)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create (url);
			request.Method = "POST";
			request.KeepAlive = true;
			request.Timeout = 5000;

			string postData = JsonConvert.SerializeObject (payload);
			byte[] postBytes = Encoding.UTF8.GetBytes (postData);
			request.ContentType = "application/json; charset=utf-8";
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = postBytes.Length;
			Stream dataStream = null;

			try {
				dataStream = request.GetRequestStream ();
			} catch (Exception ex) {
				Log.Error (Tag, ex.Message);
				return null;
			}

			dataStream.Write (postBytes, 0, postBytes.Length);
			//dataStream.Close ();

			WebResponse response = null;

			try {
				response = (HttpWebResponse)request.GetResponse ();
				dataStream = response.GetResponseStream ();
			} catch (Exception ex) {
				Log.Error (Tag, ex.Message);
				return null;
			}
			// Get the stream containing content returned by the server.
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader (dataStream);
			// Read the content.
			string responseFromServer = reader.ReadToEnd ();
			reader.Close ();
			dataStream.Close ();
			response.Close ();

			return responseFromServer;
		}

		#endregion

	}
}

