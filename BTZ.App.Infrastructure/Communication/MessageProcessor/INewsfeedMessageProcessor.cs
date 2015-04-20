using System;
using BTZ.Common;
using BTZ.App.Data;

namespace BTZ.App.Infrastructure
{
	public interface INewsfeedMessageProcessor
	{

		event EventHandler OnUpdate;

		void GetAllNewsfeeds();

		void GetSingleNewsfeed(int id);

		void PostSingleNewsfeed(WallPost post);
	}
}

