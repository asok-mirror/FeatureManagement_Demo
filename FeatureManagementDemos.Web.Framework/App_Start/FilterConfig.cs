﻿using System.Web;
using System.Web.Mvc;

namespace FeatureManagementDemos.Web.Framework
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
