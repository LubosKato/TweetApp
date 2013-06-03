using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using TweetApp.Models;

namespace TweetAppTests
{
    [TestFixture]
    public class TweetTests : TestSetupBase
    {
        private TweetBase _tweetBase;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            _tweetBase = new TweetBase(this.twitterServiceBaseStub);
        }

        [Test]
        public void Test_Download_Tweets_Valid()
        {
            var result = _tweetBase.DownloadTweets(validAccounts);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_Download__Tweets_Internal_Valid()
        {
            _tweetBase.DownloadTweetsInternal(accountsFake);
            twitterServiceBaseStub.AssertWasCalled(s => s.GetTweetsFromServiceAsync(validAccount), o => o.Repeat.Once());
            Assert.IsTrue(_tweetBase.Tweets.TweetModels.Count == 1);
            Assert.IsTrue(_tweetBase.Tweets.TweetModels[0].TweetModelDetails.Count == 2);
        }

        [Test]
        public void Test_Download__Tweets_Internal_EmptyList()
        {
            _tweetBase.DownloadTweetsInternal(new List<string>());
            twitterServiceBaseStub.AssertWasNotCalled(s => s.GetTweetsFromServiceAsync(validAccount));
            Assert.IsTrue(_tweetBase.Tweets.TweetModels.Count == 0);
        }

        [Test]
        public void Test_ProcessTweets_InValid()
        {
            var result = _tweetBase.DownloadTweets(invalidAccounts);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_ProcessTweets_EmptyString_Entry()
        {
            var result = _tweetBase.DownloadTweets(string.Empty);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test_ProcessTweets_Null_Entry()
        {
            var result = _tweetBase.DownloadTweets(null);
            Assert.IsTrue(result);
        }
    }
}
