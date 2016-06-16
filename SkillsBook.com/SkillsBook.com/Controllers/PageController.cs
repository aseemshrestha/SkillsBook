using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SkillsBook.Models.Models;
using SkillsBook.Models.DAL;
using SkillsBook.Models.ViewModel;
using System.Transactions;


namespace SkillsBook.com.Controllers
{
    public class PageController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        public async Task<ActionResult> GetTags()
        {
            var tagsTrending = CacheImplementation.GetTrendingItems(Constants.TagsGeneral);
            var tagsAnnouncement = CacheImplementation.GetTrendingItems(Constants.TagsAnnouncement);
            await Task.WhenAll(tagsAnnouncement, tagsTrending);
            ViewBag.TagsGeneral = tagsTrending.Result;
            ViewBag.TagsAnnouncement = tagsAnnouncement.Result;
            return View();
        }

        [HttpGet]
        public ActionResult DisplayClassified(int classifiedId)
        {
            var model = _unitOfWork.ClassifiedRepository.GetById(classifiedId);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> DisplayPageByTags(string tagType, string cat, int size = 25)
        {
            var type = cat.Split('(')[0];
            IEnumerable<ThreadModel> tagsList;
            var query = "Select TOP " + Constants.BlocksizeInitialLoad + " * from " + Constants.SchemaName +
                        " [Threads]";
            switch (tagType)
            {
                case "TagsGeneral":
                    query = query + " Where TagsGeneral = " + "'" + type + "'" + " order by SubmittedOn desc";
                    tagsList = _unitOfWork.ThreadRepository.GetWithRawSql(query);
                    break;
                default:
                    query = query + " Where TagsAnnouncement = " + "'" + type + "'" + " order by SubmittedOn desc";
                    tagsList = _unitOfWork.ThreadRepository.GetWithRawSql(query);
                    break;
            }
            var allThreads = CacheImplementation.GetRecentThreads;
            var mostLiked = CacheImplementation.GetMostLikedThreads;
            var mostViewed = CacheImplementation.GetMostViewedThreads;
            var mostResponded = CacheImplementation.GetMostCommentedThreads;
            var trendingGeneral = CacheImplementation.GetTrendingItems(Constants.TagsGeneral);
            var trendingAnnouncement = CacheImplementation.GetTrendingItems(Constants.TagsAnnouncement);
            await Task.WhenAll(allThreads, mostLiked, mostViewed, mostResponded, trendingGeneral, trendingAnnouncement);
            ViewBag.MostLiked = mostLiked.Result;
            ViewBag.MostViewed = mostViewed.Result;
            ViewBag.MostResponded = mostResponded.Result;
            ViewBag.TrendingTagsGeneral = trendingGeneral.Result.Take(Constants.BlocksizeTrendingTags);
            ViewBag.TrendingTagsAnnouncement = trendingAnnouncement.Result.Take(Constants.BlocksizeTrendingTags);

            return View(tagsList);
        }

        [HttpGet]
        public ActionResult DisplayAllClassifieds(string type,int page = 1)
        {
            var classifieds = _unitOfWork.ClassifiedRepository.Get(x => x.ClassifiedCategory.Equals(type)).OrderByDescending(x=>x.LastUpdated);
            int totalCount = classifieds.Count();
            var offset = (page - 1) * Constants.BlocksizeMax;
           
            var model =
                classifieds.Skip((int)offset)
                       .Take(
                           (int)
                               (offset > Constants.BlocksizeMax
                                   ? offset - (offset - Constants.BlocksizeMax)
                                   : offset ==0 ? Constants.BlocksizeMax: offset));
            ViewBag.Cat = type;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = ((totalCount - 1) / Constants.BlocksizeMax) + 1;
            return View(model);

        }

        [HttpGet]
        public ActionResult GetMorePostByCategory(string type, string category, int size = 25)
        {
            List<ThreadModel> posts = null;
            var repo = _unitOfWork.ThreadRepository;

            if (type == Constants.TagsGeneral)
            {
                posts = repo.Get(x => x.TagsGeneral.Equals(category))
                      .OrderByDescending(x => x.SubmittedOn)
                      .Skip(size)
                      .Take(size > Constants.BlocksizeInitialLoad ? size - (size - Constants.BlocksizeInitialLoad) : size)
                      .ToList();
            }
            else if (type == Constants.TagsAnnouncement)
            {
                posts = repo.Get(x => x.Announcement.Equals(category))
                      .OrderByDescending(x => x.SubmittedOn)
                      .Skip(size)
                      .Take(size > Constants.BlocksizeInitialLoad ? size - (size - Constants.BlocksizeInitialLoad) : size)
                      .ToList();
            }
            var count = posts.Count;
            ViewBag.Count = count;
            switch (count)
            {
                case 0:
                    return null;
            }
            return PartialView("Partial.PageByCategory", posts);
        }

        [HttpGet]
        public ActionResult AddThread()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitComment(string comment, int threadId)
        {
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            SkillsBook.Models.Models.ThreadModel threadRow = null;
            try
            {

                using (scope)
                {
                    var commentModel = new CommentModel
                    {
                        Thread_ThreadId = threadId,
                        CommentedBy = Request.Cookies["LogOnCookie"] == null
                            ? "Anonymous"
                            : Request.Cookies["LogOnCookie"].Values["username"],
                        CommentedOn = DateTime.Now,
                        IpAddress = GetIp(),
                        Browser = Request.Browser.Browser,
                        Comment = comment
                    };

                    _unitOfWork.CommentRepository.Insert(commentModel);
                    threadRow = _unitOfWork.ThreadRepository.GetById(threadId);
                    threadRow.Responses = threadRow.Responses + 1;
                    threadRow.LastUpdated = DateTime.Now;
                    _unitOfWork.ThreadRepository.Update(threadRow);
                    //CacheImplementation.UpdateCacheItem(CacheImplementation.RecentThreads, threadId, "Comments", null);
                   // CacheImplementation.UpdateCacheItem(CacheImplementation.MostCommentedThreads, threadId, "Comments", null);
                    CacheImplementation.ClearSpecificCacheObject(CacheImplementation.RecentThreads);
                    CacheImplementation.ClearSpecificCacheObject(CacheImplementation.MostCommentedThreads);

                    _unitOfWork.Save();
                    // notifyWatchers = true;
                    scope.Complete();
                    _unitOfWork.SendEmail(threadRow.Email, "Someone just commented in your thread",
                        "Dear " + threadRow.Username +
                        "<br /> Someone just commented on yout thread. <br /> Visit the link below to see the comment.<br />" +
                         Constants.Domain+"Page/DisplayThread?threadId="+threadRow.ThreadId+"<br /><br />"+
                        " With Best Regards <br />"+
                        Constants.Domain+"<br />"+Constants.Slogan);
                }
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                scope.Dispose();
                return Json(new { Result = "failed" });
            }
            return Json(new
            {
                Result = "success",
                TotalCount = threadRow.Responses,
                Comments = comment,
                CommentedBy = Request.Cookies["LogOnCookie"] == null
                    ? "Anonymous"
                    : Request.Cookies["LogOnCookie"].Values["username"],
                CommentedOn = DateTime.Now.ToLongDateString()
            });
        }



        public ActionResult GetMorePost(int size = 25)
        {
            var posts = _unitOfWork.ThreadRepository.Get(x => x.ThreadId > 0)
                         .OrderByDescending(x => x.LastUpdated)
                         .Skip(size)
                         .Take(size > Constants.BlocksizeInitialLoad ? size - (size - Constants.BlocksizeInitialLoad) : size)
                         .ToList();
            switch (posts.Count)
            {
                case 0:
                    return null;
            }
            return PartialView("Partial.GetMorePost", posts);

        }

        public ActionResult GetMorePostByUser(string username, int size = 15)
        {
            var posts = _unitOfWork.ThreadRepository.Get(x => x.Username == username)
                        .OrderByDescending(x => x.SubmittedOn)
                        .Skip(size).Take(size > Constants.BlocksizeUserSpecificThreads ? size - (size - Constants.BlocksizeUserSpecificThreads) : size)
                        .ToList();

            return Json(posts, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DisplaySinglePost(int threadId)
        {
            var threadSingle = _unitOfWork.ThreadRepository.GetById(threadId);
            var comments = _unitOfWork.CommentRepository.Get(x => x.Thread_ThreadId == threadId).OrderByDescending(x => x.CommentedOn).ToList();
            ViewBag.Comments = comments;
            return PartialView("Partial.SinglePost", threadSingle);
        }

        [HttpGet]
        public ActionResult Delete(int threadId)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var username = httpCookie.Values["username"];
            var row = _unitOfWork.ThreadRepository.Get(x => x.ThreadId == threadId && x.Username.Equals(username)).SingleOrDefault();

            if (row == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (row.Likes > 0)
            {
                return Json(new { Result = "N", Message = "Since your post has been been liked, you cannot delete your post." }, JsonRequestBehavior.AllowGet);
            }
            if (row.Responses > 0)
            {
                return Json(new { Result = "N", Message = "Since your post has already been commented, you cannot delete your post." }, JsonRequestBehavior.AllowGet);
            }
            if (row.Watch > 0)
            {
                return Json(new { Result = "N", Message = "Since your post is already being watched, you cannot delete your post." }, JsonRequestBehavior.AllowGet);
            }
            var rowViews = _unitOfWork.ViewsRepository.Get(x => x.ThreadId == threadId).ToList();
            foreach (var item in rowViews)
            {
                _unitOfWork.ViewsRepository.Delete(item);
            }
            _unitOfWork.ThreadRepository.Delete(threadId);
            _unitOfWork.Save();
            CacheImplementation.ClearSpecificCacheObject(CacheImplementation.RecentThreads);
            return Json(new { Result = "Y", Message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DisplayThread(int threadId, bool isLiked = false, bool isWatched = false)
        {

            //  var thread = _unitOfWork.ThreadWithComments(threadId);
            //lazy to change views
            //will optimize later to one call

            var threadSingle = _unitOfWork.ThreadRepository.GetById(threadId);
            var comments = _unitOfWork.CommentRepository.Get(x => x.Thread_ThreadId == threadId).OrderByDescending(x => x.CommentedOn).ToList();

            // get other post by same user
            var totalPostCountByUser = _unitOfWork.ThreadRepository.Get(x => x.Username == threadSingle.Username).Count();
            var posts = _unitOfWork.ThreadRepository.Get(x => x.Username == threadSingle.Username).OrderByDescending(x => x.SubmittedOn).Take(Constants.BlocksizeUserSpecificThreads).ToList();

            if (isLiked)
            {
                ViewBag.Liked = true;
            }
            if (isWatched)
            {
                ViewBag.Watched = true;
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            try
            {
                using (scope)
                {
                    threadSingle.Views = threadSingle.Views + 1;
                    _unitOfWork.ThreadRepository.Update(threadSingle);
                    _unitOfWork.Save();

                    var viewsModel = new ViewsModel()
                    {
                        ThreadId = threadId,
                        ViewedBy = Request.Cookies["LogOnCookie"] == null ? "Anonymous" : Request.Cookies["LogOnCookie"].Values["username"],
                        ViewedOn = DateTime.Now,
                        IpAddress = GetIp(),
                        Browser = Request.Browser.Browser
                    };
                    //only logged in users are recorded
                    if (Request.Cookies["LogOnCookie"] != null)
                    {
                        _unitOfWork.ViewsRepository.Insert(viewsModel);
                        _unitOfWork.Save();
                    }
                  
                    ViewBag.TotalPostByUser = totalPostCountByUser;
                    ViewBag.Comments = comments;
                    ViewBag.Posts = posts;
                    ViewBag.CurrentPage = 1;

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                scope.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            // Response.Cache.SetCacheability(HttpCacheability.NoCache);
            CacheImplementation.UpdateCacheItem(CacheImplementation.RecentThreads, threadId, "Views", null);
            CacheImplementation.UpdateCacheItem(CacheImplementation.MostViewedThreads, threadId, "Views", null);
            //CacheImplementation.UpdateCacheItem(CacheImplementation.MostLikedThreads, threadId, "Likes", null);
            CacheImplementation.UpdateCacheItem(CacheImplementation.MostCommentedThreads, threadId, "Comments", null);
            return View(threadSingle);
        }

        public JsonResult GetLikedThreads()
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null)
            {
                var username = httpCookie.Values["username"];
                var likedThreads = _unitOfWork.ThreadsILiked(username);
                var watachedThreads = _unitOfWork.ThreadsIWatched(username);

                return Json(new { LikedThreads = String.Join(",", likedThreads), WatchedThreads = String.Join(",", watachedThreads) });
            }
            return null;
        }

        [HttpPost]
        public ActionResult Like(int threadId)
        {
            if (Request.Cookies["LogOnCookie"] == null)
            {
                var result = new { IsAuthenticated = false };
                return Json(result);
            }

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            try
            {
                int totalLikesByUser = 0;
                using (scope)
                {
                    var threadModel = _unitOfWork.ThreadRepository.GetById(threadId);
                    threadModel.Likes = threadModel.Likes + 1;
                    totalLikesByUser = threadModel.Likes;
                    _unitOfWork.ThreadRepository.Update(threadModel);
                    _unitOfWork.Save();

                    var likeModel = new LikeModel
                    {
                        ThreadId = threadId,
                        LikedBy = Request.Cookies["LogOnCookie"].Values["username"],
                        LikedOn = DateTime.Now,
                        IpAddress = GetIp(),
                        Browser = Request.Browser.Browser
                    };
                    _unitOfWork.LikeRepository.Insert(likeModel);
                    _unitOfWork.Save();
                    CacheImplementation.UpdateCacheItem(CacheImplementation.RecentThreads, threadId, "Likes", "ADD");
                    CacheImplementation.UpdateCacheItem(CacheImplementation.MostLikedThreads, threadId, "Likes", "ADD");
                    scope.Complete();
                }

                return Json(new { Result = "Success", IsAuthenticated = true, totalLikes = totalLikesByUser }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult Watch(int threadId)
        {
            if (Request.Cookies["LogOnCookie"] == null)
            {
                var result = new { IsAuthenticated = false };
                return Json(result);
            }

            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            try
            {
                int totalWatchByUser = 0;
                using (scope)
                {

                    var threadModel = _unitOfWork.ThreadRepository.GetById(threadId);
                    threadModel.Watch = threadModel.Watch + 1;
                    totalWatchByUser = threadModel.Watch;
                    _unitOfWork.ThreadRepository.Update(threadModel);
                    _unitOfWork.Save();
                    var watchModel = new WatchModel
                    {
                        ThreadId = threadId,
                        WatchedBy = Request.Cookies["LogOnCookie"].Values["username"],
                        WatchedOn = DateTime.Now,
                        IpAddress = GetIp(),
                        Browser = Request.Browser.Browser
                    };
                    _unitOfWork.WatchRepository.Insert(watchModel);
                    _unitOfWork.Save();
                    CacheImplementation.UpdateCacheItem(CacheImplementation.RecentThreads, threadId, "Watch", null);
                    scope.Complete();
                }
                return Json(new { Result = "Success", IsAuthenticated = true, totalWatch = totalWatchByUser }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult AddThreadPost(AddThreadViewModel addThreadViewModel)
        {
            bool isLoggedIn = false;
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null)
            {
                isLoggedIn = true;
            }

            ViewBag.Username = addThreadViewModel.Username;
            ViewBag.Email = addThreadViewModel.Email;
            if (addThreadViewModel.ThreadRadio == "announcement")
            {
                if (String.IsNullOrEmpty(addThreadViewModel.Announcement) || String.IsNullOrEmpty(addThreadViewModel.TagsAnnouncement))
                {
                    return View("AddThread");
                }
                ViewBag.Announcement = addThreadViewModel.Announcement;
                ViewBag.TagsA = addThreadViewModel.TagsAnnouncement;

            }
            if (addThreadViewModel.ThreadRadio == "generalThread")
            {
                if (String.IsNullOrEmpty(addThreadViewModel.Title) || String.IsNullOrEmpty(addThreadViewModel.Content) ||
                    String.IsNullOrEmpty(addThreadViewModel.TagsGeneral))
                {
                    return View("AddThread");
                }
                ViewBag.Title = addThreadViewModel.Title;
                ViewBag.Content = addThreadViewModel.Content;
                ViewBag.TagsG = addThreadViewModel.TagsGeneral;
            }
            if (!isLoggedIn)
            {
                if (_unitOfWork.DoesUserNameOrEmailExist(addThreadViewModel.Username) == 1)
                {
                    ViewBag.Error = "Username already exists!";
                    return View("AddThread");
                }
                if (_unitOfWork.DoesUserNameOrEmailExist(addThreadViewModel.Email) == 2)
                {
                    ViewBag.Error = "Email already exists!";
                    return View("AddThread");
                }
            }
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions(){
                IsolationLevel = IsolationLevel.ReadCommitted
              }
             );
            try {
                using (scope){
                    if (!isLoggedIn){
                        var pass = System.Web.Security.Membership.GeneratePassword(10, 2);
                        var passencrypted = Encryption.Encrypt(pass);
                        var userModel = new UserModel
                        {
                            Username = ViewBag.Username,
                            Email = ViewBag.Email,
                            Password = passencrypted,
                            ConfirmPassword = pass,
                            IpAddress = GetIp(),
                            Browser = Request.Browser.Browser,
                            CreatedOn = DateTime.Now,
                            LastSuccessfulLogin = DateTime.Now
                        };
                        _unitOfWork.UserRepository.Insert(userModel);
                        _unitOfWork.Save();
                        _unitOfWork.SendEmail(ViewBag.Email, "Your password for " + Constants.Domain, "Dear " + ViewBag.Username + ",<br />" +
                         "You just posted a thread and thank you for your contribution. <br />Please find the auto generated temporary password." +
                          "<br />Temporary Password:" + pass +
                          "<br />You may want to login with this and change it once you login.<br /><br /> Thanking you <br />" + Constants.Domain+"<br/>"+Constants.Slogan);
                    }
                    if (addThreadViewModel.ThreadRadio == null && addThreadViewModel.TagsAnnouncement == null &&
                        addThreadViewModel.TagsGeneral == null)
                    {
                        var cm = new ClassifiedModel();
                        var objClassifiedModel = PopulateClassifiedModel(cm, addThreadViewModel);
                        _unitOfWork.ClassifiedRepository.Insert(objClassifiedModel);
                        _unitOfWork.Save();
                        CacheImplementation.ClearSpecificCacheObject(CacheImplementation.Classifieds);
                    }
                    else
                    {
                        var threadModel = new ThreadModel
                        {
                            Username = ViewBag.Username,
                            Email = ViewBag.Email,
                            ThreadRadio = addThreadViewModel.ThreadRadio,
                            Announcement = addThreadViewModel.Announcement,
                            TagsAnnouncement = addThreadViewModel.TagsAnnouncement,
                            Title = addThreadViewModel.Title,
                            Content = addThreadViewModel.Content,
                            TagsGeneral = addThreadViewModel.TagsGeneral,
                            IpAddress = GetIp(),
                            Browser = Request.Browser.Browser,
                            SubmittedOn = DateTime.Now,
                            LastUpdated = DateTime.Now,
                            Likes = 0,
                            Views = 0,
                            Responses = 0
                        };
                        _unitOfWork.ThreadRepository.Insert(threadModel);
                        _unitOfWork.Save();
                        CacheImplementation.ClearSpecificCacheObject(CacheImplementation.RecentThreads);
                    }
                    
                    scope.Complete();
                }

                ViewBag.Username = null;
                ViewBag.Email = null;
                ViewBag.Announcement = null;
                ViewBag.TagsA = null;
                ViewBag.Title = null;
                ViewBag.Content = null;
                ViewBag.TagsG = null;

            }
            catch (Exception ex)
            {
                scope.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            var cookieLogOn = new HttpCookie("LogOnCookie");
            cookieLogOn.Values["username"] = addThreadViewModel.Username;
            cookieLogOn.Values["email"] = addThreadViewModel.Email;
            cookieLogOn.Expires = DateTime.Now.AddDays(2);
            Response.Cookies.Add(cookieLogOn);

            return RedirectToAction("Index", "Home");
        }

        private ClassifiedModel PopulateClassifiedModel<T>(T type, AddThreadViewModel input) where T : ClassifiedModel
        {
            type.Username = input.Username;
            type.EmailAddress = input.Email;
            type.ClassifiedCategory = input.ClassifiedCategory;
            type.ClassifiedTitle = input.ClassifiedTitle;
            type.HousingType = input.HousingType;
            type.BuySellItem = input.BuySellItem;
            type.Price = input.Price;
            type.AdditionalInfo = input.AdditionalInfo;
            type.City = input.City;
            type.State = input.State;
            type.Country = input.Country;
            type.JobDetails = input.JobDetails;
            type.PostedOn = DateTime.Now;
            type.IpAddress = GetIp();
            type.Browser = Request.Browser.Browser;
            type.ClassifiedRadio = input.ClassifiedRadio;
            type.LastUpdated = DateTime.Now;
            type.CurrentStatus = Constants.ClassifiedItemAvailable; //available
            if (Request.Files.Count > 0)
            {
                var pathParent = Server.MapPath("~/Content/CI");
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase httpPostedFileBase = Request.Files[i];
                    if (httpPostedFileBase == null || httpPostedFileBase.ContentLength < 1) continue;
                    var fileName = Path.GetFileName(httpPostedFileBase.FileName);
                    var imageUrl = pathParent + "//" +fileName;
                    httpPostedFileBase.SaveAs(imageUrl);
                    type.ClassifiedImageLoc += fileName + "@";
                }
            }
            return type;
        }

        [HttpGet]
        public JsonResult LikePost(int threadId, string username)
        {
            var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadCommitted
            });
            try
            {
                using (scope)
                {
                    var threadModel = _unitOfWork.ThreadRepository.GetById(threadId);
                    threadModel.Likes = threadModel.Likes + 1;
                    _unitOfWork.ThreadRepository.Update(threadModel);
                    _unitOfWork.Save();

                    var likeModel = new LikeModel
                    {
                        ThreadId = threadId,
                        LikedBy = username,
                        LikedOn = DateTime.Now,
                        IpAddress = GetIp(),
                        Browser = Request.Browser.Browser
                    };
                    _unitOfWork.LikeRepository.Insert(likeModel);
                    _unitOfWork.Save();
                }
                return Json(new { Result = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                scope.Dispose();
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Result = "Failed" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult SendClassifiedMail(string emailTo, string subject,string message)
        {
          
            message = message + "<br /><br /><br />"+Constants.Domain + "<br />" + Constants.Slogan;

            return Json(_unitOfWork.SendEmail(emailTo, subject, message) ?
                new {Result = "success", Message = "Email has been sent successfully."} : 
                new { Result = "failed", Message = "OOps! Something wrong. Please try again later." });
        }

        [HttpGet]
        public ActionResult GetLikedByThreadId(int threadId)
        {
            var listLikedBy = _unitOfWork.LikeRepository.Get(x => x.ThreadId == threadId)
                                 .OrderByDescending(x => x.LikedOn);
            return Json(new { Result = listLikedBy }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCommentedByThreadId(int threadId)
        {
            var listcommentedBy = _unitOfWork.CommentRepository.Get(x => x.Thread_ThreadId == threadId).OrderByDescending(x => x.CommentedOn);
            return Json(new { Result = listcommentedBy }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetViewedByThreadId(int threadId)
        {
            var listviewedBy = _unitOfWork.ViewsRepository.Get(x => x.ThreadId == threadId).OrderByDescending(x => x.ViewedOn);
            return Json(new { Result = listviewedBy }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetWatchedByThreadId(int threadId)
        {
            var listwatchedBy = _unitOfWork.WatchRepository.Get(x => x.ThreadId == threadId).OrderByDescending(x => x.WatchedOn);
            return Json(new { Result = listwatchedBy }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        private static string GetIp()
        {
            var context = System.Web.HttpContext.Current;
            if (context == null)
                return string.Empty;

            return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                   ?? context.Request.UserHostAddress;
        }


    }
}
