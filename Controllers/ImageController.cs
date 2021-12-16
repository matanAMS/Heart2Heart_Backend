using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heart2HeartBackend.Models;
using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Heart2HeartBackend.Controllers
{

    public class ImageFromUser
    {
        public string base64string { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }


    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {

        const string BaseURL = "http://proj4.ruppin-tech.co.il/";

        ///////    Images
        //http://localhost:59821/api/Images/Images
        [System.Web.Http.Route("Images")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult Images([FromBody] ImageFromUser img)
        {

            try
            {
                string fullpath = $"{System.Web.HttpContext.Current.Server.MapPath("../../")}/{img.path}/";
                System.IO.Directory.CreateDirectory(fullpath);
                string filePath = $"{fullpath}/{img.name}.jpg";
                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(img.base64string));
                return Ok($"{BaseURL}//Images//{img.name}.jpg");

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }
    }
}
