using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillsBook.Models.Models;

namespace SkillsBook.Models.DAL
{
    public class CacheImplementation : CacheManager
    {
        public const string MostViewedThreads = "GetMostViewedThreads";
        public const string RecentThreads = "GetRecentThreads";
        public const string MostCommentedThreads = "GetMostCommentedThreads";
        public const string MostLikedThreads = "GetMostLikedThreads";
        public const string TrendingTagsGeneral = "GetTrendingTagsGeneral";
        public const string TrendingTagsAnnouncement = "GetTrendingTagsAnnouncement";
        public const string Classifieds = "GetRecentClassifieds";
        public const string RecentQuestions = "GetRecentQuestions";

        protected static readonly UnitOfWork Uow = new UnitOfWork();
       
        private static void UpdateViewsInCache(string cacheKey, int threadId)
        {
            if (!Exists(cacheKey)) return;
            
            var result = Cache.Get(cacheKey) as Task<List<ThreadModel>>;
            if (result == null) return;

            foreach (var item in result.Result.Where(x => x.ThreadId == threadId))
            {
                item.Views = item.Views + 1;
            }
        }

        private static void UpdateLikesInCache(string cacheKey, int threadId, string action)
        {
            if (!Exists(cacheKey)) return;
            var result = Cache.Get(cacheKey) as Task<List<ThreadModel>>;
            if (result == null) return;
                foreach (var item in result.Result.Where(x => x.ThreadId == threadId))
                {
                    if(action.Equals("SUBTRACT"))
                       item.Likes = item.Likes - 1;
                    else if(action.Equals("ADD"))
                       item.Likes = item.Likes + 1;
                }
            }
        
        private static void UpdateCommentsInCache(string cacheKey, int threadId)
        {
            if (!Exists(cacheKey)) return;
            var result = Cache.Get(cacheKey) as Task<List<ThreadModel>>;
            if (result == null) return;
            foreach (var item in result.Result.Where(x => x.ThreadId == threadId))
            {
                item.Responses = item.Responses + 1;
            }
        }
        private static void UpdateWatchInCache(string cacheKey, int threadId)
        {
            if (!Exists(cacheKey)) return;
            var result = Cache.Get(cacheKey) as Task<List<ThreadModel>>;
            if (result == null) return;
            foreach (var item in result.Result.Where(x => x.ThreadId == threadId))
            {
                item.Watch = item.Watch + 1;
            }
        }
     
        public static Task<List<string>> GetTrendingItems(string tagType)
        {
            switch (tagType)
            {
                case "TagsGeneral":
                    if (!Cache.Contains(TrendingTagsGeneral))
                        Refresh(TrendingTagsGeneral, null, Uow.GetTrendingTagsGeneralAsync());
                    return Cache.Get(TrendingTagsGeneral) as Task<List<string>>;

                case "TagsAnnouncement":
                    if (!Cache.Contains(TrendingTagsAnnouncement))
                        Refresh(TrendingTagsAnnouncement, null, Uow.GetTrendingTagsAnnouncementAsync());
                    return Cache.Get(TrendingTagsAnnouncement) as Task<List<string>>;

            }
            return null;
        }

        public static Task<List<ThreadModel>> GetRecentThreads
        {
            get
            {
                if (!Cache.Contains(RecentThreads))
                {
                   // var model = Uow.GetRecentRecordsAsync(Constants.BlocksizeInitialLoad);
                    Refresh(RecentThreads, null, Uow.GetRecentRecordsAsync(Constants.BlocksizeInitialLoad));
                }
                return (Task<List<ThreadModel>>)Cache.Get(RecentThreads);
            }
        }


        public static Task<List<ThreadModel>>  GetMostViewedThreads
        {
            get
            {
                if (!Cache.Contains(MostViewedThreads))
                    Refresh(MostViewedThreads, null, Uow.GetMostViewedThreads(Constants.BlocksizeMostViewed));
                return (Task<List<ThreadModel>>) Cache.Get(MostViewedThreads);
            }
        }

        public static Task<List<ClassifiedModel>> GetRecentClassifieds
        {
            get
            {
                if (!Cache.Contains(Classifieds))
                    Refresh(Classifieds, null, Uow.GetRecentClassifieds(Constants.BlocksizeClassifieds));
                return (Task<List<ClassifiedModel>>)Cache.Get(Classifieds);
            }
        }
        //not used
        public static List<QuestionModel> GetRecentQuestions
        {
            get
            {
                if (!Cache.Contains(RecentQuestions))
                    RefreshSync(Classifieds, null, Uow.GetRecentQuestions(Constants.BlocksizeClassifieds));
                return (List<QuestionModel>)Cache.Get(RecentQuestions);
            }
        }

        public static Task<List<ThreadModel>> GetMostLikedThreads
        {
            get
            {
                if (!Cache.Contains(MostLikedThreads))
                    Refresh(MostLikedThreads, null, Uow.GetMostLikedThreads(Constants.BlocksizeMostLiked));
                return (Task<List<ThreadModel>>)Cache.Get(MostLikedThreads);
            }
        }
        public static Task<List<ThreadModel>> GetMostCommentedThreads
        {
            get
            {
                if (!Cache.Contains(MostCommentedThreads))
                    Refresh(MostCommentedThreads, null, Uow.GetMostCommentedThreads(Constants.BlocksizeMostCommented));
                return (Task<List<ThreadModel>>)Cache.Get(MostCommentedThreads);
            }
        }


        public static void UpdateCacheItem(string cacheKey, int id, string updateItem,string action)
        {
            Action<string, int> updateCache = null;
            switch (updateItem)
            {
                case "views":
                case "Views":
                    updateCache = UpdateViewsInCache;
                    updateCache(cacheKey, id);
                    break;
                case "likes":
                case "Likes":
                    Action<string, int, string> updateLikeCache = UpdateLikesInCache;
                    updateLikeCache(cacheKey, id,action);
                    break;
                case "comments":
                case "Comments":
                    updateCache = UpdateCommentsInCache;
                    updateCache(cacheKey, id);
                    break;
                case "watch":
                case "Watch":
                    updateCache = UpdateWatchInCache;
                    updateCache(cacheKey, id);
                    break;
            }
        }

        public static ThreadModel GetSingleThreadByUser(int id)
        {
            if (!Cache.Contains("ThreadById"))
                Refresh<object>("ThreadById", Uow.ThreadRepository.GetById(id), null);
            return Cache.Get("ThreadById") as ThreadModel;
        }
    }
}
