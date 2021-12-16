using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;
using Heart2HeartBackend.Models;

namespace TripClubWebService.Controllers
{
    [EnableCors("*", "*", "*")]
    public class SystemToolsController : ApiController
    {

        public class NewEmailAndPassword
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string Key { get; set; }
        }






        //http://localhost:59821/KeyComparison?email=
        [Route("KeyComparison")]
        [HttpGet]
        public IHttpActionResult KeyComparison(string email, string key)
        {

            try
            {
                string localKey = "";
                email.ToArray().ToList().ForEach(ch =>
                {
                    if (Char.IsLetterOrDigit((char)(ch ^ 1)))
                        localKey += (char)(ch ^ 1);
                });
                if (key.ToLower() == localKey.ToLower())
                    return Ok();
                return Content(HttpStatusCode.Forbidden, new { message = "Invalid key!", email, key });

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }





        //http://localhost:59821/PasswordResetRequest?email=
        [Route("PasswordResetRequest")]
        [HttpGet]
        public IHttpActionResult PasswordResetRequest(string email)
        {
            try
            {
                string localKey = "";
                email.ToArray().ToList().ForEach(ch =>
                {
                    if (Char.IsLetterOrDigit((char)(ch ^ 1)))
                        localKey += (char)(ch ^ 1);
                });
                localKey = localKey.ToLower();
                var user = UsersDB.GetUserByEmail(email);
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, "Email not found");
                }
                const string resetPassUrl = @"http://proj4.ruppin-tech.co.il/build/";
                const string img = @"https://image.freepik.com/free-vector/open-locker_53876-25497.jpg";

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("matanprogrammer@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Reset Password";
                mail.IsBodyHtml = true;
                string htmlBody;
                htmlBody =
                    $@"<div style='background-color:#555;width:100%;height:100px;flex-direction:row;justify-content:space-around;display:flex;color:#fff;text-align:center;'>
         
    </div>
    <div style ='text-align:center;padding:50px 0 50px 0'>
         <h1>{ user.UserName }   שלום</h1>
              <div style='padding:50px 0 50px 0'>
                    <img src='{img}' style='height:150px;width:150px;display:block;margin:auto;' />
                    <p style='font-size:20px;font-family:sans-serif;font-weight:bold;'>
                        הייתה בקשה לשנות את הסיסמה שלך!<br>
                        אם לא הגשת בקשה זו, פשוט התעלם מדוא'ל זה.<br>
אחרת, אנא לחץ כדי לאפס את הסיסמה שלך על ידי לחיצה                   
                         <a href={ resetPassUrl}> כאן</a>
                        <h3><span style='background-color:#ccc;margin:30px'> {localKey}</span>המפתח שלך לשינוי סיסמה הוא: </h3>
                    </p>
              </div>
      </div>
      <div style ='background-color:#b7b7a4;bottom:0;position:absolute;width:100%;color:#000;text-align:center;'>
           <h3>?יש שאלות או תהיות</h3>
              <p>אנחנו כאן כדי לעזור, צרו קשר כאן  <a href = 'mailto:matanprogrammer@gmail.com'> Heart2Heart ltd </a></p>
                  </div>";
                mail.Body = htmlBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("matanprogrammer@gmail.com", "");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return Ok("Email has been sent successfully");

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }




        //http://localhost:59821/UserForgotPassword
        [Route("UserForgotPassword")]
        public IHttpActionResult Post([FromBody] NewEmailAndPassword obj)
        {
            try
            {
                string localKey = "";
                obj.Email.ToArray().ToList().ForEach(ch =>
                {
                    if (Char.IsLetterOrDigit((char)(ch ^ 1)))
                        localKey += (char)(ch ^ 1);
                });
                if (localKey.ToLower() != obj.Key.ToLower())
                    return Content(HttpStatusCode.Forbidden, "Invalid key!");

                int rowsAffected = UsersDB.UserForgotPassword(obj.Email, obj.Password);

                if (rowsAffected > 0)
                    return Ok(obj.Password);

                return Content(HttpStatusCode.NotFound, "User cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        public class ConnectUsData
        {
            public string name;
            public string phoneNumber;
            public string email;
            public string subject;
        }

        //http://localhost:59821/SendConnectUsEmail
        [Route("SendConnectUsEmail")]
        [HttpPost]
        public IHttpActionResult SendConnectUsEmail([FromBody] ConnectUsData userData)
        {
            try
            {

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("matanprogrammer@gmail.com");
                mail.To.Add("tripclubapp@gmail.com");
                mail.Subject = userData.subject;


                mail.IsBodyHtml = true;
                string htmlBody;


                htmlBody = $"<h2>name : {userData.name}</h2>";
                htmlBody += $"<h2>phoneNumber : {userData.phoneNumber}</h2>";
                htmlBody += $"<h2>userData.email : {userData.email}</h2>";
                mail.Body = htmlBody;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("matanprogrammer@gmail.com", "");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return Ok();

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }




    }
}
