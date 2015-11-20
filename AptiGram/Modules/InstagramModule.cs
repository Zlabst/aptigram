using System.Net;
using AptiGram.Models;
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
            Get["/", true] = async (y, ct) =>
                {
                    using (var client = new WebClient())
                    {
                        var json =  client.DownloadString(new Uri("https://api.instagram.com/v1/users/2261366739/media/recent?access_token=" + ConfigurationManager.AppSettings["access_token"]));
                        var parsedJson = JsonConvert.DeserializeObject<Rootobject>(json);

                        return parsedJson.data.Where(x => x.type == "image").Select(x => new PublishedImage
                            {
                                Caption = x.caption.text,
                                Url = x.images.standard_resolution.url,
                                Location = x.location.name
                            });
                    }
                };
        }
    }
}