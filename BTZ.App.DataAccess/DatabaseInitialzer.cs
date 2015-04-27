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
			
		}

		public static void Init()
		{
			DB = new SQLiteConnection (Path.Combine (Environment.GetFolderPath (Environment.SpecialFolder.Personal), "btz.db3"));

			DB.CreateTable<LocalUser> ();
			DB.CreateTable<WallPost> ();
			DB.DeleteAll<WallPost> ();
			DB.Insert (new WallPost (){ Content ="Hallo", Creator = "Jonas", Title = "Test" });
		}
		public static SQLiteConnection Database{ get{ return DB; } }

	}
}

