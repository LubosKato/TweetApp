using NUnit.Framework;
using TweetApp.TweetService;

namespace TweetAppTests
{
    [TestFixture]
    public class TwitterServiceTests : TestSetupBase
    {
        private TwitterServiceBase twitterService;

        [SetUp]
        public override void Setup()
        {
            twitterService = new TwitterServiceBase();
            twitterService.GetAuthenticatedService();
        }

        [Test]
        public void Test_Tweet_Service_Call()
        {
            Assert.NotNull(twitterService.GetAuthenticatedService());
        }

        [Test]
        public void Test_Tweet_Data_Retrieval_Synchronous_Valid()
        {
            var result = twitterService.GetTweetsFromService(validAccount);
            Assert.IsTrue(result.Count == TweetsCount);
        }

        [Test]
        public void Test_Tweet_Data_Retrieval_Valid()
        {
            var result = twitterService.GetTweetsFromServiceAsync(validAccount);
            Assert.IsTrue(result.Count == TweetsCount);
        }

        [Test]
        public void Test_Tweet_Data_Retrieval_Invalid_Input()
        {
            var result = twitterService.GetTweetsFromServiceAsync(inValidAccount);
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void Test_Tweet_Data_Retrieval_Empty_Input()
        {
            var result = twitterService.GetTweetsFromServiceAsync(string.Empty);
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void Test_Tweet_Data_Retrieval_Null_Input()
        {
            var result = twitterService.GetTweetsFromServiceAsync(null);
            Assert.IsTrue(result.Count == 0);
        }
    }
}