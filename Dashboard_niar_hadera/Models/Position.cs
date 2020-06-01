using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Position
    {
        public string PosName { get; set; }

        public Position(string _posName)
        {
            PosName = _posName;
        }

        public Position()
        {

        }
    }
}