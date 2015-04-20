using System;
using SQLite;

namespace BTZ.App.Data
{
	/// <summary>
	/// Datenbank objekt Newsfeed
	/// Jonas Ahlf 20.04.2015 21:59:22
	/// </summary>
	[Table("Newsfeed")]
	public class WallPost: BaseEntity
	{
		public WallPost ()
		{
		}

		public string Creator{ get; set;}

		/// <summary>
		/// Titel des Posts
		/// </summary>
		/// <value>The title.</value>
		public string Title{ get; set; }

		/// <summary>
		/// Schrifftlicher inhalt des Posts
		/// </summary>
		/// <value>The content.</value>
		public string Content{ get; set; }

		/// <summary>
		/// Kopfzeilenbild
		/// </summary>
		/// <value>The header image.</value>
		public byte[] HeaderImage{ get; set; }

		/// <summary>
		/// PostBild
		/// </summary>
		/// <value>The image.</value>
		public byte[] Image{ get; set; }
	}
}

