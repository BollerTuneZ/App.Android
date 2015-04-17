
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BTZ.App.Infrastructure;
using TinyIoC;


namespace BollerTuneZ
{
	[Activity (Label = "ActivityRegLoginTest")]			
	public class ActivityRegLoginTest : Activity
	{
		ILoginMessageProcessor _loginMessageProcessor;
		IPrivateRepository _pRepo;

		TextView lbn_status;
		Button btn_reg;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ActivtyTest);
			Initialize ();
		}

		void Initialize()
		{
			lbn_status = FindViewById<TextView> (Resource.Id.test_lbn_status);
			btn_reg = FindViewById<Button> (Resource.Id.test_btn_reg);

			btn_reg.Click += (object sender, EventArgs e) => _loginMessageProcessor.RegisterUser ();

			_pRepo = TinyIoCContainer.Current.Resolve<IPrivateRepository> ();
			_loginMessageProcessor = TinyIoCContainer.Current.Resolve<ILoginMessageProcessor> ();

			_pRepo.CreateUser ("Jonas", "040123");
			_loginMessageProcessor.OnRegisterResult += _loginMessageProcessor_OnRegisterResult;
		}

		void _loginMessageProcessor_OnRegisterResult (object sender, EventArgs e)
		{
			var response = (BoolArgs)e;

			RunOnUiThread (() => {
				lbn_status.Text = String.Format("Reg war {0}",response.Success);

			});
		}

	}
}

