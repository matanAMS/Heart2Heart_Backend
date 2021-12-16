using Heart2HeartBackend.Models;
using System;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Heart2HeartBackend.Controllers
{
    [EnableCors("*", "*", "*")]
    public class UsersController : ApiController
    {
        private static UsersDB db=new UsersDB();

        //http://localhost:59821/GetAllUsers
        [Route("GetAllUsers")]
        public IHttpActionResult Get()
        {
            try
            {
                Users[] users = UsersDB.GetAllUsers().ToArray();
                if (users != null)
                    return Ok(users);
                else return Content(HttpStatusCode.NotFound, "Users cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: Users
        [Route("InsertNewUser")]
        public IHttpActionResult Post([FromBody] Users user)
        {
            try
            {
                //chek if email already taken
                Users userToCheck = UsersDB.GetUserByEmail(user.Email);
                if (userToCheck != null)
                    return Content(HttpStatusCode.Conflict, $"User with Email '{user.Email}' already exists.");

                int newCode = UsersDB.InsertNewUser(user);
                user.UserId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetUserById/{user.UserId}"), user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }
        [Route("login")]//כניסת משתמש למערכת
        [HttpPost]
        public IHttpActionResult GetU([FromBody] Users user)
        {
            try
            {
                Object u = db.getUser(user.Email, user.Password);
                if (u == null)
                {
                    return Content(HttpStatusCode.NotFound, false);
                }
                return Ok(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





        [Route("GetUserById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Users user = UsersDB.GetUserById(id);
                if (user != null)
                    return Ok(user);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }




        [Route("UpdateUser")]
        public IHttpActionResult PostU([FromBody] Users user)
        {
            try
            {
                int rowsEffected = UsersDB.UpdateUser(user);
                if (rowsEffected > 0) return Content(HttpStatusCode.OK, user);
                else return Content(HttpStatusCode.NotFound, $"User with id {user.UserId} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:XXXX/DeleteUser/5
        [Route("DeleteUser/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = UsersDB.DeleteUser(id);
            if (val > 0) return Ok($"User with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"User with id {id}  was not found to delete!!!");
        }
    }
}



