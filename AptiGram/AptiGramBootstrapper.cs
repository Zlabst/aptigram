using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;

namespace AptiGram
{
    public class AptiGramBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            StaticConfiguration.DisableErrorTraces = false;
            base.ApplicationStartup(container, pipelines);
        }
    }
}