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
			TinyIoCContainer.Current.Register<IPrivateRepository,PrivateRepository> ();
			TinyIoCContainer.Current.Register<IHttpPostProcessor,HttpPostProcessor> ();
			TinyIoCContainer.Current.Register<ILoginMessageProcessor,LoginMessageProcessor> ();

		}

	}
}

