﻿using System.Web;
using System.Web.Mvc;

namespace Dashboard_niar_hadera
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
