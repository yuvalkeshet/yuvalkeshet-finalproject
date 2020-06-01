using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
//using System.Web.Mvc;

namespace Dashboard_niar_hadera.Controllers
{
    public class FileController : ApiController
    {
        // GET: File
        [Route("api/fileUpload")]
        public HttpResponseMessage Post()
        {
            List<string> fileLinks = new List<string>();
            var httpContext = HttpContext.Current;
            HttpPostedFile httpPostedFile;
            // Check for any uploaded file  

            if (httpContext.Request.Files.Count > 0)
            {

                if (Convert.ToInt32(httpContext.Request.Form["totalSize"]) > 1073741824)
                    throw new Exception("Total size cannot exceed 1GB");


                //Loop through uploaded files  
                for (int i = 0; i < httpContext.Request.Files.Count; i++)
                {


                    httpPostedFile = httpContext.Request.Files[i];

                    // this is an example of how you can extract addional values from the Ajax call
                    string msgId = httpContext.Request.Form["msgId"];

                    if (httpPostedFile != null)
                    {
                        // Construct file save path  
                        //var fileSavePath = Path.Combine(HostingEnvironment.MapPath(ConfigurationManager.AppSettings["fileUploadFolder"]), httpPostedFile.FileName);
                        string fname = httpPostedFile.FileName;
                        //string ext = httpPostedFile.FileName.Split('.').Last();
                        //string full_name = fname + "." + ext;
                        Directory.CreateDirectory(HostingEnvironment.MapPath("~/Attachments/" + msgId + "/" + i));
                        var fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/Attachments/" + msgId + "/" + i), fname);
                        // Save the uploaded file  
                        httpPostedFile.SaveAs(fileSavePath);
                        fileLinks.Add("Attachments/" + msgId + "/" + i + "/" + fname);
                    }
                }

            }

            // Return status code  
            return Request.CreateResponse(HttpStatusCode.Created, fileLinks);
        }

    }
}