using System;
using BTZ.App.Data;

namespace BTZ.App.Infrastructure
{
	/// <summary>
	/// Datenzugriff auf alle Daten, die nur der Lokal installierten app was angehen
	/// </summary>
	public interface IPrivateRepository
	{
		/// <summary>
		/// Gibt den Lokalen Nutzer zurück
		/// </summary>
		/// <returns>The local user.</returns>
		LocalUser GetLocalUser();

		void CreateUser(string name,string password);

		void UpdateLocalUser(LocalUser localUser);

		void DeleteLocalUser();
	}
}

