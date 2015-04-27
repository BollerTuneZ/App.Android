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
			foreach (var item in wallpost) {
				DatabaseInitialzer.Database.Insert (item);
			}
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
			var query = DatabaseInitialzer.Database.Table<WallPost> ();

			List<WallPost> posts = new List<WallPost> ();

			foreach (var item in query) {
				posts.Add (item);
			}

			return posts;
		}
		#endregion
	}
}

