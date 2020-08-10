//By Melissa Guzman de Gonzalez - 2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using BairesCodingChallenge.Models;
using System.Reflection;

namespace BairesCodingChallenge
{
    public static class CommonFunctions
    {

        public static List<People> ReadPeopleFile()
        {
            try
            {
                var options = new JsonSerializerOptions();

                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data\people.json");
                var jsonString = File.ReadAllText(path);
                var jsonModel = JsonSerializer.Deserialize<List<People>>(jsonString, options);

                return jsonModel;

            } catch 
            {
                return new List<People>();
            }
        }


        public static bool InsertPeople(People newPeople)
        {
            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"data\people.json");

                var options = new JsonSerializerOptions();
                var jsonString = File.ReadAllText(path);
                var jsonModel = JsonSerializer.Deserialize<List<People>>(jsonString, options);
                jsonModel.Add(newPeople);

                var convertedJson = JsonSerializer.Serialize(jsonModel);
                File.WriteAllText(path, convertedJson);               

                return true;

            }
            catch
            {
                return false;
            }
        }

        public static List<People> FilterList(List<People> peopleList, string Role)
        {
            return peopleList.Where(p => p.CurrentRole.Contains(Role)).OrderByDescending(p => p.NumberOfConnections).OrderByDescending(p => p.NumberOfRecommendations).ToList();
        }

        public static IEnumerable<People> SortedList ()
        {
            //Read the file
            var peopleList = CommonFunctions.ReadPeopleFile();

            //Filter for different positions that I find relevant
            var resultList = FilterList(peopleList, "president");

            resultList = resultList.Concat(FilterList(peopleList, "ceo").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();

            resultList = resultList.Concat(FilterList(peopleList, "chief").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();

            resultList = resultList.Concat(FilterList(peopleList, "vice president").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();

            resultList = resultList.Concat(FilterList(peopleList, "manager").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();

            resultList = resultList.Concat(FilterList(peopleList, "director").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();

            resultList = resultList.Concat(FilterList(peopleList, "technolog").Where(p => resultList.All(p2 => p2.PersonId != p.PersonId))).ToList();
            
            //Find the rest of the people
            var others = peopleList.Where(p => resultList.All(p2 => p2.PersonId != p.PersonId)).ToList();
            
            resultList = resultList.Concat(others).ToList();

            return resultList.ToArray();
        }
        
    }
}
