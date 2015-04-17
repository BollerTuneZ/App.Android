using System;
using SQLite;
using System.IO;
using BTZ.App.Data;

namespace BTZ.App.DataAccess
{
	/// <summary>
	/// Initialisiert die Datenbank
	/// Jonas Ahlf 16.04.2015 14:52:29
	/// </summary>
	public static class  DatabaseInitialzer
	{
		internal static SQLiteConnection DB;

		static DatabaseInitialzer ()
		{
			DB = new SQLiteConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "btz.db"));

			DB.CreateTable<LocalUser> ();
		}

		public static SQLiteConnection Database{ get{ return DB; } }

	}
}

