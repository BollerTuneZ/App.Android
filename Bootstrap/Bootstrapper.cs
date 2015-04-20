using System;
using TinyIoC;
using BTZ.App.Infrastructure;
using BTZ.App.DataAccess;
using BTZ.App.Communication;


namespace BollerTuneZ
{
	public static class Bootstrapper
	{

		public static void Register()
		{

			RemoteConnection connection = new RemoteConnection ();


			TinyIoCContainer.Current.Register<IPrivateRepository,PrivateRepository> ();
			TinyIoCContainer.Current.Register<ILoginMessageProcessor,LoginMessageProcessor> ();

		}

	}
}

