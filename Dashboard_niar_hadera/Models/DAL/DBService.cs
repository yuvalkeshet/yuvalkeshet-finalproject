using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using Dashboard_niar_hadera.Models;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using System.Windows.Forms;
using System.Diagnostics;

namespace Dashboard_niar_hadera.Models.DAL
{
    public class DBService
    {
        public DBService()
        {

        }

        public SqlConnection connect(String conString)
        {
            string cStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
            SqlConnection con = new SqlConnection(cStr);
            con.Open();
            return con;
        }

        public Employee Update(Employee emp)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildUpdateCommand(emp);

                if (emp.IsManager)
                {

                    string cStr2 = "SELECT COUNT(*) from Employees INNER JOIN " +
                        "Managers on Employees.email=Managers.email" +
                        " WHERE Employees.email='" + emp.Email + "'";
                    cmd = new SqlCommand(cStr2, con);

                    int count = (int)cmd.ExecuteScalar();
                    if (count == 0)
                        cStr += "INSERT INTO Managers (email, bonus) VALUES ('" + emp.Email + "'," + 1.0 + ")";
                }
                else
                    cStr += "DELETE FROM Managers WHERE email='" + emp.Email + "'";


                cmd = CreateCommand(cStr, con);
                cmd.ExecuteNonQuery();

                return emp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public User Update(User u)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildUpdateCommand(u);

                //if (emp.IsManager)
                //{

                //    string cStr2 = "SELECT COUNT(*) from Employees INNER JOIN " +
                //        "Managers on Employees.email=Managers.email" +
                //        " WHERE Employees.email='" + emp.Email + "'";
                //    cmd = new SqlCommand(cStr2, con);

                //    int count = (int)cmd.ExecuteScalar();
                //    if (count == 0)
                //        cStr += "INSERT INTO Managers (email, bonus) VALUES ('" + emp.Email + "'," + 1.0 + ")";
                //}
                //else
                //    cStr += "DELETE FROM Managers WHERE email='" + emp.Email + "'";


                cmd = CreateCommand(cStr, con);
                cmd.ExecuteNonQuery();

                return u;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public void UpdatePassword(User u)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildUpdatePasswordCommand(u);
                cmd = CreateCommand(cStr, con);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }


        public void RestorePassword(string emailAddress)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                //String cStr = BuildUpdatePasswordCommand(u);
                //cmd = CreateCommand(cStr, con);
                //cmd.ExecuteNonQuery();

                String cStr = "SELECT password FROM Users WHERE email='" + emailAddress + "'";
                cmd = CreateCommand(cStr, con);
                string password = Convert.ToString(cmd.ExecuteScalar());

