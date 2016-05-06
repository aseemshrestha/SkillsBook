using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SkillsBook.Models.DAL;
using SkillsBook.Models.Models;

namespace SkillsBook.com.Controllers
{
    public class SearchController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public PartialViewResult SearchClassifieds(string category,string title="",string location="",string option="ALL",int page=1)
        {
            List<ClassifiedModel> searchResults = null;
            var searchQuery =
                _unitOfWork.ClassifiedRepository.Get(x => x.ClassifiedCategory.Equals(category) && x.CurrentStatus == Constants.ClassifiedItemAvailable);
            
            if (title != "" && location == "")
                searchQuery = searchQuery.Where(x => x.ClassifiedTitle.ToLower().Contains(title.ToLower()));
            if (title == "" && location != "")
                searchQuery = searchQuery.Where(x => x.City.ToLower().Contains(location.ToLower()) 
                    || x.State.ToLower().Contains(location.ToLower()) || x.Country.ToLower().Contains(location.ToLower()));
            if (title !="" && location != "")
                searchQuery = searchQuery.Where(x =>x.ClassifiedTitle.ToLower().Contains(title.ToLower())
                    && ( x.City.ToLower().Contains(location.ToLower()) 
                    || x.State.ToLower().Contains(location.ToLower()) 
                    || x.Country.ToLower().Contains(location.ToLower())));
          
            var classifiedModels = searchQuery as ClassifiedModel[] ?? searchQuery.ToArray();

            if (option == "ALL" || option == "All")
            {
                searchResults = classifiedModels.ToList();
            }
            else
            {
                searchResults = classifiedModels.Where(x=>x.ClassifiedRadio.Equals(option)).ToList();
            }
            
            int totalCount = searchResults.Count();

            ViewBag.Title = title;
            ViewBag.Location = location;
            ViewBag.Option =  option;
            return PartialView("Partial.CategorySearchResults",searchResults);
        }

        [HttpGet]
        public async Task<ActionResult> Search(string term, int page =1)
        {
            IEnumerable<ThreadModel> tagsList = null;
            int totalCount;
            if (!CacheManager.Exists("Search_"+term))
            {
                totalCount =
                    _unitOfWork.ThreadRepository.Get(x => x.Username.Contains(term) || x.Title.Contains(term) || x.Announcement.Contains(term) ).Count();
                CacheManager.AddToCache<string>("Search", totalCount, null, 5);
            }
            else
            {
                totalCount = (int) CacheManager.Get("Search_"+term);
            }

            int offset = (page - 1) * Constants.BlocksizeInitialLoad;
             try
                {
                    tagsList = _unitOfWork.ThreadRepository
                       .Get(x => x.Username.Contains(term) || x.Announcement.Contains(term) || x.Title.Contains(term))
                       .Skip((int)offset)
                       .Take(
                           (int)
                               (offset > Constants.BlocksizeInitialLoad
                                   ? offset - (offset - Constants.BlocksizeInitialLoad)
                                   : offset ==0 ? Constants.BlocksizeInitialLoad: offset));
                }
                catch (Exception ex)
                {
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                }
         
                var mostLiked = CacheImplementation.GetMostLikedThreads;
                var mostViewed = CacheImplementation.GetMostViewedThreads;
                var mostResponded = CacheImplementation.GetMostCommentedThreads;
                var trendingGeneral = CacheImplementation.GetTrendingItems(Constants.TagsGeneral);
                var trendingAnnouncement = CacheImplementation.GetTrendingItems(Constants.TagsAnnouncement);
                await Task.WhenAll(mostLiked, mostViewed, mostResponded, trendingGeneral, trendingAnnouncement);
                ViewBag.MostLiked = mostLiked.Result;
                ViewBag.MostViewed = mostViewed.Result;
                ViewBag.MostResponded = mostResponded.Result;
                ViewBag.TrendingTagsGeneral = trendingGeneral.Result.Take(Constants.BlocksizeTrendingTags);
                ViewBag.TrendingTagsAnnouncement = trendingAnnouncement.Result.Take(Constants.BlocksizeTrendingTags);
                
            ViewData["SearchTerm"] = term;
            ViewData["CurrentPage"] = page;
            ViewData["TotalCount"] = totalCount;
            ViewData["TotalPages"] =  ((totalCount - 1) / Constants.BlocksizeInitialLoad) + 1;
            return !tagsList.Any() ? null : PartialView("Partial.Search",tagsList);
        }
    }
}
