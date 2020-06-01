using Dashboard_niar_hadera.Models;
using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard_niar_hadera.Controllers
{
    public class MalufunctionController : ApiController
    {
        [HttpGet]
        [Route("api/malfunction")]
        public IEnumerable<Malfunction> Get()
        {
            Malfunction m = new Malfunction();
            return m.Read();

        }

        //[Route("api/malfunction")]
        //public void Post([FromBody]Shift s, List<int> malf1, List<int> malf3)
        //{
        //    Malfunction m = new Malfunction();
        //    //m.insert(s, malf1, malf3);
        //}
    }
}
