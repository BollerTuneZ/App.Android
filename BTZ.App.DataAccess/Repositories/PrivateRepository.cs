using System;
using BTZ.App.Infrastructure;
using BTZ.App.Data;

namespace BTZ.App.DataAccess
{
	public class PrivateRepository : IPrivateRepository
	{
		private LocalUser _localUser; 

		public PrivateRepository ()
		{
		}

		#region IPrivateRepository implementation

		public void CreateUser (string name, string password)
		{
			FetchLocalUserUser ();
			if (_localUser != null) {
				DeleteLocalUser ();
			}

			DatabaseInitialzer.Database.Insert (new LocalUser (){Name = name, Password = password });
		}

		public LocalUser GetLocalUser ()
		{
			if (_localUser == null) {
				FetchLocalUserUser ();
			}
			return _localUser;
		}

		public void UpdateLocalUser (LocalUser localUser)
		{
			DatabaseInitialzer.Database.Update (localUser);
			FetchLocalUserUser ();
		}

		public void DeleteLocalUser ()
		{
			DatabaseInitialzer.Database.Delete (_localUser);
		}
		#endregion

		#region LocalUser
		void FetchLocalUserUser()
		{
			_localUser = DatabaseInitialzer.Database.Table<LocalUser> ().FirstOrDefault();
		}
		#endregion
	}
}

