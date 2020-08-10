//By Melissa Guzman de Gonzalez - 2020


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BairesCodingChallenge.Models;

namespace BairesCodingChallenge.Controllers
{
    [ApiController]
    [Route("clients")]
    public class ClientsController : Controller
    {
        [Route("topClients/{N:int}")]
        [HttpGet]
        public List<PersonIdModel> TopClients(int N)
        {          
            var result = from p in CommonFunctions.SortedList().Take(N) select new PersonIdModel { PersonId = p.PersonId };
            return result.ToList();
        }


        [Route("clientPosition/{PersonId:int}")]
        [HttpGet]
        public PositionModel ClientPosition(int PersonId)
        {            
            var sortedList = CommonFunctions.SortedList();
            int indexOfSelectedPerson = 0;

            var filteredList = sortedList.Select((item, i) => new {
                Item = item,
                Position = i
            }).Where(m => m.Item.PersonId == PersonId);

            if (filteredList.Count() > 0) {
                indexOfSelectedPerson = filteredList.First().Position + 1;
            }

            return new PositionModel { Position = indexOfSelectedPerson };

        }

        [Route("insertClient")]
        [HttpPost]
        public PositionModel InsertClient(int PersonId, string FirstName, string LastName, string CurrentRole, string Country, string Industry, int NumberOfRecommendations, int NumberOfConnections)
        {
            //Add the new person to the file
            var newPeople = new People
            {
                PersonId = PersonId,
                FirstName = FirstName,
                LastName = LastName,
                CurrentRole = CurrentRole,
                Country = Country,
                Industry = Industry,
                NumberOfRecommendations = NumberOfRecommendations,
                NumberOfConnections = NumberOfConnections
            };

            
            if (CommonFunctions.InsertPeople(newPeople))
            {
                //If the people is successfully inserted
                var sortedList = CommonFunctions.SortedList();
                int indexOfSelectedPerson = 0;

                var filteredList = sortedList.Select((item, i) => new {
                    Item = item,
                    Position = i
                }).Where(m => m.Item.PersonId == PersonId);

                if (filteredList.Count() > 0)
                {
                    indexOfSelectedPerson = filteredList.First().Position + 1;
                }

                return new PositionModel { Position = indexOfSelectedPerson };
            }            

            return new PositionModel { Position = 0 };

        }
    }
}