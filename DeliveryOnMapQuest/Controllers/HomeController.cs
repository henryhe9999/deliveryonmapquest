using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeliveryOnMapQuest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchText)
        {
            //q = "few random words" (no need to remove '+' signs) 
            //var model = GetSearchResults(q)

            var url = $"https://api.delivery.com/merchant/search/delivery?client_id=YzY4ODY3MmQwZTFhYjJiODY4ZWViZDcxMzE1OGEwYzI3&address={searchText}";

            WebRequest request = WebRequest.Create(url);

            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();

            dynamic result = JsonConvert.DeserializeObject(responseFromServer);

            var resultUrl = "https://www.mapquestapi.com/staticmap/v4/getmap?key=lEv58TIptVgUM5NJ9NEPm6uluKAhokjW&size=600,400&type=map&imagetype=png&traffic=FLOW|CON|INC&pois=";

            //var distanceUrl = "https://www.mapquestapi.com/staticmap/v4/getmap?key=lEv58TIptVgUM5NJ9NEPm6uluKAhokjW&size=600,400&type=map&imagetype=png&declutter=false&shapeformat=cmp&shape=uajsFvh}qMlJsK??zKfQ??tk@urAbaEyiC??y]{|AaPsoDa~@wjEhUwaDaM{y@??t~@yY??DX&scenter={0}&ecenter={1}&traffic=4";
            var distanceUrl = "https://www.mapquestapi.com/staticmap/v5/map?start={0}&end={1}&size=600,400&key=lEv58TIptVgUM5NJ9NEPm6uluKAhokjW";
            var resultHtml = "";

            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //foreach (dynamic merchant in result.merchants)

            resultUrl += $"red-1,{result.search_address.latitude},{result.search_address.longitude}|";


            for (int i = 0; i < result.merchants.Count && i < 5; i++)
            {
                resultUrl += $"{letters[i]},{result.merchants[i].location.latitude},{result.merchants[i].location.longitude}|";

                var itemDistanceUrl = distanceUrl;
                itemDistanceUrl = itemDistanceUrl.Replace("{0}", $"{ result.search_address.latitude},{ result.search_address.longitude}");
                itemDistanceUrl = itemDistanceUrl.Replace("{1}", $"{result.merchants[i].location.latitude},{result.merchants[i].location.longitude}");
                    
                resultHtml += $"<tr><td><a href=\"#\" onclick=\"javascript:OpenPopup('{itemDistanceUrl}')\"><b>{letters[i]}</b></a></td><td><img style=\"height:60px;\" src =\"{result.merchants[i].summary.merchant_logo}\" /><a href=\"{result.merchants[i].summary.url.complete}\">{result.merchants[i].summary.name}</a></td></tr>";


            }

            resultHtml = "<table cellpading=5>" + resultHtml + "</table>";

            ViewBag.ResultUrl = resultUrl;
            ViewBag.ResultHtml = resultHtml;

    
            return View();
        }
    }
}