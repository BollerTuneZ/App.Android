
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
using Android.Provider;
using Android.Content.PM;
using Android.Graphics;
using Java.IO;
using Android.Net;
using BTZ.App.Infrastructure;
using BTZ.App.Data;
using System.IO;
using System.Threading;


namespace Module.Newsfeed
{
	[Activity (Label = "ActivityWallPostAdd")]			
	public class ActivityWallPostAdd : Activity
	{
		INewsfeedMessageProcessor _newsfeedMessageProcessor;
		INewsfeedRepository _newsfeedRepository;
		#region Controls
		Button btnCancel;
		Button btnSend;
		EditText inpText;
		ImageView imgViewImage;
		#endregion

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.ActivityWallPostAdd);
			Init ();
			CreateDirectoryForPictures ();
			if (IsThereAnAppToTakePictures ()) {
				if (App.bitmap == null) {
					TakeAPicture ();
				}
			}
		}

		void Init()
		{
			_newsfeedMessageProcessor = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedMessageProcessor> ();
			_newsfeedRepository = TinyIoC.TinyIoCContainer.Current.Resolve<INewsfeedRepository> ();
			btnCancel = FindViewById<Button> (Resource.Id.nfa_cancel);
			btnCancel.Click += (object sender, EventArgs e) => Finish();
			btnSend = FindViewById<Button> (Resource.Id.nfa_btn_send);
			btnSend.Click += OnSendClicked;
			inpText = FindViewById<EditText> (Resource.Id.nfa_inp_text);
			imgViewImage = FindViewById<ImageView> (Resource.Id.nfa_imgView_img);
			if (App.bitmap != null) {
				imgViewImage.SetImageBitmap (App.bitmap);
			}
		}

		void OnSendClicked (object sender, EventArgs e)
		{
			if (App.bitmap == null || String.IsNullOrEmpty (inpText.Text)) {
				Toast.MakeText (this, "Kein Text, kein Bild, kein Post ;)", ToastLength.Long).Show ();
				return;
			}

			WallPost post = CreateWallPost ();

			_newsfeedMessageProcessor.PostSingleNewsfeed (post);
			_newsfeedRepository.DeleteAllWallPosts ();
			Toast.MakeText (this, "Posted", ToastLength.Short).Show ();
			App.bitmap = null;
			Finish ();
		}


		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		private void CreateDirectoryForPictures()
		{
			App._dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "BtzImages");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		private void TakeAPicture()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);

			App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			// make it available in the gallery
			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// display in ImageView. We will resize the bitmap to fit the display
			// Loading the full sized image will consume to much memory 
			// and cause the application to crash.
			int height = Resources.DisplayMetrics.HeightPixels;
			int width = 500;

			//var scalledBitmap = Bitmap.createScaledBitmap(b, 120, 120, false)

			App.bitmap = App._file.Path.LoadAndResizeBitmap (500, 500);
			RunOnUiThread (() =>
				imgViewImage.SetImageBitmap (App.bitmap));
		}

		WallPost CreateWallPost()
		{
			WallPost post = new WallPost () {
				Title = inpText.Text,
				Image = GetImageBytes()
			};
			return post;
		}
		byte[] GetImageBytes()
		{
			byte[] bitmapData;
			using (var stream = new MemoryStream ()) {
				App.bitmap.Compress (Bitmap.CompressFormat.Png, 0, stream);
				bitmapData = stream.ToArray ();
			}

			byte[] resizedImage = ImageResizer.ResizeImage(bitmapData, 848, 480);
			return resizedImage;
		}
	}

	public static class App{
		public static Java.IO.File _file;
		public static Java.IO.File _dir;     
		public static Bitmap bitmap;
	}

	public static class BitmapHelpers
	{
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight
					? outHeight / height
					: outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}



	}

}

