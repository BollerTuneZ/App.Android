using System;
using Android.Widget;
using Android.App;
using BTZ.App.Data;
using System.Collections.Generic;
using Android.Views;
using System.IO;
using Android.Graphics;


namespace BTZ.App.ListAdapter
{
	public class WallPostAdapter : BaseAdapter
	{
		Activity _activity;
		readonly List<WallPost> _wallPosts;

		public WallPostAdapter (Activity _activity, List<WallPost> _wallPosts)
		{
			this._activity = _activity;
			this._wallPosts = _wallPosts;
		}


		#region implemented abstract members of BaseAdapter

		public override Java.Lang.Object GetItem (int position)
		{
			throw new NotImplementedException ();
		}

		public override long GetItemId (int position)
		{
			return _wallPosts [position].Id;
		}

		public override Android.Views.View GetView (int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
		{
			View view = _activity.LayoutInflater.Inflate (Resource.Layout.WallPostItem,null,false);

			#region Controlls
			TextView lbnHeader = view.FindViewById<TextView> (Resource.Id.wp_lbn_header);
			ImageView imgViewHeaderImage = view.FindViewById<ImageView> (Resource.Id.wp_imgView_header);
			TextView lbnContent = view.FindViewById<TextView> (Resource.Id.wp_lbn_content);
			ImageView imgViewMainImage = view.FindViewById<ImageView> (Resource.Id.wp_imgView_mainImage);
			#endregion


			var currentWallPost = _wallPosts [position];

			if (currentWallPost == null) {
				return view;
			}

			lbnHeader.Text = currentWallPost.Title;
			if (currentWallPost.HeaderImage != null) {

				imgViewHeaderImage.SetImageBitmap (bytesToBitmap (currentWallPost.HeaderImage));
			}

			lbnContent.Text = currentWallPost.Content;

			if (currentWallPost.Image != null) {
				imgViewMainImage.SetImageBitmap (bytesToBitmap (currentWallPost.Image));
			}

			return view;
		}

		public override int Count {
			get {
				return _wallPosts.Count;
			}
		}

		#endregion

		static Bitmap bytesToBitmap (byte[] imageBytes)
		{

			Bitmap bitmap = BitmapFactory.DecodeByteArray (imageBytes, 0, imageBytes.Length);

			return bitmap;
		}
	}
}