                MailMessage mail = new MailMessage("ykeshet95@gmail.com", emailAddress);
                SmtpClient client = new SmtpClient();
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                //client.Credentials = new System.Net.NetworkCredential("ykeshet95@gmail.com", "");
                client.Host = "smtp.gmail.com";
                mail.Subject = "Restore password";
                mail.Body = password;
                //client.EnableSsl = true;
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }
        }

        public List<Department> GetDepartments()
        {

            SqlConnection con = null;
            List<Department> lh = new List<Department>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Departments";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Department dep = new Department(
                        (string)(dr["depName"])
                        );

                    lh.Add(dep);
                }

                return lh;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }


        public List<Position> GetPositionsByDep(Department dep)
        {

            SqlConnection con = null;
            List<Position> lp = new List<Position>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM PositionInDepartment" +
                                        " WHERE depName='" + dep.DepName + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Position pos = new Position(
                        (string)(dr["posName"])
                        );

                    lp.Add(pos);
                }

                return lp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> ReadConstraintsToApprove(string email)
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();

            try
            {
                con = connect("ConnectionString");


                String selectSTR = "select email, shiftType, shiftDate from ManagesEmployee inner join ConstraintShifts on " +
                                            "ManagesEmployee.employee_email = ConstraintShifts.email where manager_email = '" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Shift s = new Shift(
                        (string)(dr["email"]),
                        Convert.ToInt32(dr["shiftType"]),
                        (DateTime)(dr["shiftDate"])

                        );

                    ls.Add(s);
                }

                return ls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> ReadConstraints(string email)
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM ConstraintShifts WHERE email='" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Shift s = new Shift(
                        (string)(dr["email"]),
                        Convert.ToInt32(dr["shiftType"]),
                        (DateTime)(dr["shiftDate"])

                        );

                    ls.Add(s);
                }

                return ls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> GetShifts(string dep)
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "select Shifts.email, shiftType, shiftDate from Shifts inner join Employees on Shifts.email = Employees.email where department = '" + dep + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Shift s = new Shift(
                        (string)(dr["email"]),
                        Convert.ToInt32(dr["shiftType"]),
                        (DateTime)(dr["shiftDate"])

                        );

                    ls.Add(s);
                }

                return ls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> GetMyEmployeesShifts(string email)
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();

            try
            {
                con = connect("ConnectionString");


                String selectSTR = "select email, shiftType, shiftDate from ManagesEmployee inner join Shifts on " +
                "ManagesEmployee.employee_email = Shifts.email where manager_email = '" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Shift s = new Shift(
                        (string)(dr["email"]),
                        Convert.ToInt32(dr["shiftType"]),
                        (DateTime)(dr["shiftDate"])

                        );

                    ls.Add(s);
                }

                return ls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> GenerateOptimizedShifts(string email)
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();
            List<Shift> lc = new List<Shift>();

            List<Shift> newShifts = new List<Shift>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "select Shifts.email, Shifts.shiftType, Shifts.shiftDate " +
                                        "from ManagesEmployee inner join Shifts on " +
                                          "ManagesEmployee.employee_email = Shifts.email " +
                                          "where manager_email = '" + email + "' " +
                                        "order by email, shiftType, shiftDate; " +

                                         "select ConstraintShifts.email, ConstraintShifts.shiftType, ConstraintShifts.shiftDate, ConstraintShifts.dateOfInsertion from ManagesEmployee inner join ConstraintShifts " +
                                         "on ManagesEmployee.employee_email = ConstraintShifts.email " +
                                         "where manager_email = '" + email + "' " +
                                         "order by email, shiftType, shiftDate;";


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(cmd);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                ls = ds.Tables[0].AsEnumerable().Select(dataRow => new Shift
                {
                    Email = dataRow.Field<string>("email"),
                    ShiftType = dataRow.Field<Int16>("shiftType"),
                    ShiftDate = dataRow.Field<DateTime>("shiftDate"),
                    DayName = dataRow.Field<DateTime>("shiftDate").DayOfWeek
                    //ArrivalTime = dataRow.Field<string>("arrivalTime"),
                    //LeaveTime = dataRow.Field<string>("leaveTime")
                }).ToList();

                lc = ds.Tables[1].AsEnumerable().Select(dataRow => new Shift
                {
                    Email = dataRow.Field<string>("email"),
                    ShiftType = dataRow.Field<Int16>("shiftType"),
                    ShiftDate = dataRow.Field<DateTime>("shiftDate"),
                    DayName = dataRow.Field<DateTime>("shiftDate").DayOfWeek

                }).ToList();

                int points;
                double bonus;

                List<String> emails = lc.Select(c => c.Email).Distinct().ToList();
                List<DayOfWeek> days = lc.Select(c => c.DayName).Distinct().ToList();
                List<int> shiftTypes = lc.Select(c => c.ShiftType).Distinct().ToList();

                Dictionary<string, double> baseGrades = new Dictionary<string, double>();
                Dictionary<string, double[,]> grades = new Dictionary<string, double[,]>();
                double[,] gradesMat = new double[3, 7];

                Dictionary<string, double[,]> tmpGrades = new Dictionary<string, double[,]>();

                foreach (string e in emails)
                {
                    points = 0;
                    List<Shift> tempLc = lc.Where(c => c.Email == e).ToList();

                    foreach (Shift c in tempLc)
                    {
                        if (!ls.Exists(s => s.ShiftDate == c.ShiftDate &&
                                                    s.ShiftType == c.ShiftType))
                            points++;
                    }

                    baseGrades.Add(e, points / (double)tempLc.Count);
                }

                foreach (string e in emails)
                {

                    int counter = 0;
                    List<Shift> tempLc = lc.Where(c => c.Email == e).ToList();

                    foreach (DayOfWeek d in days)
                    {
                        int dayNum = (int)d;

                        foreach (Shift c in tempLc)
                        {
                            if (c.DayName == d)
                            {
                                counter++;
                                if (ls.Exists(s => s.ShiftDate == c.ShiftDate
                                                    && s.Email == e) && !ls.Contains(c))
                                    gradesMat[c.ShiftType - 1, dayNum] += 1;
                                else if (!ls.Contains(c))
                                    gradesMat[c.ShiftType - 1, dayNum] += 1.5;
                            }

                        }
                    }

                    for (int i = 0; i < gradesMat.GetLength(0); i++)
                    {
                        for (int j = 0; j < gradesMat.GetLength(1); j++)
                            gradesMat[i, j] /= tempLc.Where(c => (int)c.DayName == j &&
                                                                c.ShiftType == i).ToList().Count;
                    }

                    grades.Add(e, (double[,])gradesMat.Clone());
                    tmpGrades.Add(e, (double[,])gradesMat.Clone());

                    for (int i = 0; i < gradesMat.GetLength(0); i++)
                    {
                        for (int j = 0; j < gradesMat.GetLength(1); j++)
                            gradesMat[i, j] = 0;
                    }


                }

                Dictionary<string, double> tmp = new Dictionary<string, double>();

                foreach (KeyValuePair<string, double> grade in baseGrades)
                {
                    double sum = 0;
                    foreach (KeyValuePair<string, double> otherGrade in baseGrades)
                    {
                        sum += otherGrade.Value;
                    }

                    tmp.Add(grade.Key, grade.Value / sum);
                }

                foreach (KeyValuePair<string, double> grade in tmp)
                    baseGrades[grade.Key] = tmp[grade.Key];




                foreach (KeyValuePair<string, double[,]> entry in grades)
                {
                    double[,] myGrades = entry.Value;


                    for (int i = 0; i < myGrades.GetLength(0); i++)
                    {
                        for (int j = 0; j < myGrades.GetLength(1); j++)
                        {
                            double currGrade = myGrades[i, j];
                            double sum = 0;

                            foreach (KeyValuePair<string, double[,]> otherEntry in grades)
                            {
                                sum += otherEntry.Value[i, j];
                            }

                            tmpGrades[entry.Key][i, j] = (currGrade / sum) * baseGrades[entry.Key];

                        }

                    }

                }

                foreach (KeyValuePair<string, double[,]> entry in tmpGrades)
                {
                    double[,] myGrades = entry.Value;


                    for (int i = 0; i < myGrades.GetLength(0); i++)
                    {
                        for (int j = 0; j < myGrades.GetLength(1); j++)
                            grades[entry.Key][i, j] = tmpGrades[entry.Key][i, j];
                    }

                }

                DateTime today = DateTime.Today;
                int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;

                if (daysUntilSunday == 0)
                    daysUntilSunday = 7;

                DateTime nextWeekSunday = today.AddDays(daysUntilSunday);

                List<Shift> newLc = lc.Where(c => c.ShiftDate >= nextWeekSunday).ToList();

                for (int i = 0; i < newLc.Count; i++)
                {
                    double grade = grades[newLc[i].Email][newLc[i].ShiftType - 1, (int)newLc[i].DayName];

                    foreach (KeyValuePair<string, double[,]> entry in grades)
                    {
                        double[,] currGrades = entry.Value;

                        if (currGrades[newLc[i].ShiftType - 1, (int)newLc[i].DayName] > grade)
                        {
                            newLc[i].Email = entry.Key;
                            grade = currGrades[newLc[i].ShiftType - 1, (int)newLc[i].DayName];
                        }

                    }

                }

                for (int i = 0; i < newLc.Count; i++)
                {
                    List<Shift> temp = newLc.Where(c => c.DayName == newLc[i].DayName
                                    && c.ShiftType == newLc[i].ShiftType
                                    && c.ShiftDate == newLc[i].ShiftDate).ToList();

                    while (temp.Count > 3)
                    {
                        int ind = 0;
                        double minGrade = grades[temp[0].Email][temp[0].ShiftType - 1, (int)temp[0].DayName];
                        string removedEmp = temp[0].Email;

                        foreach (KeyValuePair<string, double[,]> entry in grades)
                        {
                            if (entry.Key == temp[ind].Email)
                            {
                                double[,] currGrades = entry.Value;

                                if (currGrades[temp[ind].ShiftType - 1, (int)temp[ind].DayName] < minGrade)
                                {
                                    removedEmp = entry.Key;
                                    minGrade = currGrades[newLc[i].ShiftType - 1, (int)newLc[i].DayName];
                                }
                            }

                        }

                        temp = temp.Where(c => c.Email != removedEmp).ToList();
                        newLc = newLc.Where(c => c.Email != removedEmp).ToList();
                    }
                }

                foreach (Shift c in newLc)
                    lc.Add(c);

                return lc;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Shift> GetShiftTypes()
        {

            SqlConnection con = null;
            List<Shift> ls = new List<Shift>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM ShiftTypes";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Shift s = new Shift(
                        Convert.ToInt32(dr["shiftType"]),
                        (string)(dr["shiftName"]),
                        (dr["startTime"]).ToString(),
                        (dr["endTime"]).ToString()
                        );

                    ls.Add(s);
                }

                return ls;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Malfunction> GetMalfunctionTypes()
        {

            SqlConnection con = null;
            List<Malfunction> lm = new List<Malfunction>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Malfunctions";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Malfunction m = new Malfunction(
                        Convert.ToInt32(dr["MalfunctionCode"]),
                        (string)(dr["MalfunctionDes"])
                        );

                    lm.Add(m);
                }

                return lm;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }


        public List<Employee> GetEmployees()
        {

            SqlConnection con = null;
            SqlConnection con2 = null;
            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");
                con2 = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Employees";

                //String selectSTR = "select email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +

                //   "gender, isMarried, position, department " +
                //   "from Employees " +
                //   "Except " +
                //   "select Employees.email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +
                //   "gender, isMarried, position, department " +
                //   "from Employees inner join Managers on Employees.email = Managers.email";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Employee emp = new Employee(

                        (string)dr["email"],
                        (string)dr["fname"],
                        (string)dr["lname"],
                        Convert.ToInt32(dr["salary"]),
                        Convert.ToInt32(dr["seniority"]),
                        (string)dr["phoneNo"],
                        (string)dr["address"],
                        Convert.ToDateTime(dr["startDate"]),
                        Convert.ToDateTime(dr["birthDate"]),
                        (string)dr["gender"],
                        (bool)dr["isMarried"],
                        (string)dr["position"],
                        (string)dr["department"]
                        );

                    //con2 = connect("ConnectionString");

                    string cStr = "SELECT COUNT(*) from Employees INNER JOIN " +
                        "Managers on Employees.email=Managers.email" +
                        " WHERE Employees.email='" + emp.Email + "'";
                    cmd.Connection = con2;
                    cmd.CommandText = cStr;

                    int count = (int)cmd.ExecuteScalar(); //con2.Close();
                    if (count > 0)
                        emp.IsManager = true;

                    l_emp.Add(emp);
                }

                return l_emp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }

                if (con2 != null)
                {
                    con2.Close();
                }

            }

        }

        public Employee GetEmployeeByEmail(string email)
        {

            SqlConnection con = null;
            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Employees WHERE email='" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    Employee emp = new Employee(

                        (string)dr["email"],
                        (string)dr["fname"],
                        (string)dr["lname"],
                        Convert.ToInt32(dr["salary"]),
                        Convert.ToInt32(dr["seniority"]),
                        (string)dr["phoneNo"],
                        (string)dr["address"],
                        Convert.ToDateTime(dr["startDate"]),
                        Convert.ToDateTime(dr["birthDate"]),
                        (string)dr["gender"],
                        (bool)dr["isMarried"],
                        (string)dr["position"],
                        (string)dr["department"]
                        );

                    return emp;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public Employee GetEmployeeByEmail(string email, string managerEmail)
        {

            SqlConnection con = null;
            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM ManagesEmployee INNER JOIN Employees ON employee_email=email WHERE employee_email='"
                    + email + "' AND manager_email='" + managerEmail + "'; ";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    Employee emp = new Employee(

                        (string)dr["email"],
                        (string)dr["fname"],
                        (string)dr["lname"],
                        Convert.ToInt32(dr["salary"]),
                        Convert.ToInt32(dr["seniority"]),
                        (string)dr["phoneNo"],
                        (string)dr["address"],
                        Convert.ToDateTime(dr["startDate"]),
                        Convert.ToDateTime(dr["birthDate"]),
                        (string)dr["gender"],
                        (bool)dr["isMarried"],
                        (string)dr["position"],
                        (string)dr["department"]
                        );

                    return emp;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Employee> GetRegisteredEmployees()
        {

            SqlConnection con = null;

            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");


                String selectSTR = "SELECT * FROM Employees INNER JOIN Users ON Employees.email=Users.email WHERE isActive='True'";

                //String selectSTR = "select email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +

                //   "gender, isMarried, position, department " +
                //   "from Employees " +
                //   "Except " +
                //   "select Employees.email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +
                //   "gender, isMarried, position, department " +
                //   "from Employees inner join Managers on Employees.email = Managers.email";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Employee emp = new Employee(

                        (string)dr["email"],
                        (string)dr["fname"],
                        (string)dr["lname"],
                        Convert.ToInt32(dr["salary"]),
                        Convert.ToInt32(dr["seniority"]),
                        (string)dr["phoneNo"],
                        (string)dr["address"],
                        Convert.ToDateTime(dr["startDate"]),
                        Convert.ToDateTime(dr["birthDate"]),
                        (string)dr["gender"],
                        (bool)dr["isMarried"],
                        (string)dr["position"],
                        (string)dr["department"]
                        );

                    //con2 = connect("ConnectionString");



                    l_emp.Add(emp);
                }

                return l_emp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public bool CheckIndirectRelation(string email, string managerEmail)
        {

            SqlConnection con = null;
            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "select f1.manager_email, f2.employee_email as indirect_rel" +
                    " from ManagesEmployee f1 join ManagesEmployee f2" +
                    " on f2.manager_email = f1.employee_email" +
                    " where f2.employee_email = '" + email + "' and f1.manager_email = '" + managerEmail + "'; ";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                    return true;


                return false;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<Employee> GetEmployees(string managerEmail)
        {

            SqlConnection con = null;
            SqlConnection con2 = null;
            List<Employee> l_emp = new List<Employee>();

            try
            {
                con = connect("ConnectionString");
                con2 = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Employees INNER JOIN ManagesEmployee" +
                    " ON email=employee_email WHERE manager_email='" + managerEmail + "' ;";

                //String selectSTR = "select email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +

                //   "gender, isMarried, position, department " +
                //   "from Employees " +
                //   "Except " +
                //   "select Employees.email, fname, lname, salary, seniority, phoneNo, address, startDate, birthDate, " +
                //   "gender, isMarried, position, department " +
                //   "from Employees inner join Managers on Employees.email = Managers.email";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Employee emp = new Employee(

                        (string)dr["email"],
                        (string)dr["fname"],
                        (string)dr["lname"],
                        Convert.ToInt32(dr["salary"]),
                        Convert.ToInt32(dr["seniority"]),
                        (string)dr["phoneNo"],
                        (string)dr["address"],
                        Convert.ToDateTime(dr["startDate"]),
                        Convert.ToDateTime(dr["birthDate"]),
                        (string)dr["gender"],
                        (bool)dr["isMarried"],
                        (string)dr["position"],
                        (string)dr["department"]
                        );

                    //con2 = connect("ConnectionString");

                    string cStr = "SELECT COUNT(*) from Employees INNER JOIN " +
                        "Managers on Employees.email=Managers.email" +
                        " WHERE Employees.email='" + emp.Email + "'";
                    cmd = new SqlCommand(cStr, con2);

                    int count = (int)cmd.ExecuteScalar(); //con2.Close();
                    if (count > 0)
                        emp.IsManager = true;

                    l_emp.Add(emp);
                }

                return l_emp;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }
                if (con2 != null)
                {
                    con2.Close();
                }

            }

        }

        public List<User> GetUsers()
        {

            SqlConnection con = null;
            List<User> users = new List<User>();
            DateTime? noDate = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Users";



                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {

                    //if(Convert.IsDBNull(dr["lastLogin"]))
                    User u = new User(

                        (string)dr["email"],
                        (string)dr["password"],
                        (bool)dr["isActive"],
                        Convert.ToDateTime(dr["regDate"]),
                        Convert.IsDBNull(dr["lastLogin"]) ? noDate : Convert.ToDateTime(dr["lastLogin"])
                        );


                    users.Add(u);
                }

                return users;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<User> GetUsers(string managerEmail)
        {

            SqlConnection con = null;
            List<User> users = new List<User>();
            DateTime? noDate = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Users INNER JOIN ManagesEmployee ON" +
                    " email=employee_email WHERE manager_email='" + managerEmail + "'; ";



                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {

                    //if(Convert.IsDBNull(dr["lastLogin"]))
                    User u = new User(

                        (string)dr["email"],
                        (string)dr["password"],
                        (bool)dr["isActive"],
                        Convert.ToDateTime(dr["regDate"]),
                        Convert.IsDBNull(dr["lastLogin"]) ? noDate : Convert.ToDateTime(dr["lastLogin"])
                        );


                    users.Add(u);
                }

                return users;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<User> GetUsersByDep(string depName)
        {

            SqlConnection con = null;
            List<User> users = new List<User>();
            DateTime? noDate = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Users INNER JOIN Employees ON" +
                    " Users.email=Employees.email WHERE department='" + depName + "'; ";



                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {

                    //if(Convert.IsDBNull(dr["lastLogin"]))
                    User u = new User(

                        (string)dr["email"],
                        (string)dr["password"],
                        (bool)dr["isActive"],
                        Convert.ToDateTime(dr["regDate"]),
                        Convert.IsDBNull(dr["lastLogin"]) ? noDate : Convert.ToDateTime(dr["lastLogin"])
                        );


                    users.Add(u);
                }

                return users;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public User GetUserByEmail(string email)
        {

            SqlConnection con = null;
            DateTime? noDate = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Users WHERE email='" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    User u = new User(

                      (string)dr["email"],
                        (string)dr["password"],
                        (bool)dr["isActive"],
                        Convert.ToDateTime(dr["regDate"]),
                        Convert.IsDBNull(dr["lastLogin"]) ? noDate : Convert.ToDateTime(dr["lastLogin"])
                        );

                    return u;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public List<KPI> GetKPIs()
        {

            SqlConnection con = null;
            List<KPI> kpis = new List<KPI>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM KPI_per_day order by measure_time desc";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    float f = float.Parse(dr["output_line_1"].ToString());
                    KPI kpi = new KPI(

                        Convert.ToDateTime(dr["measure_time"]),
                        float.Parse(dr["output_line_1"].ToString()),
                        float.Parse(dr["output_line_3"].ToString()),
                        float.Parse(dr["output_machine_2"].ToString()),
                        float.Parse(dr["output_machine_8"].ToString()),
                        float.Parse(dr["palper_concentration_1"].ToString()),
                        float.Parse(dr["palper_concentration_3"].ToString()),
                        float.Parse(dr["target_output_line_1"].ToString()),
                        float.Parse(dr["target_output_line_3"].ToString()),
                        float.Parse(dr["target_palper_concentration_1"].ToString()),
                        float.Parse(dr["target_palper_concentration_3"].ToString())
                        );


                    kpis.Add(kpi);
                }

                return kpis;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }


            }

        }


        public List<KPI> GetKPIsPerHour()
        {

            SqlConnection con = null;
            List<KPI> kpis = new List<KPI>();

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM KPI_per_hour";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    float f = float.Parse(dr["output_line_1"].ToString());
                    //DateTime timeStamp = DateTime.ParseExact(((TimeSpan)dr["measure_time"]).ToString(@"hh\:mm\:ss\.fffffff"),
                    //    @"hh\:mm\:ss\.fffffff", null, System.Globalization.DateTimeStyles.None);

                    //DateTime today = DateTime.Now;

                    DateTime d;

                    if (DateTime.TryParse(dr["measure_time"].ToString(), out d))
                    {
                        d = Convert.ToDateTime(d.ToString("MM-dd-yyyy HH:mm:ss"));
                    }

                    KPI kpi = new KPI(

                        Convert.ToDateTime(dr["measure_time"].ToString()),
                        float.Parse(dr["output_line_1"].ToString()),
                        float.Parse(dr["output_line_3"].ToString()),
                        float.Parse(dr["output_machine_2"].ToString()),
                        float.Parse(dr["output_machine_8"].ToString()),
                        float.Parse(dr["palper_concentration_1"].ToString()),
                        float.Parse(dr["palper_concentration_3"].ToString()),
                        float.Parse(dr["target_output_line_1"].ToString()),
                        float.Parse(dr["target_output_line_3"].ToString()),
                        float.Parse(dr["target_palper_concentration_1"].ToString()),
                        float.Parse(dr["target_palper_concentration_3"].ToString())
                        );


                    kpis.Add(kpi);
                }

                return kpis;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }


            }

        }

        public KPI GetLatestKPI()
        {

            SqlConnection con = null;
            KPI kpi = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT TOP 1 * FROM KPI ORDER BY measure_time DESC";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    float f = float.Parse(dr["output_line_1"].ToString());
                    //DateTime timeStamp = DateTime.ParseExact(((TimeSpan)dr["measure_time"]).ToString(@"hh\:mm\:ss\.fffffff"),
                    //    @"hh\:mm\:ss\.fffffff", null, System.Globalization.DateTimeStyles.None);

                    DateTime d;

                    if (DateTime.TryParse(dr["measure_time"].ToString(), out d))
                    {
                        d = Convert.ToDateTime(d.ToString("MM-dd-yyyy HH:mm:ss"));
                    }

                    kpi = new KPI(

                        Convert.ToDateTime(dr["measure_time"].ToString()),
                        float.Parse(dr["output_line_1"].ToString()),
                        float.Parse(dr["output_line_3"].ToString()),
                        float.Parse(dr["output_machine_2"].ToString()),
                        float.Parse(dr["output_machine_8"].ToString()),
                        float.Parse(dr["palper_concentration_1"].ToString()),
                        float.Parse(dr["palper_concentration_3"].ToString()),
                        float.Parse(dr["target_output_line_1"].ToString()),
                        float.Parse(dr["target_output_line_3"].ToString()),
                        float.Parse(dr["target_palper_concentration_1"].ToString()),
                        float.Parse(dr["target_palper_concentration_3"].ToString())
                        );

                }

                return kpi;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }


            }

        }

        public string Delete(string email)
        {

            SqlConnection con = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "DELETE FROM Employees WHERE email='" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                cmd.ExecuteNonQuery();

                return email;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public string Delete(string email, string managerEmail)
        {

            SqlConnection con = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "DELETE FROM ManagesEmployee WHERE employee_email='"
                    + email + "' AND manager_email='" + managerEmail + "'; ";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                cmd.ExecuteNonQuery();

                return email;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        public string DeleteUser(string email)
        {

            SqlConnection con = null;

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "DELETE FROM Users WHERE email='" + email + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                cmd.ExecuteNonQuery();

                return email;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }

        private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
        {

            SqlCommand cmd = new SqlCommand();

            cmd.Connection = con;

            cmd.CommandText = CommandSTR;

            cmd.CommandTimeout = 10;

            cmd.CommandType = System.Data.CommandType.Text;

            return cmd;
        }

        public Shift insert(Shift s)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //if (GetEmployeeByEmail(u.Email) == null)
            //    throw (new Exception("No employee is registered" +
            //        " with this email. Please check the list" +
            //        " of registered employees"));
            //if (GetUserByEmail(u.Email) != null)
            //    throw (new Exception("There is already a user registered " +
            //        "with this email"));

            String cStr = BuildInsertCommand(s);

            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             

                s.ShiftDate = DateTime.Now;

                return s;
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new Exception("You already posted this shift");
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public List<Shift> insertConstraints(List<Shift> shifts, string email)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //if (GetEmployeeByEmail(u.Email) == null)
            //    throw (new Exception("No employee is registered" +
            //        " with this email. Please check the list" +
            //        " of registered employees"));
            //if (GetUserByEmail(u.Email) != null)
            //    throw (new Exception("There is already a user registered " +
            //        "with this email"));


            String cStr = BuildInsertConstraintsCommand(shifts, email);

            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             



                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new Exception("You posted the same shift twice");
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public List<Shift> insertShifts(List<Shift> shifts)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            //if (GetEmployeeByEmail(u.Email) == null)
            //    throw (new Exception("No employee is registered" +
            //        " with this email. Please check the list" +
            //        " of registered employees"));
            //if (GetUserByEmail(u.Email) != null)
            //    throw (new Exception("There is already a user registered " +
            //        "with this email"));

            String cStr = BuildInsertShiftsCommand(shifts);

            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             



                return shifts;
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                throw new Exception("You assigned an employee to the same shift twice");
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public void insertNotes(Shift s)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }



            String cStr = BuildNotesInsertCommand(s);

            if (cStr == string.Empty)
                return;

            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 547)
            {
                deleteSummary(s);
                throw new Exception("You accidently posted the same note twice. Check your notes list and remove the duplicated notes");
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }



        public void insertMalfunctions(Shift s)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }



            String cStr = BuildMalfunctionsInsertCommand(s);

            if (cStr == string.Empty)
                return;


            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             
            }

            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public void deleteSummary(Shift s)
        {
            SqlConnection con;
            SqlCommand cmd;
            //string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }


            String cStr = "delete from ShiftSummary where email='" + s.Email + "' and ShiftType=" +
                        s.ShiftType + " and ShiftDate='" + s.ShiftDate + "'";

            cmd = CreateCommand(cStr, con);


            try
            {
                cmd.ExecuteNonQuery(); // execute the command             
            }

            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }


        }

        public string insert(Employee emp)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = BuildInsertCommand(emp);


            //if (emp.IsManager)
            //    cStr += "INSERT INTO Managers (email) VALUES ('" + emp.Email + "')";

            cmd = CreateCommand(cStr, con);


            try
            {
                if (GetEmployeeByEmail(emp.Email) != null)
                    throw (new Exception("Employee with this email already exists"));

                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command

                //String cStr2 = BuildInsertCommand(personId, person);
                //cmd = CreateCommand(cStr2, con);
                //cmd.ExecuteNonQuery();


                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public string insert(Employee emp, string managerEmail)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = "";// = BuildInsertCommand(emp);


            //if (emp.IsManager)
            //    cStr += "INSERT INTO Managers (email) VALUES ('" + emp.Email + "')";

            //cmd = CreateCommand(cStr, con);


            try
            {
                if (GetEmployeeByEmail(emp.Email, managerEmail) != null)
                    throw (new Exception("Employee with this email already exists"));
                if (GetEmployeeByEmail(managerEmail, emp.Email) != null)
                    throw (new Exception("The employee you are trying to set is directly in charge of you. You cannot manage him."));
                if (CheckIndirectRelation(managerEmail, emp.Email) == true)
                    throw (new Exception("The employee you are trying to set is indirectly in charge of you. You cannot manage him."));
                if (GetEmployeeByEmail(emp.Email) == null)
                    cStr = BuildInsertCommand(emp);
                else
                {
                    if (MessageBox.Show("Employee's data is already saved on server.\n\nOverride data?\n\n(Note: The data apears to other managers as well)", "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        cStr = BuildUpdateCommand(emp);
                    }
                }
                //MessageBox.Show("Employee's data is already saved on server.\n\nOverride data?");//Saved data will be used to prevent conflicts.
                cStr += "INSERT INTO ManagesEmployee (manager_email, employee_email) VALUES ('"
                    + managerEmail + "', '" + emp.Email + "'); ";

                cmd = CreateCommand(cStr, con);

                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command

                //String cStr2 = BuildInsertCommand(personId, person);
                //cmd = CreateCommand(cStr2, con);
                //cmd.ExecuteNonQuery();


                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        public string insert(Manager manager)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = BuildInsertCommand(manager);

            cmd = CreateCommand(cStr, con);


            try
            {
                if (GetEmployeeByEmail(manager.Email) != null)
                    throw (new Exception("Employee with this email already exists"));

                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command

                //String cStr2 = BuildInsertCommand(personId, person);
                //cmd = CreateCommand(cStr2, con);
                //cmd.ExecuteNonQuery();


                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public string insert(Manager manager, string managerEmail)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = "";// = BuildInsertCommand(emp);


            //if (emp.IsManager)
            //    cStr += "INSERT INTO Managers (email) VALUES ('" + emp.Email + "')";

            //cmd = CreateCommand(cStr, con);


            try
            {
                if (GetEmployeeByEmail(manager.Email, managerEmail) != null)
                    throw (new Exception("Employee with this email already exists"));
                if (GetEmployeeByEmail(manager.Email) == null)
                    cStr = BuildInsertCommand(manager);

                cStr += "INSERT INTO ManagesEmployee (manager_email, employee_email) VALUES ('"
                    + managerEmail + "', '" + manager.Email + "'); ";

                cmd = CreateCommand(cStr, con);

                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command

                //String cStr2 = BuildInsertCommand(personId, person);
                //cmd = CreateCommand(cStr2, con);
                //cmd.ExecuteNonQuery();


                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public string insert(User u)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (GetEmployeeByEmail(u.Email) == null)
                throw (new Exception("No employee is registered" +
                    " with this email. Please check the list" +
                    " of registered employees"));
            if (GetUserByEmail(u.Email) != null)
                throw (new Exception("There is already a user registered " +
                    "with this email"));

            String cStr = BuildInsertCommand(u);

            cmd = CreateCommand(cStr, con);


            try
            {
                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command             

                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public string insert(User u, string managerEmail)
        {
            SqlConnection con;
            SqlCommand cmd;
            string email = null;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            if (GetEmployeeByEmail(u.Email, managerEmail) == null)
                throw (new Exception("No employee is registered" +
                    " with this email. Please check the list" +
                    " of registered employees"));
            if (GetUserByEmail(u.Email) != null)
                throw (new Exception("There is already a user registered " +
                    "with this email"));

            String cStr = BuildInsertCommand(u);

            cmd = CreateCommand(cStr, con);


            try
            {
                email = Convert.ToString(cmd.ExecuteScalar()); // execute the command             

                return email;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public KPI insert(KPI kpi)
        {
            SqlConnection con;
            SqlCommand cmd;


            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = BuildInsertCommand(kpi);


            //if (emp.IsManager)
            //    cStr += "INSERT INTO Managers (email) VALUES ('" + emp.Email + "')";

            cmd = CreateCommand(cStr, con);


            try
            {

                cmd.ExecuteNonQuery();

                return kpi;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        public User Login(string login_details)
        {
            SqlConnection con = null;
            dynamic data = JObject.Parse(login_details);

            try
            {
                con = connect("ConnectionString");

                String selectSTR = "SELECT * FROM Users " +
                    "WHERE email='" + data.Email + "' AND password='" + data.Password + "'";

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                if (dr.Read())
                {
                    DateTime lastLogin = DateTime.Now;
                    User u = new User(

                       (string)dr["email"],
                       (string)dr["password"],
                       (bool)dr["isActive"],
                       Convert.ToDateTime(dr["regDate"]),
                       lastLogin,
                       dr.IsDBNull(5) ? "" : (string)dr["img"]
                   );

                    dr.Close();

                    con = connect("ConnectionString");

                    selectSTR = "UPDATE Users SET lastLogin='" + lastLogin + "' WHERE email='" + data.Email + "'";

                    cmd.CommandText = selectSTR;
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();

                    selectSTR = "SELECT * FROM Users INNER JOIN Managers ON " +
                        "Users.email=Managers.email WHERE Users.email='" + data.Email + "'";

                    cmd.CommandText = selectSTR;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    if (dr.Read())
                        u.isManager = true;

                    return u;

                }



                throw (new Exception("Mail or Password are incorrect"));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                }

            }

        }



        public Message sendMessage(Message msg)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                String cStr = BuildInsertCommand(msg);
                cmd = CreateCommand(cStr, con);

                int msgId = Convert.ToInt32(cmd.ExecuteScalar());

                cStr = BuildInsertCommand(msg, msgId);
                cmd = CreateCommand(cStr, con);

                cmd.ExecuteNonQuery(); // execute the command             

                msg.MessageCode = msgId;

                return msg;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public Message sendMessageToAll(Message msg)
        {
            SqlConnection con;
            SqlCommand cmd;
            List<User> users = new List<User>();

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                users = GetUsers();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            try
            {
                String cStr = BuildInsertCommand(msg);
                cmd = CreateCommand(cStr, con);

                int msgId = Convert.ToInt32(cmd.ExecuteScalar());

                cStr = BuildInsertCommand(msg, users, msgId);
                cmd = CreateCommand(cStr, con);

                cmd.ExecuteNonQuery(); // execute the command             

                msg.MessageCode = msgId;


                return msg;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public Message sendMessageToMyEmployees(Message msg)
        {
            SqlConnection con;
            SqlCommand cmd;
            List<User> users = new List<User>();

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                users = GetUsers(msg.FromUser);
                if (users.Count == 0)
                    throw new Exception("You have no employees to send the message to");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildInsertCommand(msg);
                cmd = CreateCommand(cStr, con);

                int msgId = Convert.ToInt32(cmd.ExecuteScalar());

                cStr = BuildInsertCommand(msg, users, msgId);
                cmd = CreateCommand(cStr, con);

                cmd.ExecuteNonQuery(); // execute the command             

                msg.MessageCode = msgId;


                return msg;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }

        public Message sendMessageToDep(Message msg, string depName)
        {
            SqlConnection con;
            SqlCommand cmd;
            List<User> users = new List<User>();

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                users = GetUsersByDep(depName);
                if (users.Count == 0)
                    throw new Exception("There are no employees assigned to this department yet");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildInsertCommand(msg);
                cmd = CreateCommand(cStr, con);

                int msgId = Convert.ToInt32(cmd.ExecuteScalar());

                cStr = BuildInsertCommand(msg, users, msgId);
                cmd = CreateCommand(cStr, con);

                cmd.ExecuteNonQuery(); // execute the command             

                msg.MessageCode = msgId;


                return msg;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }




        public List<Message> getMessages(string email, bool receivedOrSent)
        {

            SqlConnection con = null;
            //SqlConnection con2 = null;
            //SqlConnection con2 = null;
            List<Message> l_msg = new List<Message>();
            List<string> files = new List<string>();

            try
            {
                con = connect("ConnectionString");
                //con2 = connect("ConnectionString");


                String selectSTR = "";
                if (receivedOrSent == true)
                {//recieved
                    //selectSTR = "select Messages.messageCode, fromUser, toUser, title, content, messageDate, isRead, isImportant from Messages" +
                    //    " inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode WHERE toUser='" + email + "' order by messageDate desc; ";

                    selectSTR = "select Messages.messageCode, fromUser, toUser, title, content, messageDate, isRead, isImportant from Messages inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode " +
                                    "where UserSendMessage.toUser='" + email + "' " +

                                    "except" +

                                " select Messages.messageCode, UserSendMessage.fromUser, UserSendMessage.toUser, title, content, messageDate, isRead, isImportant from Messages inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode " +
                                "inner join MessageTrash on Messages.messageCode = MessageTrash.messageCode and UserSendMessage.toUser = MessageTrash.toUser" +

                                " where UserSendMessage.toUser ='" + email + "' and isByReciever='True' order by messageDate desc; ";
                }
                else
                {
                    //selectSTR = "select Messages.messageCode, fromUser, toUser, title, content, messageDate, isRead, isImportant from Messages" +
                    //    " inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode WHERE fromUser='" + email + "' order by messageDate desc; ";
                    selectSTR = "select Messages.messageCode, fromUser, toUser, title, content, messageDate, isRead, isImportant from Messages inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode " +
                                   "where UserSendMessage.fromUser='" + email + "' " +

                                   "except" +

                               " select  Messages.messageCode, UserSendMessage.fromUser, UserSendMessage.toUser, title, content, messageDate, isRead, isImportant from Messages inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode " +
                               "inner join MessageTrash on Messages.messageCode = MessageTrash.messageCode and UserSendMessage.toUser = MessageTrash.toUser" +

                               " where UserSendMessage.fromUser ='" + email + "' and isBySender='True' order by messageDate desc; ";
                }
                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Message msg = new Message(

                        Convert.ToInt32(dr["messageCode"]),
                        (string)dr["fromUser"],
                        (string)dr["toUser"],
                        Convert.ToDateTime(dr["messageDate"]),
                        (string)(dr["title"]),
                        (string)dr["content"],
                        null,
                        (bool)dr["isRead"],
                        (bool)dr["isImportant"]
                        );

                    //con2 = connect("ConnectionString");

                    //string cStr = "SELECT COUNT(*) from Employees INNER JOIN " +
                    //    "Managers on Employees.email=Managers.email" +
                    //    " WHERE Employees.email='" + emp.Email + "'";
                    //cmd = new SqlCommand(cStr, con2);

                    //int count = (int)cmd.ExecuteScalar(); //con2.Close();
                    //if (count > 0)
                    //    emp.IsManager = true;

                    l_msg.Add(msg);
                }

                dr.Close();

                //foreach (Message m in l_msg)
                //{


                //    int msgId = m.MessageCode;

                //    selectSTR = "select filePath from MessageAttachments where messageCode=" + msgId;

                //    cmd = new SqlCommand(selectSTR, con);

                //    dr = cmd.ExecuteReader();

                //    while (dr.Read())
                //        files.Add((string)dr["filePath"]);

                //    m.Files = files.ToArray();

                //    files.Clear();
                //}



                for (var i = 0; i < l_msg.Count; i++)
                {

                    con = connect("ConnectionString");

                    int msgId = l_msg[i].MessageCode;

                    selectSTR = "select filePath from MessageAttachments where messageCode=" + msgId;

                    cmd.Connection = con;
                    cmd.CommandText = selectSTR;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                        files.Add((string)dr["filePath"]);

                    dr.Close();

                    l_msg[i].Files = files.ToArray();

                    files.Clear();
                }

                return l_msg;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    //con2.Close();
                }
                //if (con2 != null)
                //{
                //    con2.Close();
                //    //con2.Close();
                //}

            }

        }


        public List<Message> getMessagesFromTrash(string email)
        {

            SqlConnection con = null;

            List<Message> l_msg = new List<Message>();
            List<string> files = new List<string>();

            try
            {
                con = connect("ConnectionString");



                String selectSTR = "";



                selectSTR = "select Messages.messageCode, UserSendMessage.fromUser, UserSendMessage.toUser, title, content, messageDate, isRead, isImportant from Messages inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode " +
                            "inner join MessageTrash on Messages.messageCode = MessageTrash.messageCode and UserSendMessage.toUser = MessageTrash.toUser" +

                            " where UserSendMessage.toUser ='" + email + "' and isByReciever='True' or UserSendMessage.fromUser ='" + email + "' and isBySender='True' order by messageDate desc; ";


                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    Message msg = new Message(

                        Convert.ToInt32(dr["messageCode"]),
                        (string)dr["fromUser"],
                        (string)dr["toUser"],
                        Convert.ToDateTime(dr["messageDate"]),
                        (string)(dr["title"]),
                        (string)dr["content"],
                        null,
                        (bool)dr["isRead"],
                        (bool)dr["isImportant"]
                        );



                    l_msg.Add(msg);
                }

                dr.Close();





                for (var i = 0; i < l_msg.Count; i++)
                {

                    con = connect("ConnectionString");

                    int msgId = l_msg[i].MessageCode;

                    selectSTR = "select filePath from MessageAttachments where messageCode=" + msgId;

                    cmd.Connection = con;
                    cmd.CommandText = selectSTR;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                        files.Add((string)dr["filePath"]);

                    dr.Close();

                    l_msg[i].Files = files.ToArray();

                    files.Clear();
                }

                return l_msg;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();

                }


            }

        }

        public Message getMessage(string email, int msgCode)
        {

            SqlConnection con = null;
            //SqlConnection con2 = null;
            //SqlConnection con2 = null;
            //List<Message> l_msg = new List<Message>();
            Message msg;
            List<string> files = new List<string>();

            try
            {
                con = connect("ConnectionString");
                //con2 = connect("ConnectionString");




                String selectSTR = "select Messages.messageCode, fromUser, toUser, title, content, messageDate, isRead, isImportant from Messages" +
                    " inner join UserSendMessage on Messages.messageCode = UserSendMessage.messageCode WHERE toUser='" + email + "' AND Messages.messageCode=" + msgCode;

                SqlCommand cmd = new SqlCommand(selectSTR, con);

                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                if (dr.Read())
                {
                    msg = new Message(

                        Convert.ToInt32(dr["messageCode"]),
                        (string)dr["fromUser"],
                        (string)dr["toUser"],
                        Convert.ToDateTime(dr["messageDate"]),
                        (string)(dr["title"]),
                        (string)dr["content"],
                        null,
                        (bool)dr["isRead"],
                        (bool)dr["isImportant"]
                        );
                }
                else
                    throw new Exception("Message not found.");


                con = connect("ConnectionString");

                int msgId = msg.MessageCode;

                selectSTR = "select filePath from MessageAttachments where messageCode=" + msgId;

                cmd.Connection = con;
                cmd.CommandText = selectSTR;

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                    files.Add((string)dr["filePath"]);

                msg.Files = files.ToArray();



                return msg;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    //con2.Close();
                }
                //if (con2 != null)
                //{
                //    con2.Close();
                //    //con2.Close();
                //}

            }

        }

        public int setImp(string email, int msgCode, bool isImp)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildUpdateCommand(email, msgCode, isImp);


                cmd = CreateCommand(cStr, con);
                cmd.ExecuteNonQuery();

                return msgCode;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        public int setRead(string email, int msgCode, bool isRead)
        {
            SqlConnection con;
            SqlCommand cmd;

            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            try
            {
                String cStr = BuildUpdateCommand(email, msgCode, isRead);


                cmd = CreateCommand(cStr, con);
                cmd.ExecuteNonQuery();

                return msgCode;
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }


        public Message deleteMessage(Message msg, bool byReciever, bool bySender)
        {
            SqlConnection con;
            SqlCommand cmd;


            try
            {
                con = connect("ConnectionString");
            }
            catch (Exception ex)
            {
                throw (ex);
            }

            String cStr = BuildInsertCommand(msg, byReciever, bySender);



            cmd = CreateCommand(cStr, con);


            try
            {


                cmd.ExecuteNonQuery(); // execute the command



                return msg;
            }
            catch (Exception ex)
            {
                //if (email == null)
                //    throw (new Exception("Error occured while trying to insert an employee"));

                throw (ex);
            }

            finally
            {
                if (con != null)
                {
                    con.Close();
                }
            }

        }





        private String BuildInsertCommand(Employee emp)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", emp.Email,
                emp.Fname, emp.Lname, emp.Salary.ToString(), emp.Seniority.ToString(),
                emp.PhoneNo, emp.Address, emp.BirthDate.ToShortDateString(),
                    emp.Gender, emp.IsMarried == true ? 1 : 0, emp.Position, emp.Department);
            String prefix = "INSERT INTO Employees (email, fname, lname, salary, seniority, phoneNo, address, birthDate, gender, isMarried, position, department) ";

            command = prefix + sb.ToString() + "; ";


            return command;
        }

        private String BuildInsertCommand(Manager manager)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', '{2}', {3}, {4}, '{5}', '{6}', '{7}', '{8}', {9}, '{10}', '{11}')", manager.Email,
                manager.Fname, manager.Lname, manager.Salary.ToString(), manager.Seniority.ToString(),
                manager.PhoneNo, manager.Address, manager.BirthDate.ToShortDateString(),
                    manager.Gender, manager.IsMarried == true ? 1 : 0, manager.Position, manager.Department);
            String prefix = "INSERT INTO Employees (email, fname, lname, salary, seniority, phoneNo, address, birthDate, gender, isMarried, position, department) ";

            command = prefix + sb.ToString() + "; ";

            sb2.AppendFormat("Values('{0}', {1})", manager.Email, manager.Bonus.ToString());
            String prefix2 = "INSERT INTO Managers (email, bonus) ";

            command += prefix2 + sb2.ToString() + "; ";

            return command;
        }

        private String BuildInsertCommand(User u)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}', {2})", u.Email,
                u.Password, u.isActive == true ? 1 : 0);
            String prefix = "INSERT INTO Users (email, password, isActive) ";

            command = prefix + sb.ToString() + "; ";


            return command;
        }

        private String BuildInsertCommand(Message msg)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', '{1}')", msg.Title,
                msg.Content);
            String prefix = "INSERT INTO Messages (title, content) ";

            command = prefix + sb.ToString() + "; SELECT SCOPE_IDENTITY()";


            return command;
        }

        private String BuildInsertCommand(Message msg, int msgId)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values({0}, '{1}', '{2}')", msgId.ToString(),
                msg.FromUser, msg.ToUser);
            String prefix = "INSERT INTO UserSendMessage (messageCode, fromUser, toUser) ";

            command = prefix + sb.ToString() + "; ";

            prefix = "";
            sb.Clear();

            for (int i = 0; i < msg.Files.Length; i++)
            {
                sb.AppendFormat("Values('{0}')", "Attachments/" + msgId + "/" + i + "/" + msg.Files[i]);
                prefix += "INSERT INTO Attachments (filePath) " + sb.ToString() + "; ";
                sb.Clear();

                sb.AppendFormat("Values({0}, '{1}')", msgId.ToString(), "Attachments/" + msgId + "/" + i + "/" + msg.Files[i]);
                prefix += "INSERT INTO MessageAttachments (messageCode, filePath) " + sb.ToString() + "; ";
                sb.Clear();
            }

            command += prefix;

            return command;
        }

        private String BuildInsertCommand(Message msg, List<User> users, int msgId)
        {
            String command = "";

            StringBuilder sb = new StringBuilder();

            //sb.AppendFormat("Values('{0}', '{1}', '{2}', '{3}')", msg.FromUser,
            //    msg.ToUser, msg.Title, msg.Content);
            String prefix = "";

            foreach (var user in users)
            {
                //Console.WriteLine("Amount is {0} and type is {1}", money.amount, money.type);

                if (msg.FromUser == user.Email)
                    continue;

                sb.AppendFormat("Values({0}, '{1}', '{2}')", msgId.ToString(), msg.FromUser, user.Email);

                prefix += "INSERT INTO UserSendMessage (messageCode, fromUser, toUser) " + sb.ToString() + "; ";



                sb.Clear();
            }

            for (int i = 0; i < msg.Files.Length; i++)
            {
                sb.AppendFormat("Values('{0}')", "Attachments/" + msgId + "/" + i + "/" + msg.Files[i]);
                prefix += "INSERT INTO Attachments (filePath) " + sb.ToString() + "; ";
                sb.Clear();

                sb.AppendFormat("Values({0}, '{1}')", msgId.ToString(), "Attachments/" + msgId + "/" + i + "/" + msg.Files[i]);
                prefix += "INSERT INTO MessageAttachments (messageCode, filePath) " + sb.ToString() + "; ";
                sb.Clear();
            }

            command = prefix;

            return command;
        }

        private String BuildInsertCommand(Message msg, bool byReciever, bool bySender)
        {
            String command;
            String prefix;
            StringBuilder sb = new StringBuilder();
            if (byReciever)
            {
                sb.AppendFormat("Values({0}, '{1}', '{2}', {3})", msg.MessageCode.ToString(), msg.FromUser,
                msg.ToUser, 1);
                prefix = "INSERT INTO MessageTrash (messageCode, fromUser, toUser, isByReciever) ";

                command = prefix + sb.ToString() + "; SELECT SCOPE_IDENTITY()";

            }
            else
            {
                sb.AppendFormat("Values({0}, '{1}', '{2}', {3})", msg.MessageCode.ToString(), msg.FromUser,
                msg.ToUser, 1);
                prefix = "INSERT INTO MessageTrash (messageCode, fromUser, toUser, isBySender) ";

                command = prefix + sb.ToString() + "; SELECT SCOPE_IDENTITY()";
            }



            return command;
        }

        private String BuildInsertCommand(KPI kpi)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})", kpi.OutputLine1, kpi.OutputLine3, kpi.OutputMachine2,
                kpi.OutputMachine8, kpi.PalperConcentration1, kpi.PalperConcentration3, kpi.TargetOutputLine1, kpi.TargetOutputLine3,
                kpi.TargetPalperConcentration1, kpi.TargetPalperConcentration3);
            String prefix = "INSERT INTO KPI (output_line_1, output_line_3, output_machine_2, output_machine_8, " +
                "palper_concentration_1, palper_concentration_3, target_output_line_1, target_output_line_3, target_palper_concentration_1, target_palper_concentration_3) ";

            command = prefix + sb.ToString() + "; ";


            return command;
        }


        private String BuildUpdateCommand(Employee emp)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            sb.AppendFormat("fname='{0}', lname='{1}', salary={2}, seniority={3}, phoneNo='{4}', address='{5}', birthDate='{6}', gender='{7}', isMarried={8}, position='{9}', department='{10}'",
                emp.Fname, emp.Lname, emp.Salary.ToString(), emp.Seniority.ToString(),
                emp.PhoneNo, emp.Address, emp.BirthDate.ToShortDateString(),
                    emp.Gender, emp.IsMarried == true ? 1 : 0, emp.Position, emp.Department);
            String prefix = "UPDATE Employees SET ";

            command = prefix + sb.ToString() + " WHERE email='" + emp.Email + "'; ";

            return command;
        }

        private String BuildUpdateCommand(User u)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            sb.AppendFormat("password='{0}', isActive={1}",
                u.Password, u.isActive == true ? 1 : 0);
            String prefix = "UPDATE Users SET ";

            command = prefix + sb.ToString() + " WHERE email='" + u.Email + "'; ";

            return command;
        }

        private String BuildUpdatePasswordCommand(User user)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();

            sb.AppendFormat("Password='{0}'", user.Password);
            String prefix = "UPDATE Users SET ";

            command = prefix + sb.ToString() + " WHERE email='" + user.Email + "'";


            //String prefix2 = "DELETE FROM PersonHobbies WHERE id=" + person.Id + "; ";

            //for (int i = 0; i < person.Hobbies.Length; i++)
            //{
            //    sb2.AppendFormat("Values({0},{1})", person.Id.ToString(), person.Hobbies[i].ToString());
            //    prefix2 += "INSERT INTO PersonHobbies " + "(id, HobbyID) " + sb2.ToString() + "; ";
            //    sb2.Clear();
            //}

            //command += prefix2 + sb2.ToString();

            return command;
        }

        private String BuildUpdateCommand(string email, int msgCode, bool isReadORImp)
        {
            String command;
            String prefix;
            StackTrace stackTrace = new StackTrace();
            string funcCall = stackTrace.GetFrame(1).GetMethod().Name;

            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            if (funcCall == "setImp")
            {
                sb.AppendFormat("isImportant={0}",
                    isReadORImp == true ? 1 : 0);
                prefix = "UPDATE UserSendMessage SET ";
            }
            else
            {
                sb.AppendFormat("isRead={0}",
                    isReadORImp == true ? 1 : 0);
                prefix = "UPDATE UserSendMessage SET ";
            }


            command = prefix + sb.ToString() + " WHERE toUser='" + email + "' AND messageCode=" + msgCode + "; ";

            return command;
        }

        private String BuildInsertCommand(Shift s)
        {
            String command;

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("Values('{0}', {1})", s.Email, s.ShiftType.ToString());
            String prefix = "INSERT INTO ShiftSummary (email, ShiftType) ";

            command = prefix + sb.ToString() + "; ";


            return command;
        }

        private String BuildInsertConstraintsCommand(List<Shift> shifts, string email)
        {
            String command = "";

            StringBuilder sb = new StringBuilder();
            String prefix = "";

            DateTime today = DateTime.Today;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;

            if (daysUntilSunday == 0)
                daysUntilSunday = 7;

            DateTime nextWeekSunday = today.AddDays(daysUntilSunday);

            shifts = shifts.Where(s => s.ShiftDate >= nextWeekSunday && s.ShiftDate <= nextWeekSunday.AddDays(6)).ToList();

            command = "DELETE FROM ConstraintShifts WHERE email = '" + email +
                "' AND shiftDate BETWEEN '" + nextWeekSunday +
                "' AND '" + nextWeekSunday.AddDays(6) + "'; ";

            foreach (Shift s in shifts)
            {
                sb.AppendFormat("Values('{0}', {1}, '{2}')", s.Email, s.ShiftType.ToString(), s.ShiftDate.ToShortDateString());
                prefix = "INSERT INTO ConstraintShifts (email, shiftType, shiftDate) ";
                command += prefix + sb.ToString() + "; ";
                sb.Clear();
            }





            return command;
        }

        private String BuildInsertShiftsCommand(List<Shift> shifts)
        {
            String command;

            StringBuilder sb = new StringBuilder();
            String prefix = "";

            DateTime today = DateTime.Today;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;

            if (daysUntilSunday == 0)
                daysUntilSunday = 7;

            DateTime nextWeekSunday = today.AddDays(daysUntilSunday);

            shifts = shifts.Where(s => s.ShiftDate >= nextWeekSunday && s.ShiftDate <= nextWeekSunday.AddDays(6)).ToList();


            command = "DELETE FROM Shifts WHERE shiftDate BETWEEN '" + nextWeekSunday +
                  "' AND '" + nextWeekSunday.AddDays(6) + "'; ";

            foreach (Shift s in shifts)
            {
                sb.AppendFormat("Values('{0}', {1}, '{2}')", s.Email, s.ShiftType.ToString(), s.ShiftDate.ToShortDateString());
                prefix = "INSERT INTO Shifts (email, shiftType, shiftDate) ";
                command += prefix + sb.ToString() + "; ";
                sb.Clear();
            }


            return command;
        }

        //private String BuildInsertCommand(Shift s, List<string> notes)
        //{
        //    String command;
        //    String prefix = "";
        //    StringBuilder sb = new StringBuilder();

        //    foreach (string note in notes)
        //    {
        //        sb.AppendFormat("Values('{0}', '{1}', {2}, {3})", note, s.Email, s.ShiftType.ToString(), s.ShiftDate.ToShortDateString());
        //        prefix += "INSERT INTO Notes (note, email, ShiftType, ShiftDate) " + sb.ToString() + "; ";
        //        sb.Clear();
        //    }



        //    command = prefix;


        //    return command;
        //}

        //private String BuildInsertCommand(Shift s, List<int> malf1, List<int> malf3)
        //{
        //    String command;
        //    String prefix = "";
        //    StringBuilder sb = new StringBuilder();

        //    foreach (int mal in malf1)
        //    {
        //        sb.AppendFormat("Values('{0}', '{1}', {2}, {3}, {4})", mal, s.Email, s.ShiftType.ToString(), 
        //            s.ShiftDate.ToShortDateString(), 1);
        //        prefix += "INSERT INTO MalfunctionInShift (malfunctionCode, email, ShiftType, ShiftDate, line) " + sb.ToString() + "; ";
        //        sb.Clear();
        //    }
        //    foreach (int mal in malf3)
        //    {
        //        sb.AppendFormat("Values('{0}', '{1}', {2}, {3}, {4})", mal, s.Email, s.ShiftType.ToString(),
        //            s.ShiftDate.ToShortDateString(), 3);
        //        prefix += "INSERT INTO MalfunctionInShift (malfunctionCode, email, ShiftType, ShiftDate, line) " + sb.ToString() + "; ";
        //        sb.Clear();
        //    }



        //    command = prefix;


        //    return command;
        //}

        private String BuildMalfunctionsInsertCommand(Shift s)
        {
            String command;
            String prefix = "";
            StringBuilder sb = new StringBuilder();

            foreach (Malfunction mal in s.Malfunctions_line1)
            {
                sb.AppendFormat("Values({0}, '{1}', {2}, '{3}', {4})", mal.MalfunctionCode.ToString(), s.Email, s.ShiftType.ToString(),
                    s.ShiftDate.ToShortDateString(), 1);
                prefix += "INSERT INTO MalfunctionInShift (malfunctionCode, email, ShiftType, ShiftDate, line) " + sb.ToString() + "; ";
                sb.Clear();
            }
            foreach (Malfunction mal in s.Malfunctions_line3)
            {
                sb.AppendFormat("Values({0}, '{1}', {2}, '{3}', {4})", mal.MalfunctionCode.ToString(), s.Email, s.ShiftType.ToString(),
                    s.ShiftDate.ToShortDateString(), 3);
                prefix += "INSERT INTO MalfunctionInShift (malfunctionCode, email, ShiftType, ShiftDate, line) " + sb.ToString() + "; ";
                sb.Clear();
            }



            command = prefix;


            return command;
        }

        private String BuildNotesInsertCommand(Shift s)
        {
            String command;
            String prefix = "";
            StringBuilder sb = new StringBuilder();

            foreach (string note in s.Notes)
            {
                sb.AppendFormat("Values('{0}', '{1}', {2}, '{3}')", note, s.Email, s.ShiftType.ToString(), s.ShiftDate.ToShortDateString());
                prefix += "INSERT INTO Notes (note, email, ShiftType, ShiftDate) " + sb.ToString() + "; ";
                sb.Clear();
            }



            command = prefix;


            return command;
        }



    }
}