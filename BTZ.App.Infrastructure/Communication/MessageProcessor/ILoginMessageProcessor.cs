using System;
namespace BTZ.App.Infrastructure
{
	/// <summary>
	/// Wickelt alle Login und Register Messages ab
	/// Jonas Ahlf 16.04.2015 14:26:25
	/// </summary>
	public interface ILoginMessageProcessor
	{

		event EventHandler OnLoginResult;

		event EventHandler OnRegisterResult;

		/// <summary>
		/// Versucht den Lokalen nutzer einzuloggen
		/// </summary>
		/// <returns><c>true</c>, if login was usered, <c>false</c> otherwise.</returns>
		void UserLogin();

		/// <summary>
		/// Versucht den Lokalennutzer zu registrieren
		/// </summary>
		/// <returns><c>true</c>, if user was registered, <c>false</c> otherwise.</returns>
		void RegisterUser ();
	}
}

