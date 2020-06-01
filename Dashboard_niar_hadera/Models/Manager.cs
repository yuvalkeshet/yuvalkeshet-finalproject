using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Manager : Employee
    {
        public float Bonus { get; set; }

        public Manager(string _email, string _fname, string _lname, int _salary,
            int _seniority, string _phoneNo, string _address, DateTime _startDate,
            DateTime _bdate, string _gender, bool _isMarried, string _pos, string _dep, float _bonus = 1)
            : base(_email, _fname, _lname, _salary,
             _seniority, _phoneNo, _address, _startDate,
             _bdate, _gender, _isMarried, _pos, _dep)
        {
            Bonus = _bonus;
        }

        public Manager(string _email, string _fname, string _lname, int _salary,
            int _seniority, string _phoneNo, string _address,
            DateTime _bdate, string _gender, bool _isMarried, string _pos, string _dep, float _bonus = 1)
            : base(_email, _fname, _lname, _salary,
            _seniority, _phoneNo, _address,
            _bdate, _gender, _isMarried, _pos, _dep)
        {
            Bonus = _bonus;
        }

        public Manager()
        {

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
    }
}