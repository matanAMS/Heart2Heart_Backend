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
    public class Comments
    {
        public Comments(int postId, int userId, string date, string message, string userName)
        {
            PostId = postId;
            UserId = userId;
            Date = date;
            Message = message;
            USER_NAME = userName;
        }

        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Date { get; set; }
        public string Message { get; set; }
        public string USER_NAME { get; }
    }
    [EnableCors("*", "*", "*")]
    public class CommentsController : ApiController
    {

        [Route("GetAllComments")]
        public IHttpActionResult Get(int postid)
        {

            try
            {
                Comments[] comment = PostsDB.GetCommentsForPost(postid).ToArray();
                if (comment != null)
                    return Ok(comment);
                else return Content(HttpStatusCode.NotFound, "Comment cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("InsertNewComment")]
        public IHttpActionResult Post([FromBody] Comments comment)
        {
            try
            {
                int newCode = PostsDB.InsertNewComment(comment);
                comment.PostId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetPostById/{comment.PostId}"), comment);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }

    }
}
