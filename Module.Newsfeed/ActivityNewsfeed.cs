
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BTZ.App.Infrastructure;
using BTZ.App.DataAccess;
using BTZ.App.Data;
using System.Threading;

namespace Module.Newsfeed
{
	[Activity (Label = "ActivityNewsfeed")]			
	public class ActivityNewsfeed : Activity
	{
		#region Controls
		ListView lvWallPosts;
		#endregion
		INewsfeedRepository _newsfeedRepo;
		INewsfeedMessageProcessor _newsfeedMessageProcessor;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ActivityNewsfeed);

			_newsfeedRepo = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedRepository> ();
			_newsfeedMessageProcessor = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedMessageProcessor> ();
			_newsfeedMessageProcessor.OnUpdate += OnNewsFeedUpdate;
			Initialize ();
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			MenuInflater.Inflate(Resource.Menu.NewsfeedMenu, menu);
			return base.OnPrepareOptionsMenu(menu);
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			if (item.ItemId == Resource.Id.add_wallpost) {
				StartActivity (typeof(ActivityWallPostAdd));
			}

			if (item.ItemId == Resource.Id.menu_newsfeed_refresh) {

				new Thread (() => {
					_newsfeedRepo.DeleteAllWallPosts();
					_newsfeedMessageProcessor.GetAllNewsfeeds();
					//RunOnUiThread(() => UpdateList (_newsfeedRepo.GetWallPosts()));
				}).Start ();

			}
			return base.OnOptionsItemSelected(item);
		}

		void OnNewsFeedUpdate (object sender, EventArgs e)
		{
			var wallPosts = _newsfeedRepo.GetWallPosts ();
			RunOnUiThread(() => UpdateList (wallPosts));
		}


		void Initialize()
		{
			lvWallPosts = FindViewById<ListView> (Resource.Id.nf_lv_wallposts);
			new Thread (() => {
				var localWallPosts = _newsfeedRepo.GetWallPosts();
				if(!localWallPosts.Any())
				{
					_newsfeedMessageProcessor.GetAllNewsfeeds();
					return;
				}

				RunOnUiThread(() => UpdateList (localWallPosts));

			}).Start ();

		}


		void UpdateList(List<WallPost> posts)
		{
			List<WallPost> reverseList = new List<WallPost> ();

			foreach (var item in posts) {
				reverseList.Add (item);
			}
			WallPostAdapter adapter = new WallPostAdapter (this, reverseList);
			lvWallPosts.Adapter = adapter;
		}
	}
}

