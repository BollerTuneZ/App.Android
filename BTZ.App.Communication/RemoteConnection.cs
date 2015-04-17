using System;
using Zyan.Communication;
using BTZ.Common;
namespace BTZ.App.Communication
{
	public class RemoteConnection
	{
		static readonly string AppServiceUri = "tcp://192.168.1.3:55566/appservice";
		public RemoteConnection ()
		{

			ZyanConnection connection = new ZyanConnection(AppServiceUri);

			RemoteService = connection.CreateProxy<BTZ.Common.IBaseAppService> ();
		}

		public IBaseAppService RemoteService{ get; internal set; }
	}
}

