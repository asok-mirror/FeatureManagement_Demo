using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unleash;
using Unleash.Strategies;

namespace FeatureManagementDemos.Web.Framework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IUnleash Unleash { get; private set; }
        public static UnleashSettings UnleashSettings { get; private set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnleashSettings = new UnleashSettings()
            {               
                AppName = "dot-net-client-test",
                Environment = "development",
                UnleashApi = new Uri("http://localhost:4242/api/"),
                ProjectId = "default",
                SendMetricsInterval = TimeSpan.FromMilliseconds(30),
                CustomHttpHeaders = new Dictionary<string, string>()
                {
                    { "Authorization", "default:development.a6e2cdcb4f6251402836c57cb3af67f33e88bd3852b105fd8b17a095" }
                },
               // FetchTogglesInterval = TimeSpan.FromMinutes(5)                             
            };            

            UnleashInfo = UnleashSettings.ToString();

            IStrategy s1 = new PhoneNumbertrategy();

            Unleash = new DefaultUnleash(UnleashSettings, s1);
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

