using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using SkillsBook.Models.DAL;
using SkillsBook.Models.Models;
using System.Text.RegularExpressions;
using System.Text;
using System.Globalization;


namespace SkillsBook.com.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult ScanTickers()
        {
            Tickers _tickers = new Tickers();
            ViewBag.Scan1 = _tickers.getBatch1A().Replace(",", "-").ToUpper();
            ViewBag.Scan2 = _tickers.getBatch2A().Replace(",", "-").ToUpper();

            ViewBag.Scan3 = _tickers.getBatch1B().Replace(",", "-").ToUpper();
            ViewBag.Scan4 = _tickers.getBatch2B().Replace(",", "-").ToUpper();

            ViewBag.Scan5 = _tickers.getBatch1C().Replace(",", "-").ToUpper();
            ViewBag.Scan6 = _tickers.getBatch2C().Replace(",", "-").ToUpper();
            ViewBag.Scan7 = _tickers.getBatch3C().Replace(",", "-").ToUpper();
            ViewBag.Scan8 = _tickers.getBatch4C().Replace(",", "-").ToUpper();
            ViewBag.Scan9 = _tickers.getBatch5C().Replace(",", "-").ToUpper();
            ViewBag.Scan10 = _tickers.getBatch6C().Replace(",", "-").ToUpper();

            ViewBag.Scan11 = _tickers.getBatch1D().Replace(",", "-").ToUpper();
            ViewBag.Scan12 = _tickers.getBatch2D().Replace(",", "-").ToUpper();

            ViewBag.Scan13 = _tickers.getBatch1E().Replace(",", "-").ToUpper();
            ViewBag.Scan14 = _tickers.getBatch2E().Replace(",", "-").ToUpper();
            ViewBag.Scan15 = _tickers.getBatch3E().Replace(",", "-").ToUpper();

            ViewBag.Scan16 = _tickers.getBatch1F().Replace(",", "-").ToUpper();
            ViewBag.Scan17 = _tickers.getBatch2F().Replace(",", "-").ToUpper();

            ViewBag.Scan18 = _tickers.getBatch1G().Replace(",", "-").ToUpper();
            ViewBag.Scan19 = _tickers.getBatch2G().Replace(",", "-").ToUpper();
            ViewBag.Scan20 = _tickers.getBatch3G().Replace(",", "-").ToUpper();

            ViewBag.Scan21 = _tickers.getBatch1H().Replace(",", "-").ToUpper();
            ViewBag.Scan22 = _tickers.getBatch2H().Replace(",", "-").ToUpper();

            ViewBag.Scan23 = _tickers.getBatch1I().Replace(",", "-").ToUpper();
            ViewBag.Scan24 = _tickers.getBatch2I().Replace(",", "-").ToUpper();
            ViewBag.Scan25 = _tickers.getBatch3I().Replace(",", "-").ToUpper();

            ViewBag.Scan26 = _tickers.getBatch1J().Replace(",", "-").ToUpper();

            ViewBag.Scan27 = _tickers.getBatch1K().Replace(",", "-").ToUpper();

            ViewBag.Scan28 = _tickers.getBatch1L().Replace(",", "-").ToUpper();
            ViewBag.Scan29 = _tickers.getBatch2L().Replace(",", "-").ToUpper();

            ViewBag.Scan30 = _tickers.getBatch1M().Replace(",", "-").ToUpper();
            ViewBag.Scan31 = _tickers.getBatch2M().Replace(",", "-").ToUpper();
            ViewBag.Scan32 = _tickers.getBatch3M().Replace(",", "-").ToUpper();

            ViewBag.Scan33 = _tickers.getBatch1N().Replace(",", "-").ToUpper();
            ViewBag.Scan34 = _tickers.getBatch2N().Replace(",", "-").ToUpper();
            ViewBag.Scan35 = _tickers.getBatch3N().Replace(",", "-").ToUpper();

            ViewBag.Scan36 = _tickers.getBatch1O().Replace(",", "-").ToUpper();
            ViewBag.Scan37 = _tickers.getBatch2O().Replace(",", "-").ToUpper();

            ViewBag.Scan38 = _tickers.getBatch1P().Replace(",", "-").ToUpper();
            ViewBag.Scan39 = _tickers.getBatch2P().Replace(",", "-").ToUpper();
            ViewBag.Scan40 = _tickers.getBatch3P().Replace(",", "-").ToUpper();

            ViewBag.Scan41 = _tickers.getBatch1Q().Replace(",", "-").ToUpper();

            ViewBag.Scan42 = _tickers.getBatch1R().Replace(",", "-").ToUpper();
            ViewBag.Scan43 = _tickers.getBatch2R().Replace(",", "-").ToUpper();
            ViewBag.Scan44 = _tickers.getBatch3R().Replace(",", "-").ToUpper();

            ViewBag.Scan45 = _tickers.getBatch1S().Replace(",", "-").ToUpper();
            ViewBag.Scan46 = _tickers.getBatch2S().Replace(",", "-").ToUpper();
            ViewBag.Scan47 = _tickers.getBatch3S().Replace(",", "-").ToUpper();
            ViewBag.Scan48 = _tickers.getBatch4S().Replace(",", "-").ToUpper();

            ViewBag.Scan49 = _tickers.getBatch1T().Replace(",", "-").ToUpper();
            ViewBag.Scan50 = _tickers.getBatch2T().Replace(",", "-").ToUpper();

            ViewBag.Scan51 = _tickers.getBatch1U().Replace(",", "-").ToUpper();

            ViewBag.Scan52 = _tickers.getBatch1V().Replace(",", "-").ToUpper();
            ViewBag.Scan53 = _tickers.getBatch2V().Replace(",", "-").ToUpper();

            ViewBag.Scan54 = _tickers.getBatch1W().Replace(",", "-").ToUpper();

            ViewBag.Scan55 = _tickers.getBatch1Y().Replace(",", "-").ToUpper();

            ViewBag.Scan56 = _tickers.getBatch1Z().Replace(",", "-").ToUpper();

            ViewBag.Scan57 = _tickers.getBatch1X().Replace(",", "-").ToUpper();

            return View();
        }
        public async Task<ActionResult> MyScan(String cache = "true")
        {
            if (cache == "true")
            {
                if (CacheImplementation.Exists("allTodayNews"))
                {
                    return View(CacheImplementation.Get("allTodayNews"));
                }
            }
            else
            {
                CacheImplementation.ClearSpecificCacheObject("allTodayNews");
            }
            var url = "https://feeds.finance.yahoo.com/rss/2.0/headline?";
            const string symbolappend = "s=";
            const string others = "&lang=en-US";
            var searchSym = "";
            Tickers _tickers = new Tickers();
            var req1 = _tickers.getBatch1A();
            var req2 = _tickers.getBatch2A();

            var req3 = _tickers.getBatch1B();
            var req4 = _tickers.getBatch2B();

            var req5 = _tickers.getBatch1C();
            var req6 = _tickers.getBatch2C();
            var req7 = _tickers.getBatch3C();
            var req8 = _tickers.getBatch4C();
            var req9 = _tickers.getBatch5C();
            var req10 = _tickers.getBatch6C();

            var req11 = _tickers.getBatch1D();
            var req12 = _tickers.getBatch2D();

            var req13 = _tickers.getBatch1E();
            var req14 = _tickers.getBatch2E();
            var req15 = _tickers.getBatch3E();

            var req16 = _tickers.getBatch1F();
            var req17 = _tickers.getBatch2F();

            var req18 = _tickers.getBatch1G();
            var req19 = _tickers.getBatch2G();
            var req20 = _tickers.getBatch3G();

            var req21 = _tickers.getBatch1H();
            var req22 = _tickers.getBatch2H();

            var req23 = _tickers.getBatch1I();
            var req24 = _tickers.getBatch2I();
            var req25 = _tickers.getBatch3I();

            var req26 = _tickers.getBatch1J();

            var req27 = _tickers.getBatch1K();

            var req28 = _tickers.getBatch1L();
            var req29 = _tickers.getBatch2L();

            var req30 = _tickers.getBatch1M();
            var req31 = _tickers.getBatch2M();
            var req32 = _tickers.getBatch3M();

            var req33 = _tickers.getBatch1N();
            var req34 = _tickers.getBatch2N();
            var req35 = _tickers.getBatch3N();

            var req36 = _tickers.getBatch1O();
            var req37 = _tickers.getBatch2O();

            var req38 = _tickers.getBatch1P();
            var req39 = _tickers.getBatch2P();
            var req40 = _tickers.getBatch3P();

            var req41 = _tickers.getBatch1Q();

            var req42 = _tickers.getBatch1R();
            var req43 = _tickers.getBatch2R();
            var req44 = _tickers.getBatch3R();

            var req45 = _tickers.getBatch1S();
            var req46 = _tickers.getBatch2S();
            var req47 = _tickers.getBatch3S();
            var req48 = _tickers.getBatch4S();

            var req49 = _tickers.getBatch1T();
            var req50 = _tickers.getBatch2T();

            var req51 = _tickers.getBatch1U();

            var req52 = _tickers.getBatch1V();
            var req53 = _tickers.getBatch2V();

            var req54 = _tickers.getBatch1W();

            var req55 = _tickers.getBatch1Y();

            var req56 = _tickers.getBatch1Z();

            var req57 = _tickers.getBatch1X();


            List<List<News>> allNews = new List<List<News>>();
            List<News> sortedList = new List<News>();
            var news1 = await GetNewsAsync(url + "s=" + req1 + "&lang=en-US");
            var news2 = await GetNewsAsync(url + "s=" + req2 + "&lang=en-US");
            var news3 = await GetNewsAsync(url + "s=" + req3 + "&lang=en-US");
            var news4 = await GetNewsAsync(url + "s=" + req4 + "&lang=en-US");
            var news5 = await GetNewsAsync(url + "s=" + req5 + "&lang=en-US");
            var news6 = await GetNewsAsync(url + "s=" + req6 + "&lang=en-US");
            var news7 = await GetNewsAsync(url + "s=" + req7 + "&lang=en-US");
            var news8 = await GetNewsAsync(url + "s=" + req8 + "&lang=en-US");
            var news9 = await GetNewsAsync(url + "s=" + req9 + "&lang=en-US");
            var news10 = await GetNewsAsync(url + "s=" + req10 + "&lang=en-US");
            var news11 = await GetNewsAsync(url + "s=" + req11 + "&lang=en-US");
            var news12 = await GetNewsAsync(url + "s=" + req12 + "&lang=en-US");
            var news13 = await GetNewsAsync(url + "s=" + req13 + "&lang=en-US");
            var news14 = await GetNewsAsync(url + "s=" + req14 + "&lang=en-US");
            var news15 = await GetNewsAsync(url + "s=" + req15 + "&lang=en-US");
            var news16 = await GetNewsAsync(url + "s=" + req16 + "&lang=en-US");
            var news17 = await GetNewsAsync(url + "s=" + req17 + "&lang=en-US");
            var news18 = await GetNewsAsync(url + "s=" + req18 + "&lang=en-US");
            var news19 = await GetNewsAsync(url + "s=" + req19 + "&lang=en-US");
            var news20 = await GetNewsAsync(url + "s=" + req20 + "&lang=en-US");
            var news21 = await GetNewsAsync(url + "s=" + req21 + "&lang=en-US");
            var news22 = await GetNewsAsync(url + "s=" + req22 + "&lang=en-US");
            var news23 = await GetNewsAsync(url + "s=" + req23 + "&lang=en-US");
            var news24 = await GetNewsAsync(url + "s=" + req24 + "&lang=en-US");
            var news25 = await GetNewsAsync(url + "s=" + req25 + "&lang=en-US");
            var news26 = await GetNewsAsync(url + "s=" + req26 + "&lang=en-US");
            var news27 = await GetNewsAsync(url + "s=" + req27 + "&lang=en-US");
            var news28 = await GetNewsAsync(url + "s=" + req28 + "&lang=en-US");
            var news29 = await GetNewsAsync(url + "s=" + req29 + "&lang=en-US");
            var news30 = await GetNewsAsync(url + "s=" + req30 + "&lang=en-US");
            var news31 = await GetNewsAsync(url + "s=" + req31 + "&lang=en-US");
            var news32 = await GetNewsAsync(url + "s=" + req32 + "&lang=en-US");
            var news33 = await GetNewsAsync(url + "s=" + req33 + "&lang=en-US");
            var news34 = await GetNewsAsync(url + "s=" + req34 + "&lang=en-US");
            var news35 = await GetNewsAsync(url + "s=" + req35 + "&lang=en-US");
            var news36 = await GetNewsAsync(url + "s=" + req36 + "&lang=en-US");
            var news37 = await GetNewsAsync(url + "s=" + req37 + "&lang=en-US");
            var news38 = await GetNewsAsync(url + "s=" + req38 + "&lang=en-US");
            var news39 = await GetNewsAsync(url + "s=" + req39 + "&lang=en-US");
            var news40 = await GetNewsAsync(url + "s=" + req40 + "&lang=en-US");
            var news41 = await GetNewsAsync(url + "s=" + req41 + "&lang=en-US");
            var news42 = await GetNewsAsync(url + "s=" + req42 + "&lang=en-US");
            var news43 = await GetNewsAsync(url + "s=" + req43 + "&lang=en-US");
            var news44 = await GetNewsAsync(url + "s=" + req44 + "&lang=en-US");
            var news45 = await GetNewsAsync(url + "s=" + req45 + "&lang=en-US");
            var news46 = await GetNewsAsync(url + "s=" + req46 + "&lang=en-US");
            var news47 = await GetNewsAsync(url + "s=" + req47 + "&lang=en-US");
            var news48 = await GetNewsAsync(url + "s=" + req48 + "&lang=en-US");
            var news49 = await GetNewsAsync(url + "s=" + req49 + "&lang=en-US");
            var news50 = await GetNewsAsync(url + "s=" + req50 + "&lang=en-US");
            var news51 = await GetNewsAsync(url + "s=" + req51 + "&lang=en-US");
            var news52 = await GetNewsAsync(url + "s=" + req52 + "&lang=en-US");
            var news53 = await GetNewsAsync(url + "s=" + req53 + "&lang=en-US");
            var news54 = await GetNewsAsync(url + "s=" + req54 + "&lang=en-US");
            var news55 = await GetNewsAsync(url + "s=" + req55 + "&lang=en-US");
            var news56 = await GetNewsAsync(url + "s=" + req56 + "&lang=en-US");
            var news57 = await GetNewsAsync(url + "s=" + req57 + "&lang=en-US");

            allNews.Add(news1);
            allNews.Add(news2);
            allNews.Add(news3);
            allNews.Add(news4);
            allNews.Add(news5);
            allNews.Add(news6);
            allNews.Add(news7);
            allNews.Add(news8);
            allNews.Add(news9);
            allNews.Add(news10);
            allNews.Add(news11);
            allNews.Add(news12);
            allNews.Add(news13);
            allNews.Add(news14);
            allNews.Add(news15);
            allNews.Add(news16);
            allNews.Add(news17);
            allNews.Add(news18);
            allNews.Add(news19);
            allNews.Add(news20);
            allNews.Add(news21);
            allNews.Add(news22);
            allNews.Add(news23);
            allNews.Add(news24);
            allNews.Add(news25);
            allNews.Add(news26);
            allNews.Add(news27);
            allNews.Add(news28);
            allNews.Add(news29);
            allNews.Add(news30);
            allNews.Add(news31);
            allNews.Add(news32);
            allNews.Add(news33);
            allNews.Add(news34);
            allNews.Add(news35);
            allNews.Add(news36);
            allNews.Add(news37);
            allNews.Add(news38);
            allNews.Add(news39);
            allNews.Add(news40);
            allNews.Add(news41);
            allNews.Add(news42);
            allNews.Add(news43);
            allNews.Add(news44);
            allNews.Add(news45);
            allNews.Add(news46);
            allNews.Add(news47);
            allNews.Add(news48);
            allNews.Add(news49);
            allNews.Add(news50);
            allNews.Add(news51);
            allNews.Add(news52);
            allNews.Add(news53);
            allNews.Add(news54);
            allNews.Add(news55);
            allNews.Add(news56);
            allNews.Add(news57);
            foreach (var item in allNews)
            {
                foreach (var i in item)
                {
                    sortedList.Add(i);
                }
            }

            sortedList.Sort((ps1, ps2) => DateTime.Compare(ps2.PublishedDateTime, ps1.PublishedDateTime));
            CacheImplementation.AddToCache("allTodayNews", null, sortedList, 1);

            return View(sortedList);

        }
        //display today's news only
        public Task<List<News>> GetNewsAsync(String url)
        {
            var listOfNews = new List<News>();

            try
            {
                
                XmlDocument rssXmlDoc = new XmlDocument();
                rssXmlDoc.Load(url);
                XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

                foreach (XmlNode rssNode in rssNodes)
                {
                    var news = new News();

                    XmlNode rssSubNode = rssNode.SelectSingleNode("title");

                    news.Title = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("link");
                    news.Link = rssSubNode != null ? rssSubNode.InnerText : "";

                    rssSubNode = rssNode.SelectSingleNode("pubDate");
                    DateTime newsToday = DateParse(rssSubNode.InnerText);

                    if (newsToday.Date.ToShortDateString() != DateTime.Today.ToShortDateString())
                    {
                        continue;
                    }
                    news.PublishedDateTime = newsToday;

                    listOfNews.Add(news);

                }
            }
            catch (Exception ex) { }
            return Task.Run(() => listOfNews);


        }

        public async Task<ActionResult> Index(String symbols = null)
        {
            //  DateTime d = DateParse();
            var url = "https://feeds.finance.yahoo.com/rss/2.0/headline?";
            const string symbolappend = "s=";
            const string others = "&lang=en-US";
            var searchSym = "";
            Tickers tickers = new Tickers();

            if (symbols != null && !Regex.IsMatch(symbols, @"^[a-zA-Z0-9-]+$"))
            {
                return HttpNotFound();
            }
            //    return View("index2",null);

            if (symbols != null && symbols.Contains("-"))
            {
                int count = symbols.Count(f => f == '-');
                if (count > 20)
                {
                    ViewBag.Error = "You can only request 20 symbols max";
                    return RedirectToAction("ScanTickers", "Home");
                }
                else
                {
                    searchSym = symbols;
                    symbols = symbols.Replace("-", ",");
                    url = url + "s=" + symbols + "&lang=en-US";
                }
            }

            else if (symbols != null && symbols.StartsWith("from"))
            {
                searchSym = symbols.Replace("from", "News starting from ticker - ");
            }
            else if (symbols != null && symbols.Equals("vol200-1"))
            {
                searchSym = "Above 50DSMA & Avg VOL > 200K- I";
            }
            else if (symbols != null && symbols.Equals("vol200-2"))
            {
                searchSym = "Above 50DSMA & Avg VOL > 200K- II";
            }
            else if (symbols != null && symbols.Equals("vol200-3"))
            {
                searchSym = "Above 50DSMA & Avg VOL > 200K- III";
            }
            else if (symbols != null && symbols.Equals("vol200-4"))
            {
                searchSym = "Above 50DSMA & Avg VOL > 200K- IV";
            }
            ViewBag.Search = "Results for : " + searchSym;
            if (symbols == null)
            {
                ViewBag.Search = "News starting from ticker - A - (default)";
                url = url + "s=" + tickers.getTickersFromA() + "&lang=en-US";
            }
            else if (symbols.Equals("vol200-1"))
            {
                //  //avg vol >200,000 and > 50D SMA
                url = url + "s=" + SMA50Vol200.TickersI + "&lang=en-US";
            }
            else if (symbols.Equals("vol200-2"))
            {
                //  //avg vol >200,000 and > 50D SMA
                url = url + "s=" + SMA50Vol200.TickersII + "&lang=en-US";
            }
            else if (symbols.Equals("vol200-3"))
            {
                //  //avg vol >200,000 and > 50D SMA
                url = url + "s=" + SMA50Vol200.TickersIII + "&lang=en-US";
            }
            else if (symbols.Equals("vol200-4"))
            {
                //  //avg vol >200,000 and > 50D SMA
                url = url + "s=" + SMA50Vol200.TickersIV + "&lang=en-US";
            }
            else if (symbols.Equals("fromA"))
            {

                url = url + "s=" + tickers.getTickersFromA() + "&lang=en-US";
            }
            else if (symbols.Equals("fromB"))
            {
                url = url + "s=" + tickers.getTickersFromB() + "&lang=en-US";
            }
            else if (symbols.Equals("fromC"))
            {
                url = url + "s=" + tickers.getTickersFromC() + "&lang=en-US";
            }
            else if (symbols.Equals("fromD"))
            {
                url = url + "s=" + tickers.getTickersFromD() + "&lang=en-US";
            }
            else if (symbols.Equals("fromE"))
            {
                url = url + "s=" + tickers.getTickersFromE() + "&lang=en-US";
            }
            else if (symbols.Equals("fromF"))
            {
                url = url + "s=" + tickers.getTickersFromF() + "&lang=en-US";
            }
            else if (symbols.Equals("fromG"))
            {
                url = url + "s=" + tickers.getTickersFromG() + "&lang=en-US";
            }
            else if (symbols.Equals("fromH"))
            {
                url = url + "s=" + tickers.getTickersFromH() + "&lang=en-US";
            }
            else if (symbols.Equals("fromI"))
            {
                url = url + "s=" + tickers.getTickersFromI() + "&lang=en-US";
            }
            else if (symbols.Equals("fromJ"))
            {
                url = url + "s=" + tickers.getTickersFromJ() + "&lang=en-US";
            }
            else if (symbols.Equals("fromK"))
            {
                url = url + "s=" + tickers.getTickersFromK() + "&lang=en-US";
            }
            else if (symbols.Equals("fromL"))
            {
                url = url + "s=" + tickers.getTickersFromL() + "&lang=en-US";
            }
            else if (symbols.Equals("fromM"))
            {
                url = url + "s=" + tickers.getTickersFromM() + "&lang=en-US";
            }
            else if (symbols.Equals("fromN"))
            {
                url = url + "s=" + tickers.getTickersFromN() + "&lang=en-US";
            }
            else if (symbols.Equals("fromO"))
            {
                url = url + "s=" + tickers.getTickersFromO() + "&lang=en-US";
            }
            else if (symbols.Equals("fromP"))
            {
                url = url + "s=" + tickers.getTickersFromP() + "&lang=en-US";
            }
            else if (symbols.Equals("fromQ"))
            {
                url = url + "s=" + tickers.getTickersFromQ() + "&lang=en-US";
            }
            else if (symbols.Equals("fromR"))
            {
                url = url + "s=" + tickers.getTickersFromR() + "&lang=en-US";
            }
            else if (symbols.Equals("fromS"))
            {
                url = url + "s=" + tickers.getTickersFromS() + "&lang=en-US";
            }
            else if (symbols.Equals("fromT"))
            {
                url = url + "s=" + tickers.getTickersFromT() + "&lang=en-US";
            }
            else if (symbols.Equals("fromU"))
            {
                url = url + "s=" + tickers.getTickersFromU() + "&lang=en-US";
            }
            else if (symbols.Equals("fromV"))
            {
                url = url + "s=" + tickers.getTickersFromV() + "&lang=en-US";
            }
            else if (symbols.Equals("fromW"))
            {
                url = url + "s=" + tickers.getTickersFromW() + "&lang=en-US";
            }
            else if (symbols.Equals("fromX"))
            {
                url = url + "s=" + tickers.getTickersFromX() + "&lang=en-US";
            }
            else if (symbols.Equals("fromY"))
            {
                url = url + "s=" + tickers.getTickersFromY() + "&lang=en-US";
            }
            else if (symbols.Equals("fromZ"))
            {
                url = url + "s=" + tickers.getTickersFromZ() + "&lang=en-US";
            }
            else
            {
                ViewBag.Search = "Results for : " + symbols;
                url = "https://feeds.finance.yahoo.com/rss/2.0/headline?" + symbolappend + symbols + others;
            }
            var listOfNews = new List<News>();
            XmlDocument rssXmlDoc = new XmlDocument();
            rssXmlDoc.Load(url);
            XmlNodeList rssNodes = rssXmlDoc.SelectNodes("rss/channel/item");

            foreach (XmlNode rssNode in rssNodes)
            {
                var news = new News();
                XmlNode rssSubNode = rssNode.SelectSingleNode("title");
                news.Title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                news.Link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("pubDate");
                news.PublishedDateTime = DateParse(rssSubNode.InnerText);

                listOfNews.Add(news);

            }
            return View("Index2", listOfNews);
        }

        private String ToEstDateTime(DateTime timeUtc)
        {
            //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            // DateTime easternTime = TimeZoneInfo.ConvertTime(timeInput, easternZone);
            // return easternTime.ToLongTimeString();


            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return estTime.ToShortDateString() + " " + estTime.ToLongTimeString();


        }
        private DateTime ToEstDateTimeDate(DateTime timeUtc)
        {
            //TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            // DateTime easternTime = TimeZoneInfo.ConvertTime(timeInput, easternZone);
            // return easternTime.ToLongTimeString();

            // DateTime timeUtc = Convert.ToDateTime(dt);  
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime estTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, cstZone);
            return estTime;


        }
        private DateTime DateParse(String dateString)
        {
            // string dateString = "Tue, 04 Apr 2017 22:55:10 GMT";
            //Thu, 01 May 2008 07:34:42 GMT

            DateTime convertedDate = DateTime.Parse(dateString.Replace("UTC", "GMT"));
            return convertedDate.AddHours(3);


        }
    }
}

