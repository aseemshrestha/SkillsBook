using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
//using SkillsBook.Models.Migrations;
using SkillsBook.Models.Migrations;

namespace SkillsBook.Models.Models
{
    public class SiteContext : DbContext
    {
       public SiteContext()
            : base("Data Source=SQL5005.myASP.NET;Initial Catalog=DB_9FFD81_sajhaspace;User Id=DB_9FFD81_sajhaspace_admin;Password=saraswati77")
        {
       
        }
        public DbSet<UserModel> Users{ get; set; }
        public DbSet<ThreadModel> Threads { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<ViewsModel> Views { get; set; }
        public DbSet<FollowModel> Follow { get; set; }
        public DbSet<WatchModel> Watch { get; set; }
        public DbSet<CommentModel> Comment { get; set; }
        public DbSet<ClassifiedModel> Classified { get; set; }
        public DbSet<FeedbackModel> Feedback { get; set; }
        public DbSet<QuestionModel> Question { get; set; }
        public DbSet<AnswerModel> Answer{ get; set; }
        public DbSet<AnswerResponseModel> AnswerResponse{ get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
           //Database.SetInitializer<SiteContext>(new DropCreateDatabaseIfModelChanges<SiteContext>());
           Database.SetInitializer(new MigrateDatabaseToLatestVersion<SiteContext, Configuration>());
           // AutomaticMigrationDataLossAllowed = true;
           // new SiteContext().Database.Initialize(false); 

        }
    }
}
