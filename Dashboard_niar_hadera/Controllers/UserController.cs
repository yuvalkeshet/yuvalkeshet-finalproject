using Dashboard_niar_hadera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;

namespace Dashboard_niar_hadera.Controllers
{
    public class UserController : ApiController
    {

        [Route("api/user")]
        public IHttpActionResult Post([FromBody]User u)
        {
            return Json(new { email = u.insert() });
        }

        [Route("api/user")]
        public IHttpActionResult Post([FromBody]User u, string managerEmail)
        {
            return Json(new { email = u.insert(managerEmail) });
        }

        [HttpGet]
        [Route("api/users/login")]
        public User Get(string LoginDetails)
        {
            User p = new User();
            return p.Login(LoginDetails);
        }

        [HttpGet]
        [Route("api/users")]
        public List<User> Get()
        {
            User u = new User();
            return u.getUsers();
        }

        [HttpGet]
        [Route("api/users/by_department")]
        public List<User> GetByDepartment(string depName)
        {
            User u = new User();
            return u.getUsers(depName);
        }

        [HttpGet]
        [Route("api/users")]
        public List<User> GetByManager(string managerEmail)
        {
            User u = new User();
            return u.getUsers(managerEmail);
        }

        [HttpGet]
        [Route("api/user")]
        public User GetByEmail(string email)
        {
            User u = new User();
            return u.getUserByEmail(email);
        }

        [HttpPut]
        [Route("api/user/update")]
        public User Put([FromBody]User u)
        {
            return u.Update();
        }

        [HttpPut]
        [Route("api/user/update_password")]
        public void PutPass([FromBody]User u)
        {
            u.UpdatePassword();
        }

        [HttpPut]
        [Route("api/user/restore_password")]
        public void Put(string email)
        {
            User u = new User();
            u.RestorePassword(email);
        }

        [HttpDelete]
        [Route("api/user/delete")]
        public string Delete(string email)
        {
            User u = new User();

            return u.delete(email);
        }
    }
}