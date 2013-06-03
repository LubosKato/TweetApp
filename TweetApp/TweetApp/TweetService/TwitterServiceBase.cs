using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using TweetSharp;

namespace TweetApp.TweetService
{
    public class TwitterServiceBase : ITwitterServiceBase
    {
        #region constructor
        private static TwitterService twitterService;

        public TwitterServiceBase()
        {
            twitterService = new TwitterService();
        }
        #endregion

        #region Twitter Service
        public TwitterService GetAuthenticatedService()
        {
            var twitterClientInfo = new TwitterClientInfo {ConsumerKey = ConsumerKey, ConsumerSecret = ConsumerSecret};
            twitterService = new TwitterService(twitterClientInfo);
            twitterService.AuthenticateWith(AccessToken, AccessTokenSecret);
            return twitterService;
        }

        [Obsolete]
        public List<TwitterStatus> GetTweetsFromService(string account)
        {
            var accountOptions = new ListTweetsOnUserTimelineOptions {ScreenName = account, Count = TweetsCount};
            return twitterService.ListTweetsOnUserTimeline(accountOptions).ToList();
        }

        public List<TwitterStatus> GetTweetsFromServiceAsync(string account)
        {
            if(account == null || string.IsNullOrEmpty(account))
                return new List<TwitterStatus>();
            try
            {
                var accountOptions = new ListTweetsOnUserTimelineOptions { ScreenName = account, Count = TweetsCount };
                IAsyncResult asyncresult = twitterService.BeginListTweetsOnUserTimeline(accountOptions);
                var result = twitterService.EndListTweetsOnHomeTimeline(asyncresult, TimeOut);
                if (!asyncresult.IsCompleted)
                    throw new TimeoutException();
                return result == null ? new List<TwitterStatus>() : result.ToList();
            }
            catch (TimeoutException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region ConsumerKey & ConsumerSecret
        private static string ConsumerSecret
        {
            get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
        }
        private static string ConsumerKey
        {
            get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
        }

        private static string AccessToken
        {
            get { return ConfigurationManager.AppSettings["AccessToken"]; }
        }
        private static string AccessTokenSecret
        {
            get { return ConfigurationManager.AppSettings["AccessTokenSecret"]; }
        }
        private static int TweetsCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["TweetsCount"]); }
        }
        private static TimeSpan TimeOut
        {
            get { return new TimeSpan(0,0,Convert.ToInt32(ConfigurationManager.AppSettings["TimeOut"]));  }
        }
        #endregion 
    }
}