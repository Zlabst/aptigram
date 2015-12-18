using System.Net;
using AptiGram.Models;
using Nancy;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Configuration;
using Octokit;
using System.Threading.Tasks;

namespace AptiGram.Modules
{
    public class BackgroundModule : NancyModule
    {
        public BackgroundModule()
        {
            Get["/background", true] = async (y, ct) =>
                {
                    using (var client = new WebClient())
                    {
                        var json = client.DownloadString(new Uri("https://api.instagram.com/v1/users/2261366739/media/recent?access_token=" + ConfigurationManager.AppSettings["access_token"]));
                        var parsedJson = JsonConvert.DeserializeObject<Rootobject>(json);

                        var publishedImages = parsedJson.data.Where(x => x.type == "image").Select(x => new PublishedImage
                            {
                                Caption = x.caption == null ? "" : x.caption.text,
                                Url = x.images.standard_resolution.url,
                                Location = x.location == null ? "" : x.location.name,
                            }).ToList();

                        var serializedJson = JsonConvert.SerializeObject(publishedImages);
                        await PublishToGitAsync(serializedJson);

                        return null;
                    }
                };
        }

        public async Task PublishToGitAsync(string json)
        {
            var credentials = new Credentials(ConfigurationManager.AppSettings["github_access_token"]);
            var connection = new Connection(new ProductHeaderValue("aptitud.github.io"))
            {
                Credentials = credentials
            };

            var client = new GitHubClient(connection);
            
            var contents = await client.Repository.Content.GetAllContents("aptitud", "aptitud.github.io", "instagram.json");
            string fileSha = contents.First().Sha;
            await client.Repository.Content.UpdateFile(
                "aptitud",
                "aptitud.github.io",
                "instagram.json", 
                new UpdateFileRequest("Update", json, fileSha));

        }
    }
}