using NUnit.Framework;
using TweetApp.Helpers;

namespace TweetAppTests
{
    [TestFixture]
    public class TestHelpers : TestSetupBase
    {
        [Test]
        public void Test_Word_Count_In_Text()
        {
            var result = validAccounts.ProcessTweetString();
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void Test_Remaining_WhiteSpace_In_Text()
        {
            var result = validAccounts.ProcessTweetString();
            Assert.IsFalse(result[0].Contains(" "));
        }

        [Test]
        public void Test_Invalid_Text()
        {
            var result = invalidAccounts.ProcessTweetString();
            Assert.IsTrue(result.Count == 1);
        }

        [Test]
        public void Test_Valid_Input_With_Text_After_account()
        {
            var result = invalidAccounts1.ProcessTweetString();
            Assert.AreEqual(result.Count, 3);
        }

        [Test]
        public void Test_Emtpty_Text()
        {
            var result = string.Empty.ProcessTweetString();
            Assert.IsTrue(result.Count == 0);
        }

        [Test]
        public void Test_Accounts_Count_In_Text()
        {
            var result = validTweedText.AccountNameCount();
            Assert.AreEqual(result, 5);
        }

        [Test]
        public void Test_Accounts_Count_In_Text_With_Email()
        {
            var result = validTweedTextWithEmail.AccountNameCount();
            Assert.AreEqual(result, 2);
        }

        [Test]
        public void Test_Accounts_Count_EmptyText()
        {
            var result = string.Empty.AccountNameCount();
            Assert.AreEqual(result, 0);
        } 
    }
}