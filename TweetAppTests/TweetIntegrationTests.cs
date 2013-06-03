using NUnit.Framework;
using TweetApp;
using TweetApp.Models;
using TweetApp.TweetService;

namespace TweetAppTests
{
    [TestFixture]
    public class TweetIntegrationTests : TestSetupBase
    {
        private TweetBase _tweetBase;

        [SetUp]
        public override void Setup()
        {
            _tweetBase = new TweetBase(new TwitterServiceBase());
        }

        [Test]
        public void Test_ProcessTweets_Valid()
        {
            var result = _tweetBase.DownloadTweets(validAccounts);
            Assert.IsTrue(result);
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
