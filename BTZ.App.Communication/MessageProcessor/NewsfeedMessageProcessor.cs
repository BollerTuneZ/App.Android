using System;
using BTZ.App.Infrastructure;
using System.Threading;
using BTZ.Common;
using Newtonsoft.Json;
using Android.Util;
using AutoMapper;
using BTZ.App.Data;
using System.Collections.Generic;

namespace BTZ.App.Communication
{
	/// <summary>
	/// Jonas Ahlf 20.04.2015 22:10:28
	/// </summary>
	public class NewsfeedMessageProcessor : INewsfeedMessageProcessor
	{
		const string Tag = "NewsfeedMessageProcessor";
		readonly IPrivateRepository _privateRepo;
		readonly INewsfeedRepository _newsfeedRepo;
		readonly RemoteConnection _remoteConnection;

		public NewsfeedMessageProcessor (IPrivateRepository _privateRepo, INewsfeedRepository _newsfeedRepo)
		{
			this._privateRepo = _privateRepo;
			this._newsfeedRepo = _newsfeedRepo;
			this._remoteConnection = new RemoteConnection ();
			#region Mappings
			Mapper.CreateMap<NewsfeedDto,WallPost>();
			Mapper.CreateMap<WallPost,NewsfeedDto>();

			#endregion
		}
		

		#region INewsfeedMessageProcessor implementation

		public event EventHandler OnUpdate;

		public void GetAllNewsfeeds ()
		{
			new Thread (() => {
				NewsfeedRequest request = new NewsfeedRequest()
				{
					RequestType = RequestType.GetAll
				};

				BaseDto bDto = new BaseDto{
					Type = DtoType.Newsfeed,
					JsonObject = JsonConvert.SerializeObject(request)
				};

				string result = _remoteConnection.Request(bDto);

				if (String.IsNullOrEmpty(result)) {
					Log.Error(Tag,"GetAllWallposts error result is null");
					return;
				}

				var arrayMessage = JsonConvert.DeserializeObject<ArrayMessage>(result);

				foreach (var id in arrayMessage.Ids) {
					GetSingleNewsfeed(id);
				}

			}).Start ();
		}

		public void GetSingleNewsfeed (int id)
		{
			
			new Thread (() => {
				NewsfeedRequest request = new NewsfeedRequest()
				{
					RequestType = RequestType.GetSingle,
					Id = id
				};

				BaseDto bDto = new BaseDto{
					Type = DtoType.Newsfeed,
					JsonObject = JsonConvert.SerializeObject(request)
				};
				string result = _remoteConnection.Request(bDto);
				if (String.IsNullOrEmpty(result)) {
					Log.Error(Tag,"GetSingleWallpost error result is null");
					return;
				}

				NewsfeedDto dto = JsonConvert.DeserializeObject<NewsfeedDto>(result);

				if(dto == null)
				{
					Log.Error(Tag,String.Format("Could not parse newsfeeddto {0}",result));
					return;
				}

				WallPost post = Mapper.Map<NewsfeedDto,WallPost>(dto);

				_newsfeedRepo.AddWallPosts(new List<WallPost>(new []{post}));
				OnUpdate(this,null);
			}).Start ();
		}

		public void PostSingleNewsfeed (WallPost post)
		{
			new Thread (() => {

				NewsfeedDto dto = Mapper.Map<WallPost,NewsfeedDto>(post);

				NewsfeedRequest request = new NewsfeedRequest()
				{
					RequestType = RequestType.PostSingle,
					SingleDto = dto,
					Token = _privateRepo.GetLocalUser().Token
				};

				BaseDto bDto = new BaseDto{
					Type = DtoType.Newsfeed,
					JsonObject = JsonConvert.SerializeObject(request)
				};
				string result = _remoteConnection.Request(bDto);
				if (String.IsNullOrEmpty(result)) {
					Log.Error(Tag,"GetSingleWallpost error result is null");
					return;
				}

				var resultObject = JsonConvert.DeserializeObject<NewsfeedDto>(result);

				if (resultObject.Error != null) {
					Log.Error(Tag, resultObject.Error.Message);
				}

			}).Start ();

		}

		#endregion


	}
}

