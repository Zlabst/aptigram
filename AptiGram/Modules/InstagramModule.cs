using System.Net;
using InstagramCSharp.Enums;
using InstagramCSharp.Models;
using InstagramCSharp.OAuth;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Configuration;

namespace AptiGram.Modules
{
    public class InstagramModule : NancyModule
    {
        public InstagramModule()
        {
            Get["/", true] = async (x, ct) =>
                {
                    using (var client = new WebClient())
                    {
                        return client.DownloadString(new Uri("https://api.instagram.com/v1/users/2261366739/media/recent?access_token=" + ConfigurationManager.AppSettings["access_token"]));
                    }
                };
        }
    }
}