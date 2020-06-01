using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace Dashboard_niar_hadera.Models
{
    public class BackgroundTasks
    {
        private static KPI kpi;
        private static Random random = new Random();



        public static void StartKPI()
        {


            var task = Task.Run(async () =>
               {
                   for (; ; )
                   {
                       await Task.Delay(60000);

                       kpi = new KPI(
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * 0.4),
                           (float)(random.NextDouble() * 0.4),
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * (40 - 20) + 20),
                           (float)(random.NextDouble() * 0.4),
                           (float)(random.NextDouble() * 0.4));


                       HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://proj.ruppin.ac.il/bgroup70/prod/api/kpi");
                       httpWebRequest.ContentType = "application/json";
                       httpWebRequest.Method = "POST";

                       using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                       {
                           string json = "{\"OutputLine1\":\"" + kpi.OutputLine1 + "\"," +
                                         "\"OutputLine3\":\"" + kpi.OutputLine3 + "\"," +
                                         "\"OutputMachine2\":\"" + kpi.OutputMachine2 + "\"," +
                                         "\"OutputMachine8\":\"" + kpi.OutputMachine8 + "\"," +
                                         "\"PalperConcentration1\":\"" + kpi.PalperConcentration1 + "\"," +
                                         "\"PalperConcentration3\":\"" + kpi.PalperConcentration3 + "\"," +
                                         "\"TargetOutputLine1\":\"" + kpi.TargetOutputLine1 + "\"," +
                                         "\"TargetOutputLine3\":\"" + kpi.TargetOutputLine3 + "\"," +
                                         "\"TargetPalperConcentration1\":\"" + kpi.TargetPalperConcentration1 + "\"," +
                                         "\"TargetPalperConcentration3\":\"" + kpi.TargetPalperConcentration3 + "\"}";


                           streamWriter.Write(json);
                           streamWriter.Flush();
                           streamWriter.Close();
                       }

                       var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                       using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                       {
                           var result = streamReader.ReadToEnd();
                       }
                   }
               });
        }
    }
}