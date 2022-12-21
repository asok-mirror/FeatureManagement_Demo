using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unleash;
using Unleash.Internal;

namespace WebApp
{
    public class WebApiApplication : HttpApplication
    {
        public static IUnleash Unleash { get; private set; }
        public static UnleashSettings UnleashSettings { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnleashSettings = new UnleashSettings()
            {
                //UnleashApi = new Uri("http://unleash.herokuapp.com/api/"),
                ////UnleashApi = new Uri("http://localhost:4242/api/"),
                //AppName = "dotnet-api-test",
                //InstanceTag = "instance 1",
                //SendMetricsInterval = TimeSpan.FromSeconds(20),
                //UnleashContextProvider = new AspNetContextProvider(),
                ////JsonSerializer = new JsonNetSerializer()

                AppName = "dot-net-client",
                Environment = "development",
                UnleashApi = new Uri("http://localhost:4242/api/"),
                ProjectId = "default",
                SendMetricsInterval = TimeSpan.FromMinutes(5),
                CustomHttpHeaders = new Dictionary<string, string>()
                {
                    { "Authorization", "default:development.a6e2cdcb4f6251402836c57cb3af67f33e88bd3852b105fd8b17a095" }
                },
                FetchTogglesInterval = TimeSpan.FromMinutes(5)
                //BootstrapOverride = false, // Defaults to true               
            };
            //UnleashSettings.UseBootstrapFileProvider("");

            //Uncomment to test bootstrapping via url. This particular url points to the BootstapHost, a thin webapp hosting a bootstrap.json file
           // UnleashSettings.BootstrapOverride = true;
          //  UnleashSettings.UseBootstrapFileProvider(@"C:\toolbox\un-leash\unleash-client-dotnet\samples\FeatureManagementDemos\WebApp\featureToggle.json");
            //UnleashSettings.ToggleBootstrapProvider = 

            UnleashInfo = UnleashSettings.ToString();
            
            Unleash = new DefaultUnleash(UnleashSettings);
        }

        public static string UnleashInfo;

        protected void Application_End()
        {
            Unleash?.Dispose();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Items["UnleashContext"] = new UnleashContext
            {
                UserId = "ABC",
                SessionId = HttpContext.Current.Session?.SessionID,
                RemoteAddress = HttpContext.Current.Request.UserHostAddress,
                Properties = new Dictionary<string, string>()
                {
                    {"UserRoles", "A, B, C"}
                }
            };
        }
    }

    public class AspNetContextProvider : IUnleashContextProvider
    {
        public UnleashContext Context =>
            HttpContext.Current?.Items["UnleashContext"] as UnleashContext;
    }
}