using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Dashboard_niar_hadera.Controllers
{
    public class DepartmentController : ApiController
    {
        [HttpGet]
        [Route("api/departments")]
        public IEnumerable<Department> Get()
        {
            Department dep = new Department();
            return dep.Read();

        }

        [HttpGet]
        [Route("api/department/getPositions")]
        public IEnumerable<Position> Get(string depName)
        {
            Department dep = new Department(depName);
            return dep.GetPositions();

        }
    }
}