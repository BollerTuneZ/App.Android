using System;
using System.Collections.Generic;
using BTZ.App.Data;

namespace BTZ.App.Infrastructure
{
	/// <summary>
	/// Datenbank zugriff auf die WallPosts des Newsfeeds
	/// Jonas Ahlf 20.04.2015 22:00:33
	/// </summary>
	public interface INewsfeedRepository
	{
		/// <summary>
		/// Fügt Wallposts der Datenbank hinzu
		/// </summary>
		/// <param name="wallpost">Wallpost.</param>
		void AddWallPosts(List<WallPost> wallpost);
		/// <summary>
		/// Updates Wallposts in der Datenbank
		/// </summary>
		/// <param name="wallpost">Wallpost.</param>
		void UpdateWallPosts(List<WallPost> wallpost);
		/// <summary>
		/// Löscht Wallposts aus der Datenbank
		/// </summary>
		/// <param name="wallpost">Wallpost.</param>
		void DeleteWallPosts(List<WallPost> wallpost);
		void DeleteAllWallPosts();

		/// <summary>
		/// Liefert alle Wallposts zurück
		/// </summary>
		/// <returns>The wall posts.</returns>
		List<WallPost> GetWallPosts();
	}
}

