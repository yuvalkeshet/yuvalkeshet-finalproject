using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Department
    {
        public string DepName { get; set; }

        public Department(string _depName)
        {
            DepName = _depName;
        }

        public Department()
        {

        }

        public List<Department> Read()
        {
            DBService dbs = new DBService();
            return dbs.GetDepartments();
        }

        public List<Position> GetPositions()
        {
            DBService dbs = new DBService();
            return dbs.GetPositionsByDep(this);
        }
    }
}