using System.Web.Mvc;
using Castle.Core.Logging;
using NUnit.Framework;
using Rhino.Mocks;
using TweetApp.Controllers;
using TweetApp.Models;

namespace TweetAppTests
{
    [TestFixture]
    public class HomeControllerTest : TestSetupBase
    {
        private HomeController homeController;
        private HomeController homeControllerNull;
        private InputModel inputModel;
        private ITweetBase tweetBase;
        public ILogger Logger { get; set; }

        [SetUp]
        public void TestSetup()
        {
            this.tweetBase = MockRepository.GenerateStub<ITweetBase>();
            tweetBase.Stub(c => c.Tweets).Return(context);
            this.homeController = new HomeController(tweetBase);
            this.homeControllerNull = new HomeController();
            this.inputModel = new InputModel();
        }

        [Test]
        public void Test_Home()
        {
            // Act
            var result = homeController.AddTweets() as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
        }

        [Test]
        public void Test_Index()
        {
            // Arrange          
            const string expectedViewName = "Index";
            // Act
            var result = homeController.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
            Assert.That(homeController.ViewData.Model.GetType(), Is.EqualTo(typeof(TweetModelContext)));
        }

        [Test]
        public void Test_Index_Invalid()
        {
            // Arrange          
            const string expectedViewName = "NotFound";
            // Act
            var result = homeControllerNull.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
        }

        [Test]
        public void Test_IndexJson()
        {
            // Act
            var result = homeController.IndexJson();
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.That(homeController.ViewData.Model.GetType(), Is.EqualTo(typeof(TweetModelContext)));
        }

        [Test]
        public void Test_IndexJson_InValid()
        {
            // Arrange          
            const string expectedViewName = "NotFound";
            // Act
            var result = homeControllerNull.IndexJson() as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
        }

        [Test]
        public void Test_AddTweets_View_Get_Tweets_Valid()
        {
            //Arrange
            this.inputModel.InputValues = validAccounts;
            const string expectedRouteName = "Index";
            // Act
            var result = homeController.AddTweets("Get Tweets", inputModel) as RedirectToRouteResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a RedirectToRouteResult");
            Assert.AreEqual(expectedRouteName, result.RouteValues["action"], "v name should have been {0}", expectedRouteName);
        }

        [Test]
        public void Test_AddTweets_View_Get_Tweets_In_JSon_Valid()
        {
            //Arrange
            this.inputModel.InputValues = validAccounts;
            const string expectedRouteName = "IndexJson";
            // Act
            var result = homeController.AddTweets("Get Tweets In JSon", inputModel) as RedirectToRouteResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a RedirectToRouteResult");
            Assert.AreEqual(expectedRouteName, result.RouteValues["action"], "Route name should have been {0}", expectedRouteName);
        }

        [Test]
        public void Test_AddTweets_View_Get_Tweets_InValid()
        {
            //Arrange
            this.inputModel.InputValues = validAccounts;
            const string expectedViewName = "Error";
            // Act
            var result = homeController.AddTweets("Get Tweets In JSon", null) as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
        }

        [Test]
        public void Test_AddTweets_Null_InValid()
        {
            //Arrange
            this.inputModel.InputValues = validAccounts;
            const string expectedViewName = "Error";
            // Act
            var result = homeController.AddTweets(null, null) as ViewResult;
            // Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
        }
    }
}