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
    [EnableCors("*", "*", "*")]
    public class PostController : ApiController
    {
        //http://localhost:59821/GetAllPosts
        [Route("GetAllPosts")]
        public IHttpActionResult Get()
        {

            try
            {
                Posts[] temp = PostsDB.GetAllPosts().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Posts cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        [Route("GetAllPostsForAmutot")]
        public IHttpActionResult GetP()
        {

            try
            {
                Posts[] temp = PostsDB.GetAllPostsForAmutot().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Posts cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }






        //http://localhost:59821/PostsOfPartnersByUserId/5
        [Route("PostsOfAmutotByUserId/{id}")]
        [HttpGet]
        public IHttpActionResult PostsOfAmutotByUserId(int id)
        {
            try
            {
                Posts[] p = PostsDB.PostsOfAmutotByUserId(id).ToArray();

                if (p != null)
                    return Ok(p);
                else return Content(HttpStatusCode.NotFound, "Posts dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }





        //http://localhost:59821/GetPostById/5
        [Route("GetPostById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Posts[] p = PostsDB.GetPostById(id).ToArray();

                if (p != null)
                    return Ok(p);
                else return Content(HttpStatusCode.NotFound, "Post dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



        [Route("InsertNewPost")]
        public IHttpActionResult Post([FromBody] Posts post)
        {
            try
            {
                int newCode = PostsDB.InsertNewPost(post);
                post.PostId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetPostById/{post.PostId}"), post);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:59821/UpdatePost
        [Route("UpdatePost")]
        public IHttpActionResult Put([FromBody] Posts post)
        {
            try
            {
                int val = PostsDB.UpdatePost(post);

                if (val > 0) return Content(HttpStatusCode.OK, post);
                else return Content(HttpStatusCode.NotFound, $"{post.Description} with id {post.PostId} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



        [Route("DeletePost/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = PostsDB.DeletePost(id);
            if (val > 0) return Ok($"Post with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Post with id {id}  was not found to delete!!!");
        }
    }
}