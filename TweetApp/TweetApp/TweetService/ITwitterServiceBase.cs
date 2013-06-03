using System.Collections.Generic;
using TweetSharp;

namespace TweetApp.TweetService
{
    public interface ITwitterServiceBase
    {
        TwitterService GetAuthenticatedService();
        List<TwitterStatus> GetTweetsFromService(string account);
        List<TwitterStatus> GetTweetsFromServiceAsync(string account);
    }
}