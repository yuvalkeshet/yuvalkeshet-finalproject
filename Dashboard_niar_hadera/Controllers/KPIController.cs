using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace Dashboard_niar_hadera.Controllers
{
    public class KPIController : ApiController
    {
        [HttpGet]
        [Route("api/kpi")]
        public List<KPI> Get()
        {
            KPI kpi = new KPI();
            return kpi.GetKPIs();
        }

        [HttpGet]
        [Route("api/kpiPerHour")]
        public List<KPI> GetKPIsPerHour()
        {
            KPI kpi = new KPI();
            return kpi.GetKPIsPerHour();
        }

        [HttpGet]
        [Route("api/kpiLatest")]
        public KPI GetLatestKPI()
        {
            KPI kpi = new KPI();
            return kpi.GetLatestKPI();
        }

        [HttpPost]
        [Route("api/kpi")]
        public KPI Post([FromBody]KPI kpi)
        {

            return kpi.insert();
        }

    }
}