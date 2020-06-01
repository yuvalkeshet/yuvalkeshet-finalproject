using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegDate { get; set; }
        public string Image { get; set; }
        public bool isActive { get; set; }
        public bool isManager { get; set; }
        public DateTime? LastLogin { get; set; }


        public User(string _email, string _password, bool _isActive,
            DateTime _regDate, DateTime? _lastLogin)
        {

            Email = _email;
            Password = _password;
            RegDate = _regDate;
            isActive = _isActive;
            LastLogin = _lastLogin;
        }

        public User(string _email, string _password, bool _isActive,
           DateTime _regDate, DateTime? _lastLogin, string _image)
        {

            Email = _email;
            Password = _password;
            RegDate = _regDate;
            isActive = _isActive;
            LastLogin = _lastLogin;
            Image = _image;
        }

        public User(string _email, string _password, bool _isActive)
        {

            Email = _email;
            Password = _password;
            isActive = _isActive;
        }


        public User()
        {

        }

        public User Login(string login_details)
        {
            DBService dbs = new DBService();
            return dbs.Login(login_details);
        }

        public List<User> getUsers()
        {
            DBService dbs = new DBService();
            return dbs.GetUsers();
        }

        public List<User> getUsers(string managerEmail)
        {
            DBService dbs = new DBService();
            return dbs.GetUsers(managerEmail);
        }

        public List<User> getUsersByDep(string depName)
        {
            DBService dbs = new DBService();
            return dbs.GetUsersByDep(depName);
        }

        public User getUserByEmail(string email)
        {
            DBService dbs = new DBService();
            return dbs.GetUserByEmail(email);
        }

        public string insert()
        {
            DBService dbs = new DBService();
            return dbs.insert(this);
        }

        public string insert(string managerEmail)
        {
            DBService dbs = new DBService();
            return dbs.insert(this, managerEmail);
        }

        public User Update()
        {
            DBService dbs = new DBService();
            return dbs.Update(this);
        }

        public void UpdatePassword()
        {
            DBService dbs = new DBService();
            dbs.UpdatePassword(this);
        }

        public void RestorePassword(string email)
        {
            DBService dbs = new DBService();
            dbs.RestorePassword(email);
        }

        public string delete(string email)
        {
            DBService dbs = new DBService();
            return dbs.DeleteUser(email);
        }
    }
}