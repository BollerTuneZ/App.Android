using System;
using BTZ.App.Infrastructure;
using System.Collections.Generic;
using BTZ.App.Data;
using System.Linq;


namespace BTZ.App.DataAccess
{
	public class NewsfeedRepository : INewsfeedRepository
	{
		public NewsfeedRepository ()
		{
		}

		#region INewsfeedRepository implementation
		public void AddWallPosts (List<WallPost> wallpost)
		{
			DatabaseInitialzer.Database.InsertAll (wallpost);
		}
		public void UpdateWallPosts (List<WallPost> wallpost)
		{
			DatabaseInitialzer.Database.UpdateAll (wallpost);
		}
		public void DeleteWallPosts (List<WallPost> wallpost)
		{
			foreach (var item in wallpost) {
				DatabaseInitialzer.Database.Delete (item);
			}
		}

		public List<WallPost> GetWallPosts ()
		{
			return DatabaseInitialzer.Database.Table<WallPost> ().ToList ();
		}
		#endregion
	}
}

