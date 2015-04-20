using System;
using BTZ.App.Infrastructure;
using System.Threading;
using BTZ.App.Data;
using BTZ.Common;
using Android.Util;
using Newtonsoft.Json;

namespace BTZ.App.Communication
{
	/// <summary>
	/// Jonas Ahlf 16.04.2015 15:33:36
	/// </summary>
	public class LoginMessageProcessor : ILoginMessageProcessor
	{
		#region WebUri
		const string BaseUri = "http://192.168.1.3:56534/btz";
		const string LoginUri = "/login/";
		const string RegUri = "/reg/";

		#endregion

		readonly RemoteConnection _remoteConnection;
		readonly IPrivateRepository _privateRepo;

		public LoginMessageProcessor (IPrivateRepository _privateRepo)
		{
			this._remoteConnection = new RemoteConnection ();
			this._privateRepo = _privateRepo;
		}

		#region ILoginMessageProcessor implementation
		public event EventHandler OnLoginResult;
		public event EventHandler OnRegisterResult;

		public void UserLogin ()
		{
			new Thread (() => {

				LocalUser user = _privateRepo.GetLocalUser();

				LoginData data = new LoginData()
				{
					Username = user.Name,
					Password = user.Password
				};

				var result = _remoteConnection.Request(new BaseDto(){JsonObject = JsonConvert.SerializeObject(data), Type = DtoType.Login});

				BoolArgs args;

				if(result == null)
				{
					args = new BoolArgs(){Success = false};
					FireLoginEvent(args);
					return;
				}

				var response = ParseLoginResponse(result);

				if(response == null)
				{
					args = new BoolArgs(){Success = false};
					FireLoginEvent(args);
					return;
				}

				if(response.Success)
				{
					user.Token = response.Token;
					_privateRepo.UpdateLocalUser(user);
				}

				args = new BoolArgs(){Success = response.Success};
				FireLoginEvent(args);

			}).Start ();
		}

		public void RegisterUser ()
		{

			LocalUser user = _privateRepo.GetLocalUser();

			LoginData data = new LoginData()
			{
				Username = user.Name,
				Password = user.Password
			};

			var result = _remoteConnection.Request(new BaseDto(){JsonObject = JsonConvert.SerializeObject(data), Type = DtoType.Register});

			BoolArgs args;

			if(result == null)
			{
				args = new BoolArgs(){Success = false};
				FireRegEvent(args);
				return;
			}

			var response = ParseLoginResponse(result);

			if(response == null)
			{
				args = new BoolArgs(){Success = false};
				FireRegEvent(args);
				return;
			}

			if (response.Success) {
				user.Token = response.Token;
				_privateRepo.UpdateLocalUser (user);
			} else {
				_privateRepo.DeleteLocalUser ();
			}

			args = new BoolArgs(){Success = response.Success};
			FireRegEvent(args);
		}
		#endregion

		void FireLoginEvent(BoolArgs args)
		{
			if (OnLoginResult != null) {
				OnLoginResult (this, args);
			}
		}

		void FireRegEvent(BoolArgs args)
		{
			if (OnRegisterResult != null) {
				OnRegisterResult (this, args);
			}
		}

		LoginResponse ParseLoginResponse(string value)
		{
			try {
				return JsonConvert.DeserializeObject<LoginResponse>(value);
			} catch (Exception ex) {
				return null;
			}
		}

		string SerializeObject(object obj)
		{
			return JsonConvert.SerializeObject (obj);
		}

	}
}

