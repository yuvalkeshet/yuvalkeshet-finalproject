using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Dashboard_niar_hadera.Models
{
    public class KPI
    {
        public DateTime Timestamp { get; set; }
        public float OutputLine1 { get; set; }
        public float OutputLine3 { get; set; }
        public float OutputMachine2 { get; set; }
        public float OutputMachine8 { get; set; }
        public float PalperConcentration1 { get; set; }
        public float PalperConcentration3 { get; set; }
        public float TargetOutputLine1 { get; set; }
        public float TargetOutputLine3 { get; set; }
        public float TargetPalperConcentration1 { get; set; }
        public float TargetPalperConcentration3 { get; set; }

        public KPI()
        {

        }
     
        public KPI(float line1, float line3, float mach2, float mach8, 
            float palp1, float palp3, float tarLine1, float tarLine3, float tarPalp1, float tarPalp3)
        {
            OutputLine1 = line1;
            OutputLine3 = line3;
            OutputMachine2 = mach2;
            OutputMachine8 = mach8;
            PalperConcentration1 = palp1;
            PalperConcentration3 = palp3;
            TargetOutputLine1 = tarLine1;
            TargetOutputLine3 = tarLine3;
            TargetPalperConcentration1 = tarPalp1;
            TargetPalperConcentration3 = tarPalp3;


        }

        public KPI(DateTime timestamp, float line1, float line3, float mach2, float mach8,
            float palp1, float palp3, float tarLine1, float tarLine3, float tarPalp1, float tarPalp3)
        {
            Timestamp = timestamp;
            OutputLine1 = line1;
            OutputLine3 = line3;
            OutputMachine2 = mach2;
            OutputMachine8 = mach8;
            PalperConcentration1 = palp1;
            PalperConcentration3 = palp3;
            TargetOutputLine1 = tarLine1;
            TargetOutputLine3 = tarLine3;
            TargetPalperConcentration1 = tarPalp1;
            TargetPalperConcentration3 = tarPalp3;


        }


        public KPI insert()
        {
            DBService dbs = new DBService();
            return dbs.insert(this);
        }

        public List<KPI> GetKPIs()
        {
            DBService dbs = new DBService();
            return dbs.GetKPIs();
        }

        public List<KPI> GetKPIsPerHour()
        {
            DBService dbs = new DBService();
            return dbs.GetKPIsPerHour();
        }

        public KPI GetLatestKPI()
        {
            DBService dbs = new DBService();
            return dbs.GetLatestKPI();
        }
    }
}