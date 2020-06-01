using Dashboard_niar_hadera.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dashboard_niar_hadera.Models
{
    public class Shift
    {
        public string Email { get; set; }
        public int ShiftType { get; set; }
        public string ShiftName { get; set; }
        public DateTime ShiftDate { get; set; }
        public DayOfWeek DayName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ArrivalTime { get; set; }
        public string LeaveTime { get; set; }
        public List<Malfunction> Malfunctions_line1 { get; set; }
        public List<Malfunction> Malfunctions_line3 { get; set; }
        public List<string> Notes { get; set; }


        public Shift(string email, int shiftType, string shiftName, DateTime shiftDate,
            List<Malfunction> malf1, List<Malfunction> malf3, List<string> notes)
        {
            Email = email;
            ShiftType = shiftType;
            ShiftName = shiftName;
            ShiftDate = shiftDate;
            Malfunctions_line1 = malf1;
            Malfunctions_line3 = malf3;
            Notes = notes;
        }

        public Shift(string email, int shiftType, DateTime shiftDate,
            List<Malfunction> malf1, List<Malfunction> malf3)
        {
            Email = email;
            ShiftType = shiftType;
            ShiftDate = shiftDate;
            Malfunctions_line1 = malf1;
            Malfunctions_line3 = malf3;
        }


        public Shift(string email, int shiftType)
        {
            Email = email;
            ShiftType = shiftType;
        }

        public Shift(string email, int shiftType, DateTime shiftDate)
        {
            Email = email;
            ShiftType = shiftType;
            ShiftDate = shiftDate;
        }

        public Shift(string email, int shiftType, DateTime shiftDate, DayOfWeek dayName)
        {
            Email = email;
            ShiftType = shiftType;
            ShiftDate = shiftDate;
            DayName = dayName;
        }


        public Shift(int shiftType, string shiftName, string startTime = null, string endTime = null)
        {
            ShiftType = shiftType;
            ShiftName = shiftName;
            StartTime = startTime;
            EndTime = endTime;
        }

        public Shift()
        {

        }

        public Shift insert()
        {
            DBService dbs = new DBService();
            return dbs.insert(this);
        }

        public List<Shift> insertConstraints(List<Shift> shifts, string email)
        {
            DBService dbs = new DBService();
            return dbs.insertConstraints(shifts, email);
        }

        public List<Shift> insertShifts(List<Shift> shifts)
        {
            DBService dbs = new DBService();
            return dbs.insertShifts(shifts);
        }

        public void insertNotes()
        {
            DBService dbs = new DBService();
            dbs.insertNotes(this);
        }

        public void insertMalfunctions()
        {
            DBService dbs = new DBService();
            dbs.insertMalfunctions(this);
        }

        //public void insert(List<int> malf1, List<int> malf3)
        //{
        //    DBService dbs = new DBService();
        //    dbs.insert(this, malf1, malf3);
        //}

        public List<Shift> Read()
        {
            DBService dbs = new DBService();
            return dbs.GetShiftTypes();
        }

        public List<Shift> ReadConstraints(string email)
        {
            DBService dbs = new DBService();
            return dbs.ReadConstraints(email);
        }

        public List<Shift> ReadConstraintsToApprove(string email)
        {
            DBService dbs = new DBService();
            return dbs.ReadConstraintsToApprove(email);
        }

        public List<Shift> ReadShifts(string dep)
        {
            DBService dbs = new DBService();
            return dbs.GetShifts(dep);
        }

        public List<Shift> ReadMyEmployeesShifts(string email)
        {
            DBService dbs = new DBService();
            return dbs.GetMyEmployeesShifts(email);
        }


        public List<Shift> GenerateOptimizedShifts(string email)
        {
            DBService dbs = new DBService();
            return dbs.GenerateOptimizedShifts(email);
        }
    }
}