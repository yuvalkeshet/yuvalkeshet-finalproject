using Dashboard_niar_hadera.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;


namespace Dashboard_niar_hadera
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        //string cStr = WebConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BackgroundTasks.StartKPI();
            
            //SqlDependency.Start(cStr);

            //SqlConnection con = new SqlConnection(cStr);
            //SqlCommand sqlCommand = new SqlCommand();
            //sqlCommand.Connection = con;
            //con.Open(); //await sqlCommand.Connection.OpenAsync();           
            //sqlCommand.CommandType = CommandType.Text;
            //sqlCommand.CommandText = "SELECT TOP 1 * FROM KPI ORDER BY measure_time DESC";
            //sqlCommand.Notification = null;

            //SqlDependency sqlDep = new SqlDependency(sqlCommand);
            ////sqlDep.OnChange += ;
            //sqlCommand.ExecuteReader(); //await this.sampleSqlCommand.ExecuteReaderAsync();


        }
    }
}
