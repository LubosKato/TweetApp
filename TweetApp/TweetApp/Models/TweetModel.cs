using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Web.Script.Serialization;
using TweetApp.Helpers;
using TweetApp.TweetService;
using TweetSharp;

[assembly: InternalsVisibleTo("TweetAppTests")]
namespace TweetApp.Models
{
    #region Models definition
    [Serializable]
    public class TweetModelContext
    {
        public List<TweetModel> TweetModels { get; set; }
    }

    [Serializable]
    public class TweetModel
    {
        public string Author { get; set; }
        public int TweetCount { get; set; }
        public int AccountNameCount { get; set; }
        public List<TweetModelDetails> TweetModelDetails { get; set; }
    }

    [Serializable]
    public class TweetModelDetails
    {
        public string Text { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy, HH:mm}")]
        public DateTime CreatedDate { get; set; }
    }
    #endregion

    #region TweetLogic
    public class TweetBase : ITweetBase
    {
        private readonly ITwitterServiceBase twitterService;
        private static readonly TweetModelContext tweets = new TweetModelContext();

        public TweetModelContext Tweets
        {
            get { return tweets; }
        }

        public string GetTweetsJson
        {
            get { return new JavaScriptSerializer().Serialize(tweets); }
        }

        public TweetBase(ITwitterServiceBase twitterService)
        {
            this.twitterService = twitterService;
        }

        public bool DownloadTweets(string accounts)
        {
            var tweetAccounts = new List<string>();
            if(accounts != null && !string.IsNullOrEmpty(accounts))
            {
                tweetAccounts = accounts.ProcessTweetString();
            }
            
            if (tweetAccounts != null)
            {
                twitterService.GetAuthenticatedService();
                DownloadTweetsInternal(tweetAccounts);
            }
            return true;
        }

        internal void DownloadTweetsInternal(List<string> tweetAccounts)
        {
            Tweets.TweetModels = new List<TweetModel>();
            try
            {
                foreach (string account in tweetAccounts)
                {
                    int accountNamesCount = 0;
                    var tweetsInList = twitterService.GetTweetsFromServiceAsync(account);
                    if (tweetsInList != null && tweetsInList.Count > 0)
                    {
                        var tweetModel = new TweetModel();
                        var tweetModelDetails = new List<TweetModelDetails>();
                        foreach (TwitterStatus tweet in tweetsInList)
                        {
                            var totalDays = (DateTime.Now - tweet.CreatedDate).TotalDays;
                            if (totalDays < DaysCount)
                            {
                                tweetModelDetails.Add(new TweetModelDetails
                                                          {
                                                              CreatedDate = tweet.CreatedDate,
                                                              Text = tweet.Text,
                                                          });
                                accountNamesCount += tweet.Text.AccountNameCount();
                            }
                        }
                        tweetModelDetails.Sort((y, x) => -1*DateTime.Compare(y.CreatedDate, x.CreatedDate));
                        tweetModel.TweetModelDetails = tweetModelDetails;
                        tweetModel.Author = account;
                        tweetModel.TweetCount = tweetModelDetails.Count;
                        tweetModel.AccountNameCount = accountNamesCount;
                        Tweets.TweetModels.Add(tweetModel);
                    }
                }
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

        #region configuration
        private static int DaysCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["DaysCount"]); }
        }
        #endregion
    }
    #endregion
}