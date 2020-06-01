using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models.DAL
{
    public class Malfunction
    {
        public int MalfunctionCode { get; set; }
        public string MalfunctionDes { get; set; }

        public Malfunction(int malfunctionCode, string malfunctionDes)
        {
            MalfunctionCode = malfunctionCode;
            MalfunctionDes = malfunctionDes;
        }

        public Malfunction()
        {

        }

        public List<Malfunction> Read()
        {
            DBService dbs = new DBService();
            return dbs.GetMalfunctionTypes();
        }
    }
}