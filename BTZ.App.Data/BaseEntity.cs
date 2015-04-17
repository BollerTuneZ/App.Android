using System;
using SQLite;
namespace BTZ.App.Data
{
	/// <summary>
	/// Basisklasse die von allen Lokalen Datenbank entities geerbt wird
	/// Jonas Ahlf 16.04.2015 14:31:50
	/// </summary>
	[Table("BaseEntity")]
	public class BaseEntity
	{
		public BaseEntity ()
		{
		}

		[PrimaryKey, AutoIncrement, Column("_id")]
		public int Id { get; set; }
	}
}

