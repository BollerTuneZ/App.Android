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
using System.Threading;
using System.IO;
using Android.Graphics;
using BTZ.App.Data;

namespace BTZ.App.CommunicationTest
{
	[Activity (Label = "ActivityCommunicationTest")]			
	public class ActivityCommunicationTest : Activity
	{

		Button _btnGet;
		Button _btnSend;

		 INewsfeedMessageProcessor _newsfeedMessageProcessor;
		 INewsfeedRepository _newsfeedRepository;

		byte[] imageBytes;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ActivityCommunicationTest);
			_newsfeedMessageProcessor = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedMessageProcessor> ();
			_newsfeedMessageProcessor.OnUpdate += OnWallPostsUpdate;
			_newsfeedRepository = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedRepository> ();

			new Thread (() => {

				using (Stream inputSteam = this.Assets.Open("Indien_Goa_Strand-marked_kleen.jpg"))
				{
					var _originalBitmap = BitmapFactory.DecodeStream(inputSteam);
					MemoryStream stream = new MemoryStream();
					_originalBitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
					imageBytes = stream.ToArray();
				}


			}).Start ();
			Initialze ();
		}

		void Initialze()
		{
			_btnGet = FindViewById<Button> (Resource.Id.test_btn_get);
			_btnSend = FindViewById<Button> (Resource.Id.test_btn_post);
			_btnSend.Click += OnSendClicked;
			_btnGet.Click += OnGetClicked;
		}

		void OnGetClicked (object sender, EventArgs e)
		{
			Toast.MakeText(this,"Hole alle WallPosts",ToastLength.Short).Show();

			new Thread (_newsfeedMessageProcessor.GetAllNewsfeeds).Start ();
		}



		void OnSendClicked (object sender, EventArgs e)
		{
			Toast.MakeText(this,"Sende WallPost",ToastLength.Short).Show();

			var post = CreateWallPost ();

			new Thread (() =>
				_newsfeedMessageProcessor.PostSingleNewsfeed (post)).Start ();

		}

		WallPost CreateWallPost()
		{
			var tempWallPost = new WallPost () {
				Content = "Hallo Welt",
				Creator = "Jonas",
				HeaderImage = imageBytes,
				Image = imageBytes,
				Title = "TestPost"
			};

			return tempWallPost;
		}

		void OnWallPostsUpdate (object sender, EventArgs e)
		{
			RunOnUiThread (() => {
				var wallPosts = _newsfeedRepository.GetWallPosts();
				Toast.MakeText(this,String.Format("Wallposts {0}",wallPosts.Count),ToastLength.Short).Show();
			});
		}
	}
}

