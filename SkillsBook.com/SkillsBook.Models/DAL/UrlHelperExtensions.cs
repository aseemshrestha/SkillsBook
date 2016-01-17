using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace SkillsBook.Models.DAL
{
    public static class UrlHelperExtensions
    {
        private static int _revisionNumber;

        public static string ContentVersioned(this UrlHelper urlHelper, string contentPath)
        {
            string url = urlHelper.Content(contentPath);
            int revisionNumber = GetRevisionNumber();

            return String.Format("{0}?v={1}", url, revisionNumber);
        }

        public static int GetRevisionNumber()
        {
            if (_revisionNumber == 0)
            {
                Version v = Assembly.GetExecutingAssembly().GetName().Version;

                _revisionNumber = v.Revision;
            }

            return _revisionNumber;
        }
    }
}