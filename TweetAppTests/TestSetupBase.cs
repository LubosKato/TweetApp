using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Castle.MicroKernel;
using Castle.Windsor;
using NUnit.Framework;
using Rhino.Mocks;
using TweetApp.Controllers;
using TweetApp.Models;
using TweetApp.TweetService;
using TweetSharp;

namespace TweetAppTests
{
    [TestFixture]
    public class TestSetupBase
    {
        protected const string validAccounts = "@pay_by_phone, @PayByPhone, @PayByPhone_UK";
        protected const string invalidAccounts = "@pay_by_phone. @PayByPhoneX@PayByPhone_UK";
        protected const string invalidAccounts1 = "@pay_by_phone, @PayByPhone adasdadasd, @PayByPhone_UK";
        protected const string validTweedText = "@pay_by_phone. @PayByPhoneX@PayByPhone_UK @pay_by_phone asdasd @pay_by_phone"; //should return count 5
        protected const string validTweedTextWithEmail = "@pay_by_phone. and @pay and TweedText@email.com asdasdad"; // count 2
        protected const string validAccount = "@pay_by_phone";
        protected const string inValidAccount = "ay_by_phone";

        protected ITwitterServiceBase twitterServiceBaseStub;
        protected List<TwitterStatus> tweetsFake = new List<TwitterStatus>();
        protected List<string> accountsFake = new List<string>();
        protected TweetModelContext context = new TweetModelContext();

        [SetUp]
        public virtual void Setup()
        {
            accountsFake = GetAccountsFake();
            tweetsFake = GetFakeTweets();
            this.twitterServiceBaseStub = MockRepository.GenerateStub<ITwitterServiceBase>();
            twitterServiceBaseStub.Stub(c => c.GetTweetsFromServiceAsync(validAccount)).Return(tweetsFake);
            //context.TweetModels = new List<TweetModel>();
            context.TweetModels = GetFakeContext();
        }

        private List<TweetModel> GetFakeContext()
        {
            var result = new List<TweetModel>();
            result.Add(new TweetModel()
                           {
                               AccountNameCount = 1,
                               Author = "pay_by_phone",
                               TweetCount = 2,
                               TweetModelDetails =
                                   new List<TweetModelDetails>()
                                       {new TweetModelDetails() {CreatedDate = DateTime.Now, Text = validTweedText}}
                           });
            return result;
        }

        private List<TwitterStatus> GetFakeTweets()
        {
            var dateTimeBeforeTwoWeeks = DateTime.Today.AddDays(-14);
            tweetsFake.Add(new TwitterStatus() { Text = validTweedText, CreatedDate = DateTime.Now });
            tweetsFake.Add(new TwitterStatus() { Text = validTweedTextWithEmail, CreatedDate = Convert.ToDateTime("5/31/2013 7:26:19 PM") });
            tweetsFake.Add(new TwitterStatus() { Text = validTweedText, CreatedDate = dateTimeBeforeTwoWeeks });
            return tweetsFake;
        }

        private List<string> GetAccountsFake()
        {
            accountsFake.Add("@pay_by_phone");
            accountsFake.Add("@PayByPhone");
            accountsFake.Add("@PayByPhone_UK");
            return accountsFake;
        }

        protected static int TweetsCount
        {
            get { return Convert.ToInt32(ConfigurationManager.AppSettings["TweetsCount"]); }
        }

        protected IHandler[] GetAllHandlers(IWindsorContainer container)
        {
            return GetHandlersFor(typeof(object), container);
        }

        protected IHandler[] GetHandlersFor(Type type, IWindsorContainer container)
        {
            return container.Kernel.GetAssignableHandlers(type);
        }

        protected Type[] GetImplementationTypesFor(Type type, IWindsorContainer container)
        {
            return GetHandlersFor(type, container)
                .Select(h => h.ComponentModel.Implementation)
                .OrderBy(t => t.Name)
                .ToArray();
        }
    }
}