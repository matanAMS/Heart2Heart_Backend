using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Heart2HeartBackend.Models;

namespace Heart2HeartBackend.Controllers
{
    public class Messages
    {
        public Messages(int fromUser, int toUser, string date, string message)
        {
            FromUser = fromUser;
            ToUser = toUser;
            Date = date;
            Message = message;
        }

        public int FromUser { get; set; }
        public int ToUser { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
     
  
       
    }
    [EnableCors("*", "*", "*")]
    public class MessagesController : ApiController
    {
        private static UsersDB db = new UsersDB();
        [Route("MessageFrom/{id}")]
        public IHttpActionResult Get([FromBody] int id)
        {
            try
            {
                Messages message = db.GetMessages(id); 
                if (message != null)
                    return Ok(message);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



    }
}
