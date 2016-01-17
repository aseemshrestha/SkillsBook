using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SkillsBook.com.LOL
{
    public class Lol
    {
        public string GetJokes()
        {
            var dict = new Dictionary<string, string>
            {
                {"1", "Son: I am not able to go to school today.Father: what happened? Son: I am not feeling well Father: Where you are not feeling well? Son: In school!"},
                {"anotherUrl", "another Url"}
            };

            return  JsonConvert.SerializeObject(dict, Formatting.Indented);
            

        }
    }
}