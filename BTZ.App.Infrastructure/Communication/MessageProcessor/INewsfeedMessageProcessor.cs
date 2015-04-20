using System;
using BTZ.Common;

namespace BTZ.App.Infrastructure
{
	public interface INewsfeedMessageProcessor
	{

		event EventHandler OnUpdate;

		void GetAllNewsfeeds();

		void GetSingleNewsfeed(int id);

		void PostSingleNewsfeed(NewsfeedDto dto);
	}
}

