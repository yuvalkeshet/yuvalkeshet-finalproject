using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Dashboard_niar_hadera.Controllers
{
    public class ShiftController : ApiController
    {
        [HttpPost]
        [Route("api/constraints")]
        public List<Shift> PostConstraints([FromBody]List<Shift> shifts, string email)
        {
            Shift s = new Shift();
            return s.insertConstraints(shifts, email);
        }

        [HttpPost]
        [Route("api/shifts")]
        public List<Shift> PostShifts([FromBody]List<Shift> shifts)
        {
            Shift s = new Shift();
            return s.insertShifts(shifts);
        }

        [HttpPost]
        [Route("api/shift")]
        public Shift Post([FromBody]Shift s)
        {
            return s.insert();
        }

        [HttpPost]
        [Route("api/shift/notes")]
        public void PostNotes([FromBody]Shift s)
        {           
            s.insertNotes();
        }

        [HttpPost]
        [Route("api/shift/malfunctions")]
        public void PostMalfs([FromBody]Shift s)
        {
            s.insertMalfunctions();
        }
        //[HttpPost]
        //[Route("api/shift/malfunctions")]
        //public void Post([FromBody]Shift s, List<int> malf1, List<int> malf3)
        //{
        //    s.insert(malf1, malf3);
        //}

        [HttpGet]
        [Route("api/shift")]
        public IEnumerable<Shift> Get()
        {
            Shift s = new Shift();
            return s.Read();

        }

        [HttpGet]
        [Route("api/constraints")]
        public List<Shift> GetConstraints(string email)
        {
            Shift s = new Shift();
            return s.ReadConstraints(email);
        }

        [HttpGet]
        [Route("api/constraints/approval")]
        public List<Shift> GetConstraintsToApprove(string email)
        {
            Shift s = new Shift();
            return s.ReadConstraintsToApprove(email);
        }

        [HttpGet]
        [Route("api/shifts")]
        public List<Shift> GetShifts(string dep)
        {
            Shift s = new Shift();
            return s.ReadShifts(dep);
        }

        [HttpGet]
        [Route("api/shifts/myEmployees")]
        public List<Shift> GetMyEmployeesShifts(string email)
        {
            Shift s = new Shift();
            return s.ReadMyEmployeesShifts(email);
        }

        [HttpGet]
        [Route("api/shifts/optimized")]
        public List<Shift> GenerateOptimizedShifts(string email)
        {
            Shift s = new Shift();
            return s.GenerateOptimizedShifts(email);
        }


    }
}
