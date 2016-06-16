using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SkillsBook.Models.DAL
{
    public class Constants
    {

        public static string SchemaName = "[DB_9FFD81_sajhaspace].[dbo].";
        public static int BlocksizeInitialLoad = 25;
        public static int BlocksizeMostViewed = 5;
        public static int BlocksizeMostLiked = 5;
        public static int BlocksizeMostCommented = 5;
        public static  int BlocksizeUserSpecificThreads = 15;
        public static int BlocksizeMax = 100;
        public static int BlocksizeTrendingTags = 5;
        public static int BlocksizeTest = 1;
        public static string TagsGeneral = "TagsGeneral";
        public static string TagsAnnouncement = "TagsAnnouncement";
        public static int BlocksizeClassifieds = 25;

        //public static string Domain = "www.SajhaCommunity.com";
        public static string Slogan = "Uniting Communities Together";

        //email

        public static string FromEmailAddress = "postmaster@gufgaaf.com";
        public static string Password = "Hanumanji77@";
        public static string SmptClient = "mail.gufgaaf.com";
        public static int PortNumber = 25;

        public static string Subject = "Account Recover";
        private const string Dear = "Dear _username,<br />";

        private const string Message = "Please find the temporary password we mailed to you. You may want to login with temporary password and change the password once you login." + "Your temporary password is <strong>_temppassword</strong><br />";

        private static readonly string Footer = "<br />"+ Domain+"<br /> Keep browsing and keep having fun.";

        public static string Body = String.Format("{0},{1},{2}", Dear, Message,Footer);

        public static int ClassifiedItemAvailable = 1;
        public static int ClassifiedItemTaken = 0;


        public static string Available = "available";
        public static string Looking= "still looking";
        public static string Wanted = "wanted";

        public static int Useful = 0;
        public static int Somewhat = 1;
        public static int NotUseful = 2;
        

        public static string Domain
        {
            get
            {
                var appPath = string.Empty;

                var context = HttpContext.Current;

                if (context != null)
                {
                    appPath = string.Format("{0}://{1}{2}{3}",
                                            context.Request.Url.Scheme,
                                            context.Request.Url.Host,
                                            context.Request.Url.Port == 80
                                                ? string.Empty
                                                : ":" + context.Request.Url.Port,
                                            context.Request.ApplicationPath);
                }

               /* if (!appPath.EndsWith("/"))
                    appPath += "/";*/

                return appPath;
            }
        }
    }
}
