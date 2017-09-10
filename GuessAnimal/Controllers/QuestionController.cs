using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using GuessAnimal.Models;
using GuessAnimal.BusinessLayer;
using Newtonsoft.Json;


namespace GuessAnimal.Api.Controllers
{
    [RoutePrefix("api/question")]
    //[Produces("application/json")]
    public class QuestionController : ApiController
    {
        private AnimalEntities db = new AnimalEntities();

        // GET: api/Question
        public IQueryable<AnimalDetail> GetAnimalDetails()
        {
            return db.AnimalDetails;
        }

        // GET: api/Question
        [ResponseType(typeof(AnimalDetail))]
     //   [Route("Get/{animalId}/{factId}/{selectedAnswer}")]
        public IHttpActionResult Get([FromUri]int  animalId, int factId, string selectedAnswer)
        {
            if (animalId == 0 && factId == 0)
            {
                var animalDetails = from element in db.AnimalDetails.Include(a => a.Animal)
                                    group element by element.AnimalId
                            into groups
                                    select groups.OrderBy(p => p.FactId).FirstOrDefault();

                return Ok(animalDetails.ToList().FirstOrDefault());
            }
            AnimalDetail temp = null;

            if (!string.IsNullOrEmpty(selectedAnswer) && selectedAnswer.ToLower().Equals("yes"))
            {
                temp = new BusinessLayer.BLAnimalDetail().GetNextFactForAnimal(animalId,factId);
                //ViewData["AnimalFound"] = true;
            }
            else if (!string.IsNullOrEmpty(selectedAnswer) && selectedAnswer.ToLower().Equals("no"))
            {
                temp = new BusinessLayer.BLAnimalDetail().GetFactForNextAnimal(animalId);
                //ViewData["AnimalFound"] = false;
            }
            //  ModelState.Clear();

            if (temp == null)
            {
                //  ViewData["EndGuess"] = true;

                var animal = db.Animals.Find(animalId);
                if (animal != null)
                {
                    //    ViewData["AnimalName"] = animal.Name;
                }
            }


            return Ok(temp);
        }

        // GET: api/Question/5
        [ResponseType(typeof(AnimalDetail))]
        //public IHttpActionResult Get(int id)
        //{
        //    AnimalDetail animalDetail = db.AnimalDetails.Find(id);
        //    if (animalDetail == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(animalDetail);
        //}

        // GET: api/Question/5
        [ResponseType(typeof(AnimalDetail))]
        public IHttpActionResult GetAnimalDetail(int id)
        {
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return NotFound();
            }

            return Ok(animalDetail);
        }

        // PUT: api/Question/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAnimalDetail(int id, AnimalDetail animalDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != animalDetail.FactId)
            {
                return BadRequest();
            }

            db.Entry(animalDetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnimalDetailExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        // POST: api/Question
//        [ResponseType(typeof(AnimalDetail))]
//        public IHttpActionResult Post()
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//           // db.AnimalDetails.Add(animalDetail);
////db.SaveChanges();

//            return CreatedAtRoute("DefaultApi", new { id = 0 }, animalDetail);
//        }

        // POST: api/Question
        [ResponseType(typeof(AnimalDetail))]
        public IHttpActionResult PostAnimalDetail(AnimalDetail animalDetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AnimalDetails.Add(animalDetail);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = animalDetail.FactId }, animalDetail);
        }

        // DELETE: api/Question/5
        [ResponseType(typeof(AnimalDetail))]
        public IHttpActionResult DeleteAnimalDetail(int id)
        {
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return NotFound();
            }

            db.AnimalDetails.Remove(animalDetail);
            db.SaveChanges();

            return Ok(animalDetail);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AnimalDetailExists(int id)
        {
            return db.AnimalDetails.Count(e => e.FactId == id) > 0;
        }
    }
}