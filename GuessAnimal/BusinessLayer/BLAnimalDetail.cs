using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GuessAnimal.Models;

namespace GuessAnimal.BusinessLayer
{
    public class BLAnimalDetail
    {
        private AnimalEntities db = new AnimalEntities();

        /// <summary>
        /// Retrieve one fact(Animal Detail) for each animal.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="currentFact"></param>
        /// <returns></returns>
        public AnimalDetail GetFactForNextAnimal(int animalId)
        {
            int nextAnimalId = 0;
            if (animalId > 0)
            {
                nextAnimalId = animalId + 1;
            }
            else {
                nextAnimalId = db.Animals.OrderBy(a => a.Id).FirstOrDefault().Id;

            }
                
                var animalDetails = from element in db.AnimalDetails.Include(a => a.Animal)
                                    group element by element.AnimalId
                                    into groups
                                    select groups.OrderBy(p => p.FactId).FirstOrDefault();
                return animalDetails.Include(a => a.Animal).ToList().Where(ad => ad.AnimalId == nextAnimalId).OrderBy(a => a.FactId).FirstOrDefault();
                //from element in db.AnimalDetails.Include(a => a.Animal)
                //                select groups.OrderBy(p => p.FactId).FirstOrDefault();
            
            
            return null;
            
        }

        /// <summary>
        /// if one fact is matched then retrieve the next fact for the same Animal.
        /// </summary>
        /// <param name="currentFact"></param>
        /// <returns></returns>
        public AnimalDetail GetNextFactForAnimal(int animalId, int factId)
        {
            //if (currentFact != null)
           // {
                int nextAnimalId = animalId + 1;

                var items = db.AnimalDetails.Include(a => a.Animal).Where(ad => ad.AnimalId == animalId).OrderBy(a => a.FactId).ToList();
                int index = items.FindIndex(it => it.FactId == factId);

                if (items.Count > index + 1)
                {
                    return items.ElementAt(index + 1);
                }
                else {
                   
                }
                
          //  }
            return null;

        }

        //private AnimalDetail AddOptions(AnimalDetail animalDetail)
        //{
        //    animalDetail.Options.Add(new AnswerOption() { Title="Yes"});

        //    animalDetail.Options.Add(new AnswerOption() { Title = "No" });

        //    return animalDetail;
        //}
    }
}