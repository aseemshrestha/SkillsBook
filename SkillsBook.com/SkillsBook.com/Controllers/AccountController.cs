using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using SkillsBook.com.Filters;
using SkillsBook.Models.DAL;
using SkillsBook.Models.Models;
using System.Text.RegularExpressions;

namespace SkillsBook.com.Controllers
{
    public class AccountController : AsyncController
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Unlike(int threadId, int currentLike)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            string username = String.Empty;
            if (httpCookie != null)
            {
                username = httpCookie.Values["username"];
            }
            _unitOfWork.Unlike(threadId, username, currentLike);
            CacheImplementation.UpdateCacheItem(CacheImplementation.RecentThreads, threadId, "Likes", "SUBTRACT");
            return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SwitchClassifiedStatus(int cid,int status)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            string username = String.Empty;
            if (httpCookie != null)
            {
                username = httpCookie.Values["username"];
            }
            var row = _unitOfWork.ClassifiedRepository.Get(x => x.ClassifiedId == cid && x.Username.Equals(username)).Single();
            if (status == 0)
            {
                status = 1;
            }
            else if (status == 1)
            {
                status = 0;
            }
            row.CurrentStatus = status;
            row.LastUpdated = DateTime.Now;
            _unitOfWork.ClassifiedRepository.Update(row);
            _unitOfWork.Save();
            CacheImplementation.ClearSpecificCacheObject(CacheImplementation.Classifieds);
            return Json(new { Result = "success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpPost([Bind(Include = "Username,Email,Password,ConfirmPassword")]UserModel signUpModel)
        {
            if (!ModelState.IsValid) return View("SignUp");
            try
            {
                if (_unitOfWork.DoesUserNameOrEmailExist(signUpModel.Username) == 1)
                {
                    ViewBag.Message = "OOps! Username already exists!";
                    return View("SignUp");
                }
                if (_unitOfWork.DoesUserNameOrEmailExist(signUpModel.Email) == 2)
                {
                    ViewBag.Message = "OOps! Email address already exists!";
                    return View("SignUp");
                }
                signUpModel.Password = Encryption.Encrypt(signUpModel.Password);
                signUpModel.ConfirmPassword = Encryption.Encrypt(signUpModel.ConfirmPassword);

                signUpModel.IpAddress = GetIp();
                signUpModel.Browser = HttpContext.Request.Browser.Browser;
                signUpModel.CreatedOn = DateTime.Now;
                signUpModel.LastSuccessfulLogin = DateTime.Now;
                _unitOfWork.UserRepository.Insert(signUpModel);
                _unitOfWork.Save();
                ViewBag.Message = "Thank you for registering. You may now login.";
            }
            catch (Exception ex)
            {
                // log the error
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return View("SignUp");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOn(string emailOrUsername, string password)
        {
            //check if email is in cookie..if is skip authentication
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var user = _unitOfWork.IsValidUser(emailOrUsername, password);
                if (user != null)
                {
                    user.LastSuccessfulLogin = DateTime.Now;
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();
                    var cookieLogOn = new HttpCookie("LogOnCookie");
                    cookieLogOn.Values["username"] = user.Username;
                    cookieLogOn.Values["email"] = user.Email;
                    cookieLogOn.Values["lastlogin"] = ToEstDateTime(user.LastSuccessfulLogin);
                    cookieLogOn.Expires = DateTime.Now.AddDays(2);
                    Response.Cookies.Add(cookieLogOn);
                    //  viewed by - just keep last 7 days of record
                  //  var query =
                    //    "DELETE FROM "+Constants.SchemaName+" [Views] WHERE ViewedBy = "+ user.Username+ " and ViewedOn < DATEADD(dd,-7,GETDATE())";
                  //  _unitOfWork.ViewsRepository.GetWithRawSql(query);
                    //_unitOfWork.Save();
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.MessageLogOn = "Authentication Failed. Please try again.";
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return View("SignUp");
        }

        public ActionResult LogOut()
        {
            //delete cookie..force user to logon
            var httpCookie = Response.Cookies["LogOnCookie"];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index", "Home");
        }

       
        [HttpPost]
        public async Task<ActionResult> ForgotPass(string email)
        {
       
            if (!Regex.IsMatch(email,
                @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                return Json(new { Result = "f", Message = "Invalid Email Address." },
                   JsonRequestBehavior.AllowGet);
            }
            var emailDb = await _unitOfWork.DoesEmailExistStringAsync(email);
            if (emailDb == null)
            {
                 return Json(new {Result = "f", Message = "Email do not exist."},
                    JsonRequestBehavior.AllowGet);
            }
            var temppassword = System.Web.Security.Membership.GeneratePassword(10, 2);
            var temppasswordEncrypted = Encryption.Encrypt(temppassword);
            emailDb.Password = temppasswordEncrypted;
            emailDb.ConfirmPassword = temppasswordEncrypted; 
            _unitOfWork.Save();

             _unitOfWork.SendEmail(email, Constants.Subject, Constants.Body.Replace("_username", emailDb.Username).Replace( "_temppassword",temppassword).Replace(",",""));

            return Json(new { Result = "s", Message = "Email has been sent. Please check your inbox. Thank you." },
                   JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("Check")]
        public JsonResult CheckUserNameOrEmailExists(string usernameOrEmail)
        {

            switch (_unitOfWork.DoesUserNameOrEmailExist(usernameOrEmail))
            {
                case 1:
                    {
                        var result = new
                        {
                            UsernameExists = "true",
                            Message = "Username Already Exists."
                        };
                        return Json(result);
                    }
                case 2:
                    {
                        var result = new
                        {
                            EmailExists = "true",
                            Message = "Email Already Exists."
                        };
                        return Json(result);
                    }
                default:
                    {
                        return null;
                    }
            }
        }
        [HttpPost]
        public JsonResult UpdatePassword(string old, string newpass, string confirmnew)
        {
            if (!newpass.Equals(confirmnew))
            {
                return Json(new { Result = "failed", Msg = "Password and Confirm Password do not match." });

            }
            var httpCookie = Request.Cookies["LogOnCookie"];
            var uname = httpCookie.Values["username"];
            return Json(_unitOfWork.UpdatePassword(uname, old, newpass) ? new { Result = "success", Msg = "Password Updated Successfully" }
                 : new { Result = "failed", Msg = "Your existing password didn't match" });
        }

        [HttpGet]
        public ActionResult GetWatchedPage(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            var likedDetails = _unitOfWork.GetWatchThreadDetails(uname).Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            int totalPosts = _unitOfWork.GetTotalWatchByUser(uname);
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;

            return PartialView("Partial.Watch", likedDetails);

        }

        [HttpGet]
        public ActionResult GetViewedPage(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            var likedDetails = _unitOfWork.GetViewedThreadDetails(uname).Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            int totalPosts = _unitOfWork.GetTotalViewsByUser(uname);
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;

            return PartialView("Partial.Viewed", likedDetails);

        }
        [HttpGet]
        public ActionResult GetCommentedPage(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            var commentedDetails = _unitOfWork.GetCommentedThreadDetails(uname).Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            int totalPosts = _unitOfWork.GetTotalCommentsByUser(uname);
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;

            return PartialView("Partial.Commented", commentedDetails);

        }
        [HttpGet]
        public ActionResult GetLikedPage(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            var likedDetails = _unitOfWork.GetLikedThreadDetails(uname).Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            int totalPosts = _unitOfWork.GetTotalLikesByUser(uname);
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;

            return PartialView("Partial.Liked", likedDetails);

        }
        [HttpGet]
        public ActionResult MyClassifieds(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            //get all at once 
            var query = _unitOfWork.ClassifiedRepository.Get(x => x.Username.Equals(uname)).OrderByDescending(x=>x.LastUpdated).ToList();
            int totalPosts = query.Count();
            var classifiedDetails = query.Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;
            return PartialView("Partial.MyClassifieds", classifiedDetails);
        }

        [HttpGet]
        public ActionResult MyQuestions(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            var offset = (page - 1) * Constants.BlocksizeMax;
            //get all at once 
            var query = _unitOfWork.QuestionRepository.Get(x => x.Username.Equals(uname)).OrderByDescending(x => x.LastUpdated).ToList();
            int totalPosts = query.Count();
            var questionDetails = query.Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset));
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;
            return PartialView("Partial.MyQuestions", questionDetails);
        }

        [HttpGet]
        public ActionResult MyAccount(int page = 1)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var uname = httpCookie.Values["username"];
            ViewBag.Username = uname;
            ViewBag.Email = httpCookie.Values["email"];
            var totalPosts = _unitOfWork.GetTotalPostByUser(uname);
            var offset = (page - 1) * Constants.BlocksizeMax;
            var accountDetails = _unitOfWork.GetAccountDetails(uname).Skip(offset)
                                   .Take((int)(offset > Constants.BlocksizeMax
                                   ? offset - (offset - Constants.BlocksizeMax)
                                   : offset == 0 ? Constants.BlocksizeMax : offset));


            foreach (var item in accountDetails)
            {
                ViewBag.AccountCreatedOn = item.CreatedOn;
                ViewBag.LastSuccessfulLogin = item.LastSuccessfulLogin;
                break;
            }
            ViewBag.TotalPosts = totalPosts;
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalPosts;
            ViewData["BlockSize"] = Constants.BlocksizeMax;
            ViewData["TotalPages"] = ((totalPosts - 1) / Constants.BlocksizeMax) + 1;
            return View(accountDetails);
        }

        [HttpGet]
        public ActionResult Feedback()
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null)
            {

                ViewData["USERNAME"] = httpCookie.Values["username"];
                ViewData["EMAIL"] = httpCookie.Values["email"];
            }

            return View("Feedback");
        }

        [HttpPost]
        public ActionResult FeedbackPost(string uname,string email,string subject,string message)
        {
            if (String.IsNullOrEmpty(uname) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(subject) ||
                String.IsNullOrEmpty(message))
            {
                return Json(new {Result = "vf", Message = "All Fields are required."});
            }
            var fb = new FeedbackModel
            {
                Username = uname,
                Email = email,
                Subject = subject,
                Message = message,
                SubmittedOn = DateTime.Now,
                IpAddress = GetIp()
                //Browser = Request.Browser.Browser
            };
            try
            {
                _unitOfWork.FeedBackRepository.Insert(fb);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { Result = "failed", Message = "OOPS! Something went wrong. Please try again." });

            }
            return Json(new {Result ="success", Message="Thank you for your Feedback."});
        }

        private static string GetIp()
        {
            var context = System.Web.HttpContext.Current;
            if (context == null)
                return string.Empty;

            return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]
                   ?? context.Request.UserHostAddress;
        }

