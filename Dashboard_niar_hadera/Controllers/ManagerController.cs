using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Dashboard_niar_hadera.Controllers
{
    public class ManagerController : ApiController
    {
        [Route("api/manager")]
        public IHttpActionResult Post([FromBody]Manager manager)
        {
            return Json(new { email = manager.insert() });
        }

        [Route("api/manager")]
        public IHttpActionResult Post([FromBody]Manager manager, string managerEmail)
        {
            return Json(new { email = manager.insert(managerEmail) });
        }
    }
}