using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Employee
    {
        public string Email { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public int Salary { get; set; }
        public int Seniority { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public bool IsMarried { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public bool IsManager { get; set; }


        public Employee(string _email, string _fname, string _lname, int _salary,
            int _seniority, string _phoneNo, string _address, DateTime _startDate,
            DateTime _bdate, string _gender, bool _isMarried, string _pos, string _dep)
        {

            Email = _email;
            Fname = _fname;
            Lname = _lname;
            Salary = _salary;
            Seniority = _seniority;
            PhoneNo = _phoneNo;
            Address = _address;
            StartDate = _startDate;
            BirthDate = _bdate;
            Gender = _gender;
            IsMarried = _isMarried;
            Position = _pos;
            Department = _dep;
            
        }

        public Employee(string _email, string _fname, string _lname, int _salary,
           int _seniority, string _phoneNo, string _address,
           DateTime _bdate, string _gender, bool _isMarried, string _pos, string _dep)
        {

            Email = _email;
            Fname = _fname;
            Lname = _lname;
            Salary = _salary;
            Seniority = _seniority;
            PhoneNo = _phoneNo;
            Address = _address;
            BirthDate = _bdate;
            Gender = _gender;
            IsMarried = _isMarried;
            Position = _pos;
            Department = _dep;
            
        }


        public Employee()
        {

        }

        public List<Employee> GetEmployees()
        {
            DBService dbs = new DBService();
            return dbs.GetEmployees();
        }

        public Employee GetByEmail(string email)
        {
            DBService dbs = new DBService();
            return dbs.GetEmployeeByEmail(email);
        }

        public List<Employee> GetEmployees(string managerEmail)
        {
            DBService dbs = new DBService();
            return dbs.GetEmployees(managerEmail);
        }

        public List<Employee> GetRegisteredEmployees()
        {
            DBService dbs = new DBService();
            return dbs.GetRegisteredEmployees();
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

        public string delete(string email)
        {
            DBService dbs = new DBService();
            return dbs.Delete(email);
        }

        public string delete(string email, string managerEmail)
        {
            DBService dbs = new DBService();
            return dbs.Delete(email, managerEmail);
        }

        public Employee Update()
        {
            DBService dbs = new DBService();
            return dbs.Update(this);
        }
    }
}