        private String ToEstDateTime(DateTime timeInput)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTime(timeInput, easternZone);
            return easternTime.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

        public ActionResult E(string input)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null && httpCookie.Values["username"] == "bamboo")
            {
                return Content(Encryption.Encrypt(input));
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult D(string input)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null && httpCookie.Values["username"] == "bamboo")
            {
                return Content(Encryption.Decrypt(input));
            }
            return RedirectToAction("Index", "Home");
        }

       

        public ActionResult Delete(int threadId)
        {
            var httpCookie = Request.Cookies["LogOnCookie"];
            if (httpCookie != null && httpCookie.Values["username"] == "bamboo")
            {

                var row = _unitOfWork.ThreadRepository.Get(x => x.ThreadId == threadId).SingleOrDefault();
                if (row!=null && row.Likes > 0)
                {
                    var rowLikes= _unitOfWork.LikeRepository.Get(x => x.ThreadId == threadId).ToList();
                    foreach (var item in rowLikes)
                    {
                        _unitOfWork.LikeRepository.Delete(item);
                    }
                }
                if (row != null && row.Responses> 0)
                {
                    var rowR = _unitOfWork.CommentRepository.Get(x => x.Thread_ThreadId == threadId).ToList();
                    foreach (var comment in rowR)
                    {
                        _unitOfWork.CommentRepository.Delete(comment);
                    }
                }
                if (row != null && row.Watch > 0)
                {
                    var rowW = _unitOfWork.WatchRepository.Get(x => x.ThreadId == threadId).ToList();
                    foreach (var watch in rowW)
                    {
                        _unitOfWork.CommentRepository.Delete(watch);
                    }
                }
                var rowViews = _unitOfWork.ViewsRepository.Get(x => x.ThreadId == threadId).ToList();
                foreach (var item in rowViews)
                {
                    _unitOfWork.ViewsRepository.Delete(item);
                }
                _unitOfWork.ThreadRepository.Delete(threadId);
                _unitOfWork.Save();
                CacheImplementation.ClearSpecificCacheObject(CacheImplementation.RecentThreads);
                return Json(new {Result = "Y", Message = "Deleted Successfully"}, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
