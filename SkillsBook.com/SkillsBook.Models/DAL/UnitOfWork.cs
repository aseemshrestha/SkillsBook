using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;
using SkillsBook.Models.Models;
using SkillsBook.Models.ViewModel;

namespace SkillsBook.Models.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly SiteContext _context = new SiteContext();
        private BaseRepository<UserModel> _userRepository;
        private BaseRepository<ThreadModel> _threadRepository;
        private BaseRepository<LikeModel> _likeRepository;
        private BaseRepository<ViewsModel> _viewsRepository;
        private BaseRepository<FollowModel> _followRepository;
        private BaseRepository<WatchModel> _watchRepository;
        private BaseRepository<CommentModel> _commentRepository;
        private BaseRepository<ClassifiedModel> _classifiedRepository;
        private BaseRepository<FeedbackModel> _feedbackRepository;
        private BaseRepository<QuestionModel> _questionRepository;
        private BaseRepository<AnswerModel> _answerRepository;
        private BaseRepository<AnswerResponseModel> _answerResponseRepository;
        private BaseRepository<QuizModel> _quizRepository;
        private BaseRepository<QuizAnswersModel> _quizAnswerRepository;

        public BaseRepository<QuizModel> QuizRepository
        {
            get
            {
                if (this._quizRepository== null)
                {
                    this._quizRepository = new BaseRepository<QuizModel>(_context);
                }
                return _quizRepository;
            }
        }
        public BaseRepository<QuizAnswersModel> QuizAnswerRepository
        {
            get
            {
                if (this._quizAnswerRepository == null)
                {
                    this._quizAnswerRepository = new BaseRepository<QuizAnswersModel>(_context);
                }
                return _quizAnswerRepository;
            }
        }
        public BaseRepository<QuestionModel> QuestionRepository
        {
            get
            {
                if (this._questionRepository == null)
                {
                    this._questionRepository = new BaseRepository<QuestionModel>(_context);
                }
                return _questionRepository;
            }
        }
        public BaseRepository<AnswerModel> AnswerRepository
        {
            get
            {
                if (this._answerRepository == null)
                {
                    this._answerRepository = new BaseRepository<AnswerModel>(_context);
                }
                return _answerRepository;
            }
        }
        public BaseRepository<AnswerResponseModel> AnswerResponseRepository
        {
            get
            {
                if (this._answerResponseRepository == null)
                {
                    this._answerResponseRepository = new BaseRepository<AnswerResponseModel>(_context);
                }
                return _answerResponseRepository;
            }
        }

        public BaseRepository<FeedbackModel> FeedBackRepository
        {
            get
            {
                if (this._feedbackRepository == null)
                {
                    this._feedbackRepository = new BaseRepository<FeedbackModel>(_context);
                }
                return _feedbackRepository;
            }
        }
        public BaseRepository<UserModel> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new BaseRepository<UserModel>(_context);
                }
                return _userRepository;
            }
        }
        public BaseRepository<ClassifiedModel> ClassifiedRepository
        {
            get
            {
                if (this._classifiedRepository == null)
                {
                    this._classifiedRepository = new BaseRepository<ClassifiedModel>(_context);
                }
                return _classifiedRepository;
            }
        }
        public BaseRepository<CommentModel> CommentRepository
        {
            get
            {
                if (this._commentRepository == null)
                {
                    this._commentRepository = new BaseRepository<CommentModel>(_context);
                }
                return _commentRepository;
            }
        }
        public BaseRepository<ThreadModel> ThreadRepository
        {
            get
            {
                if (this._threadRepository == null)
                {
                    this._threadRepository = new BaseRepository<ThreadModel>(_context);
                }
                return _threadRepository;
            }
        }
        public BaseRepository<LikeModel> LikeRepository
        {
            get
            {
                if (this._likeRepository == null)
                {
                    this._likeRepository = new BaseRepository<LikeModel>(_context);
                }
                return _likeRepository;
            }
        }
        public BaseRepository<ViewsModel> ViewsRepository
        {
            get
            {
                if (this._viewsRepository == null)
                {
                    this._viewsRepository = new BaseRepository<ViewsModel>(_context);
                }
                return _viewsRepository;
            }
        }
        public BaseRepository<FollowModel> FollowRepository
        {
            get
            {
                if (this._followRepository == null)
                {
                    this._followRepository = new BaseRepository<FollowModel>(_context);
                }
                return _followRepository;
            }
        }
        public BaseRepository<WatchModel> WatchRepository
        {
            get
            {
                if (this._watchRepository == null)
                {
                    this._watchRepository = new BaseRepository<WatchModel>(_context);
                }
                return _watchRepository;
            }
        }

        public async Task<List<string>> GetTrendingTagsGeneralAsync()
        {
            // var context1 = new SiteContext();
            using (var context1 = new SiteContext())
            {
                var results =
                    context1.Threads.GroupBy(x => x.TagsGeneral)
                        .Select(grp => new { Name = grp.Key, Count = grp.Count() })
                        .OrderByDescending(x => x.Count);

                return
                    await
                        (from item in results
                         where !String.IsNullOrEmpty(item.Name)
                         select item.Name + " (" + item.Count + ") ").ToListAsync();
            }
        }
        public async Task<List<string>> GetTrendingTagsAnnouncementAsync()
        {
            using (var context2 = new SiteContext())
            {
                var results =
                    context2.Threads.GroupBy(x => x.TagsAnnouncement)
                        .Select(grp => new { Name = grp.Key, Count = grp.Count() })
                        .OrderByDescending(x => x.Count);

                return
                    await
                        (from item in results
                         where !String.IsNullOrEmpty(item.Name)
                         select item.Name + " (" + item.Count + ") ").ToListAsync();
            }
        }


        public List<int> ThreadsILiked(string username)
        {
            var likeModel = _context.Likes.Where(x => x.LikedBy == username).OrderByDescending(x => x.LikedOn).ToList();
            return likeModel.Select(like => like.ThreadId).ToList();
        }

        public List<int> ThreadsIWatched(string username)
        {
            var watchModel = _context.Watch.Where(x => x.WatchedBy == username).ToList();
            return watchModel.Select(watch => watch.ThreadId).ToList();
        }

        public UserModel IsValidUser(string emailOrUsername, string password)
        {
            var passwordE = Encryption.Encrypt(password);
            var user = _context.Users.SingleOrDefault(x => (x.Email.Equals(emailOrUsername) || x.Username.Equals(emailOrUsername)) && x.Password.Equals(passwordE));
            return user;
        }

        public async Task<List<ThreadModel>> GetMostViewedThreads(int num)
        {
            using (var context3 = new SiteContext())
            {
                var rows =
                    context3.Threads.SqlQuery("SELECT TOP " + num + " * FROM " + Constants.SchemaName + "[Threads]" +
                                              "ORDER BY Views desc").ToListAsync();
                return await rows;
            }
        }
        public async Task<List<ThreadModel>> GetMostLikedThreads(int num)
        {
            using (var context4 = new SiteContext())
            {
                var rows = context4.Threads.SqlQuery("SELECT TOP " + num + " * FROM " + Constants.SchemaName + "[Threads]" + "ORDER BY Likes desc").ToListAsync();
                return await rows;
            }
            //  var context4 = new SiteContext();

        }

        public async Task<List<ThreadModel>> GetMostCommentedThreads(int num)
        {
            using (var context5 = new SiteContext())
            {
                var rows =
                    context5.Threads.SqlQuery("SELECT TOP " + num + " * FROM " + Constants.SchemaName + "[Threads]" +
                                              "ORDER BY Responses desc").ToListAsync();
                return await rows;
            }
        }

        public List<QuestionModel> GetRecentQuestions(int blockSize)
        {


            var query = "Select Top " + blockSize + " * from " + Constants.SchemaName +
                           " [Questions]" + "order by LastUpdated desc";

            return _context.Database.SqlQuery<QuestionModel>(query).ToList();
            // return rows.ToList();
        }




        public async Task<List<ThreadModel>> GetRecentRecordsAsync(int blockSize)
        {
            using (var context6 = new SiteContext())
            {
                var model =
                    context6.Threads.SqlQuery("Select Top " + blockSize + " * from " + Constants.SchemaName +
                                              " [Threads]" + "order by LastUpdated desc");
                return await model.ToListAsync();
            }
        }
        public IEnumerable<QuizModel> GetRecentQuiz(int blockSize, int page)
        {
            var offset = (page - 1) * blockSize;
            /*var query = "Select Top " + blockSize + " * from " + Constants.SchemaName +
                        " [Quiz] q INNER JOIN " + Constants.SchemaName +
                        " [QuizAnswer] qa on q.QuizId=qa.Question_QuizId " + "order by q.LastUpdated desc";
            return _context.Database.SqlQuery<QuizModel>(query).ToList();*/
            //.Skip(offset).Take((int)(offset > Constants.BlocksizeMax ? offset - (offset - Constants.BlocksizeMax) : offset == 0 ? Constants.BlocksizeMax : offset)).OrderByDescending(x => x.AskedOn)
            return _context.Quiz.Include(x => x.QuizAnswers)
                .OrderByDescending(x=>x.LastUpdated)
                .Skip(offset).Take((int)(offset > blockSize ? offset - (offset - blockSize) : offset == 0 ? blockSize : offset))
                .ToList();
            
        }
        public async Task<List<ClassifiedModel>> GetRecentClassifieds(int blockSize)
        {
            using (var context7 = new SiteContext())
            {
                var classifieds =
                    context7.Classified.SqlQuery("Select Top " + blockSize + " * from " + Constants.SchemaName +
                                                 " [Classifieds]" + "where CurrentStatus = " + Constants.ClassifiedItemAvailable + " order by LastUpdated desc");
                return await classifieds.ToListAsync();
            }
        }
        /* pulled all record at once now - we might have to change this to pull record per blocksize */
        public IEnumerable<AccountInfoViewModel> GetAccountDetails(string username)
        {
            var query = "SELECT users.CreatedOn,users.Email,users.LastSuccessfulLogin,thread.ThreadId,thread.ThreadRadio,thread.Title,thread.Announcement,thread.Likes,thread.Responses,"
                + " thread.Views,thread.Watch,thread.SubmittedOn FROM " + Constants.SchemaName + " [Users] users INNER JOIN" + Constants.SchemaName + " [Threads]thread"
                + " ON users.Username = thread.Username where users.Username=" + "'" + username + "'" + "order by thread.SubmittedOn desc";

            return _context.Database.SqlQuery<AccountInfoViewModel>(query).ToList();
        }

        public IEnumerable<AccountInfoViewModel> GetCommentedThreadDetails(string username)
        {
            var query = "SELECT comment.Thread_ThreadId,comment.CommentedOn,thread.ThreadId,thread.ThreadRadio,thread.Title,thread.Announcement,thread.Likes,thread.Responses,"
                + " thread.Views,thread.Watch,thread.SubmittedOn FROM " + Constants.SchemaName + " [Comment] comment INNER JOIN" + Constants.SchemaName + " [Threads]thread"
                + " ON comment.Thread_ThreadId = thread.ThreadId where comment.CommentedBy=" + "'" + username + "'" + "order by comment.CommentedOn desc";

            return _context.Database.SqlQuery<AccountInfoViewModel>(query).ToList();
        }
        public IEnumerable<AccountInfoViewModel> GetLikedThreadDetails(string username)
        {
            var query = "SELECT likes.ThreadId,likes.LikedOn,thread.ThreadId,thread.ThreadRadio,thread.Title,thread.Announcement,thread.Likes,thread.Responses,"
                + " thread.Views,thread.Watch,thread.SubmittedOn FROM " + Constants.SchemaName + " [Likes] likes INNER JOIN" + Constants.SchemaName + " [Threads]thread"
                + " ON likes.ThreadId = thread.ThreadId where likes.LikedBy=" + "'" + username + "'" + "order by likes.LikedOn desc";

            return _context.Database.SqlQuery<AccountInfoViewModel>(query).ToList();
        }
        public IEnumerable<AccountInfoViewModel> GetViewedThreadDetails(string username)
        {
            var query = "SELECT views.ThreadId,views.ViewedOn,thread.ThreadId,thread.ThreadRadio,thread.Title,thread.Announcement,thread.Likes,thread.Responses,"
                + " thread.Views,thread.Watch,thread.SubmittedOn FROM " + Constants.SchemaName + " [Views] views INNER JOIN" + Constants.SchemaName + " [Threads]thread"
                + " ON views.ThreadId = thread.ThreadId where views.ViewedBy=" + "'" + username + "'" + "order by views.ViewedOn desc";

            return _context.Database.SqlQuery<AccountInfoViewModel>(query).ToList();
        }
        public IEnumerable<AccountInfoViewModel> GetWatchThreadDetails(string username)
        {
            var query = "SELECT watch.ThreadId,watch.WatchedOn,thread.ThreadId,thread.ThreadRadio,thread.Title,thread.Announcement,thread.Likes,thread.Responses,"
                + " thread.Views,thread.Watch,thread.SubmittedOn FROM " + Constants.SchemaName + " [Watch] watch INNER JOIN" + Constants.SchemaName + " [Threads]thread"
                + " ON watch.ThreadId = thread.ThreadId where watch.WatchedBy=" + "'" + username + "'" + "order by watch.WatchedOn desc";

            return _context.Database.SqlQuery<AccountInfoViewModel>(query).ToList();
        }

        public IEnumerable<QuestionModel> GetAnswers(int questionId)
        {
            var qa = _context.Question.Where(x => x.QuestionId == questionId)
                .Include(y => y.Answers)
                .Include(x => x.AnswerResponse)
                .ToList();
            foreach (var item in qa)
            {
                if (item.Answers != null)
                    item.Answers = item.Answers.OrderByDescending(z => z.AnsweredOn).ToList();
            }
            return qa;

            /*select q.QuestionId,q.Username,q.Category,q.Question,q.AskedOn,q.LastUpdated,
        a.QuestionQuestionId,a.Answer,a.AnsweredOn
        from [SB].[dbo].[Questions]q inner join [SB].[dbo].[Answers]a on
        q.QuestionId = a.QuestionQuestionId order by a.AnsweredOn desc;

           var query = "select q.QuestionId,q.Username,q.Category,q.Question,q.AskedOn,q.LastUpdated,a.QuestionQuestionId,a.UsefulYes,a.UsefulNo,a.UserfulSomeWhat," +
                        " a.Answer,a.AnsweredOn from "+ Constants.SchemaName+" [Questions]q inner join "+Constants.SchemaName+" [Answers]a " +
                        " ON q.QuestionId = a.QuestionQuestionId where q.QuestionId="+ "'" +questionId+ "'"+ "order by a.AnsweredOn desc";

           return _context.Database.SqlQuery<QaViewModel>(query).ToList();*/
        }

        public int GetTotalPostByUser(string username)
        {
            return _context.Threads.Count(x => x.Username.Equals(username));
        }

        public int GetTotalCommentsByUser(string username)
        {
            return _context.Comment.Count(x => x.CommentedBy.Equals(username));
        }
        public int GetTotalLikesByUser(string username)
        {
            return _context.Likes.Count(x => x.LikedBy.Equals(username));
        }
        public int GetTotalViewsByUser(string username)
        {
            return _context.Views.Count(x => x.ViewedBy.Equals(username));
        }
        public int GetTotalWatchByUser(string username)
        {
            return _context.Watch.Count(x => x.WatchedBy.Equals(username));
        }

        public void Unlike(int threadId, string username, int currentLike)
        {
            var itemToRemove = _context.Likes.SingleOrDefault(x => x.ThreadId == threadId && x.LikedBy == username);
            if (itemToRemove != null)
            {
                _context.Likes.Remove(itemToRemove);
                _context.SaveChanges();
            }
            var itemToUpdate = _context.Threads.SingleOrDefault(x => x.ThreadId == threadId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Likes = currentLike - 1;
                _context.SaveChanges();
            }
        }
        public bool UpdatePassword(string username, string old, string newpass)
        {
            var row = _context.Users.Single(x => x.Username.Equals(username));
            var oldEncrypted = Encryption.Encrypt(old);
            if (!row.Password.Equals(oldEncrypted))
            {
                return false;
            }
            row.Password = Encryption.Encrypt(newpass);
            row.ConfirmPassword = row.Password;
            _context.SaveChanges();
            return true;
        }

        public void Save()
        {
            _context.SaveChanges();

        }
        public bool SendEmail(string address, string subject, string message)
        {
            var loginInfo = new NetworkCredential(Constants.FromEmailAddress, Constants.Password);
            var msg = new MailMessage();
            var smtpClient = new SmtpClient(Constants.SmptClient, Constants.PortNumber);

            msg.From = new MailAddress(Constants.FromEmailAddress);
            msg.To.Add(new MailAddress(address));
            msg.Subject = subject;
            msg.Body = message;
            msg.IsBodyHtml = true;

            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = loginInfo;
            try
            {
                smtpClient.Send(msg);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public void DeleteRange(int threadId)
        {
            _context.Views.RemoveRange(_context.Views.Where(x => x.ThreadId == threadId));
        }

        public int DoesUserNameOrEmailExist(string input)
        {
            if (_context.Users.Any(x => x.Username.Equals(input)))
                return 1;


            if (_context.Users.Any(x => x.Email.Equals(input)))
                return 2;


            return 0;
        }
        public bool DoesEmailOnlyExist(string input)
        {
            if (_context.Users.Any(x => x.Email.Equals(input)))
                return true;
            return false;
        }
        public bool DoesUserNameOnlyExist(string input)
        {
            if (_context.Users.Any(x => x.Username.Equals(input)))
                return true;
            return false;

        }
        public async Task<UserModel> DoesEmailExistStringAsync(string input)
        {

            var user = await _context.Users.SingleAsync(x => x.Email.Equals(input));
            return user;

        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }



        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
