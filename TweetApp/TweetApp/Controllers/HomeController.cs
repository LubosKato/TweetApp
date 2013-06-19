using System;
using System.Web.Mvc;
using Castle.Core.Logging;
using TweetApp.Models;
using TweetApp.TweetService;

namespace TweetApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITweetBase model;
        public ILogger Logger { get; set; }

        public HomeController() : this(new TweetBase(new TwitterServiceBase())) {}

        public HomeController(ITweetBase model)
        {
            this.model = model;
        }       

        // GET: /Home/
        [HttpGet]
        public ActionResult AddTweets()
        {
            return View("AddTweets");
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            TweetModelContext models = model.Tweets;
            if (models.TweetModels == null || models.TweetModels != null && models.TweetModels.Count == 0)
                return View("NotFound");
            return View("Index", models);
        }

        [HttpGet]
        public virtual ActionResult IndexJson()
        {
            TweetModelContext models = model.Tweets;
            if (models.TweetModels == null || models.TweetModels != null && models.TweetModels.Count == 0)
                return View("NotFound");
            return this.Json(View(models), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public virtual ActionResult AddTweets(string btnSubmit, InputModel inputModel)
        {
            try
            {
                model.DownloadTweets(inputModel.InputValues);
            }
            catch (TimeoutException)
            {
                if (Logger != null)
                    Logger.ErrorFormat("Twitter service timeout !");
                return View("Timeout");
            }
            catch (Exception ex)
            {
                if (Logger != null)
                    Logger.ErrorFormat("Tweet app experienced unexpected error. Message: {0}, StackTrace: {1}", ex.Message, ex.StackTrace);
                return View("Error");
            }
            
            if (ModelState.IsValid)
            {
                switch (btnSubmit)
                {
                    case "Get Tweets":
                        return (RedirectToAction("Index"));
                    case "Get Tweets In JSon":
                        return (RedirectToAction("IndexJson"));
                    default:
                        return (View("AddTweets"));
                }
            }
            return View("AddTweets");
        }
    }
}
