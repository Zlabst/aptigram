using System;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.LightningCache.Extensions;
using Nancy.Routing;
using Nancy.TinyIoc;
using System.Web.Hosting;

namespace AptiGram
{
    public class AptiGramBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;
            base.ApplicationStartup(container, pipelines);
            this.EnableLightningCache(container.Resolve<IRouteResolver>(), ApplicationPipelines, new UrlHashKeyGenerator());
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                                                      .WithHeader("Access-Control-Allow-Methods", "GET")
                                                                      .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type"));
        }

        public class UrlHashKeyGenerator : Nancy.LightningCache.CacheKey.ICacheKeyGenerator
        {
            public string Get(Request request)
            {
                using (var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.Url.ToString()));
                    return Convert.ToBase64String(hash);
                }
            }
        }
    }
}