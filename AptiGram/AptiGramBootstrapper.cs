using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace AptiGram
{
    public class AptiGramBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;
            base.ApplicationStartup(container, pipelines);
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) => ctx.Response.WithHeader("Access-Control-Allow-Origin", "*")
                                                                      .WithHeader("Access-Control-Allow-Methods", "GET")
                                                                      .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type"));
        }
    }
}