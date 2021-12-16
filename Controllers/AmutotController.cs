using Heart2HeartBackend.Models;
using System;
using System.Web.Http.Cors;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace Heart2HeartBackend.Controllers
{
    [EnableCors("*", "*", "*")]
    public class AmutotController : ApiController
    {
    

        //check if exist
        [Route("InsertNewAmuta")]
        public IHttpActionResult Post([FromBody] Amutot amuta)
        {
            try
            {
                //chek if email already taken
                Amutot amutaToCheck = AmutotDB.GetAmutaByHP(amuta.AmutaHP);
                if (amutaToCheck != null)
                    return Content(HttpStatusCode.Conflict, $"Amuta with AMUTA_HP '{amuta.AmutaHP}' already exists.");

                int newCode = AmutotDB.InsertNewAmuta(amuta);
                amuta.AmutaID = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetAmutaByID/{amuta.AmutaID}"), amuta);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }
        // GET: Amutot
        [Route("GetAmutaById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Amutot amuta = AmutotDB.GetAmutaById(id);
                if (amuta != null)
                    return Ok(amuta);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("GetAllAmutot")]
        public IHttpActionResult GetA()
        {
            try
            {
                Amutot[] amutot = AmutotDB.GetAllAmutot().ToArray();
                if (amutot != null)
                    return Ok(amutot);
                else return Content(HttpStatusCode.NotFound, "amutot dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        [Route("GetAllAmutotForAdmin")]
        public IHttpActionResult Get()
        {
            try
            {
                Amutot[] amutot = AmutotDB.GetAllAmutotForAdmin().ToArray();
                if (amutot != null)
                    return Ok(amutot);
                else return Content(HttpStatusCode.NotFound, "Amuta dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }
        [Route("UpdateAmutaDisplay")]
        public IHttpActionResult PostA([FromBody] Amutot amuta)
        {
            try
            {
                int rowsEffected = AmutotDB.UpdateAmutaDisplay(amuta);
                if (rowsEffected > 0)
                { 
                  return Content(HttpStatusCode.OK, amuta);
                }
                else return Content(HttpStatusCode.NotFound, $"User with id {amuta.AmutaID} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("GetAmutaByHP/{hp}")]
        public IHttpActionResult Get(string hp)
        {
            try
            {
                Amutot amuta = AmutotDB.GetAmutaByHP(hp);
                if (amuta != null)
                    return Ok(amuta);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("UpdateAmuta")]
        public IHttpActionResult Put([FromBody] Amutot amuta)
        {
            try
            {
                int rowsEffected = AmutotDB.UpdateAmuta(amuta);
                if (rowsEffected > 0) return Content(HttpStatusCode.OK, amuta);
                else return Content(HttpStatusCode.NotFound, $"User with id {amuta.AmutaID} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        [Route("DeleteAmuta/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = AmutotDB.DeleteAmuta(id);
            if (val > 0) return Ok($"User with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"User with id {id}  was not found to delete!!!");
        }
    }
}
