using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Ajax.Utilities;
using SkillsBook.com.LolNQuote;
using SkillsBook.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SkillsBook.Models.Models;

namespace SkillsBook.com.Controllers
{
    public class HomeController : Controller
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public async Task<ActionResult> Index()
        {
            //www.asp.net/mvc/overview/performance/using-asynchronous-methods-in-aspnet-mvc-4
          
            var allThreads = CacheImplementation.GetRecentThreads;
            var mostLiked = CacheImplementation.GetMostLikedThreads;
            var mostViewed = CacheImplementation.GetMostViewedThreads;
            var mostResponded= CacheImplementation.GetMostCommentedThreads;
            var trendingGeneral = CacheImplementation.GetTrendingItems(Constants.TagsGeneral);
            var trendingAnnouncement = CacheImplementation.GetTrendingItems(Constants.TagsAnnouncement);
            var classifiedsRecent = CacheImplementation.GetRecentClassifieds;
           // var recentQuestion = CacheImplementation.GetRecentQuestions;

            await Task.WhenAll(allThreads,mostLiked,mostViewed,mostResponded,trendingGeneral,trendingAnnouncement,classifiedsRecent);
            ViewBag.MostLiked = mostLiked.Result;
            ViewBag.MostViewed = mostViewed.Result;
            ViewBag.MostResponded = mostResponded.Result;
            ViewBag.TrendingTagsGeneral = trendingGeneral.Result.Take(Constants.BlocksizeTrendingTags);
            ViewBag.TrendingTagsAnnouncement = trendingAnnouncement.Result.Take(Constants.BlocksizeTrendingTags);
            ViewBag.Classifieds = classifiedsRecent.Result;

            //store in session
            if (Session["RecentQuestions"] == null)
            {
                var recentQuestions =
                    _unitOfWork.QuestionRepository.GetWithRawSql("Select Top 25 * from " + Constants.SchemaName +
                                                                 "Questions order by LastUpdated desc");

                Session["RecentQuestions"] = recentQuestions;
            }
          
            //ViewBag.RecentQuestio=Session["RecentQuestions"];
           
                   
            if (Session["QuoteTop"] == null || Session["Quote"] == null || Session["AuthorTop"] == null ||
                Session["Author"] == null)
            {
                var doc = new XmlDocument();
                doc.Load(Server.MapPath("~/LolNQuote/quotes.xml"));
                XmlElement xelRoot = doc.DocumentElement;
                if (xelRoot != null)
                {
                    XmlNodeList xnlNodes = xelRoot.SelectNodes("/QuoteDetails/QuoteMeta");
                    int count = 0;
                    if (xnlNodes != null)

                        foreach (XmlNode xndNode in xnlNodes)
                        {
                            var xmlElement = xndNode["Quote"];
                            if (xmlElement != null)
                            {
                                if (count == 0)
                                {
                                   // ViewBag.QuoteTop = xmlElement.InnerText;
                                    Session["QuoteTop"] = xmlElement.InnerText;
                                }
                                if (count == 1)
                                {
                                   // ViewBag.Quote = xmlElement.InnerText;
                                    Session["Quote"] = xmlElement.InnerText;
                                }
                            }
                            var element = xndNode["Author"];
                            if (element != null)
                            {
                                if (count == 0)
                                {
                                   // ViewBag.AuthorTop = element.InnerText;
                                    Session["AuthorTop"] = element.InnerText;
                                }
                                if (count == 1)
                                {
                                   // ViewBag.Author = element.InnerText;
                                    Session["Author"] = element.InnerText;
                                }
                            }
                            count++;
                            if (count > 1)
                                break;
                        }
                }
            }
            return View(allThreads.Result);
        }
        private String ToEstDateTime(DateTime timeInput)
        {
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTime(timeInput, easternZone);
            return easternTime.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
        }

    }
}
