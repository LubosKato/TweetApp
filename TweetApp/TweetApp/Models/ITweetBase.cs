namespace TweetApp.Models
{
    public interface ITweetBase
    {
        TweetModelContext Tweets { get; }
        bool DownloadTweets(string accounts);
    }
}