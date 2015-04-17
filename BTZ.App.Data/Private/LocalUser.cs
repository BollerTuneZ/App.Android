using System;
using SQLite;
namespace BTZ.App.Data
{
	[Table("LocalUser")]
	public class LocalUser : BaseEntity
	{
		public LocalUser ()
		{
		}

		public string Name{ get; set; }

		public string Password{ get; set; }

		public string Token{ get; set; }
	}
}

