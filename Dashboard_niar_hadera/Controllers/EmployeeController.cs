using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Dashboard_niar_hadera.Controllers
{
    public class EmployeeController : ApiController
    {
        [Route("api/employee")]
        public IHttpActionResult Post([FromBody]Employee emp)
        {
            return Json(new { email = emp.insert() });
        }

        [Route("api/employee")]
        public IHttpActionResult Post([FromBody]Employee emp, string managerEmail)
        {
            return Json(new { email = emp.insert(managerEmail) });
        }

        [HttpDelete]
        [Route("api/employee/delete")]
        public string Delete(string email)
        {
            Employee emp = new Employee();

            return emp.delete(email);
        }

        [HttpDelete]
        [Route("api/employee/delete")]
        public string DeleteByManager(string email, string managerEmail)
        {
            Employee emp = new Employee();

            return emp.delete(email, managerEmail);
        }

        [HttpGet]
        [Route("api/employees")]
        public IEnumerable<Employee> Get()
        {
            Employee emp = new Employee();

            return emp.GetEmployees();
        }

        [HttpGet]
        [Route("api/employee")]
        public Employee Get(string email)
        {
            Employee emp = new Employee();

            return emp.GetByEmail(email);
        }

        [HttpGet]
        [Route("api/employees")]
        public IEnumerable<Employee> GetByManager(string managerEmail)
        {
            Employee emp = new Employee();

            return emp.GetEmployees(managerEmail);
        }

        [HttpGet]
        [Route("api/employees/registered")]
        public IEnumerable<Employee> GetRegisteredEmployees()
        {
            Employee emp = new Employee();

            return emp.GetRegisteredEmployees();
        }

        [HttpPut]
        [Route("api/employee/update")]
        public Employee Put([FromBody]Employee emp)
        {
            return emp.Update();
        }
    }
}