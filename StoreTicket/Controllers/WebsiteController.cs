using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using StoreTicket.Models;
using StoreTicket.Models.ViewModels;
using System.Web.Script.Serialization;

namespace StoreTicket.Controllers
{
    public class WebsiteController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static WebsiteController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }

        // GET: Website/List
        public ActionResult List()
        {
            //objective: communicate with our animal data api to retrieve a list of animals
            //curl https://localhost:44320/api/userdata/listwebsites

            string url = "websitedata/listwebsites";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<WebsiteDto> websites = response.Content.ReadAsAsync<IEnumerable<WebsiteDto>>().Result;
            //Debug.WriteLine("Number of website received : ");
            //Debug.WriteLine(users.Count());
            return View(websites);
        }

        // GET: Website/Details/5
        public ActionResult Details(int id)
        {   
            DetailsWebsite ViewModel = new DetailsWebsite();

            //objective: communicate with our animal data api to rwtrieve one user
            //curl https://localhost:44320/api/userdata/findwebsite/{id}

            string url = "websitedata/findwebsite/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            WebsiteDto SelectedWebsite = response.Content.ReadAsAsync<WebsiteDto>().Result;
            //Debug.WriteLine("websitee received : ");
            //Debug.WriteLine(selecteduser.UserName);

            ViewModel.SelectedWebsite = SelectedWebsite;

            url = "ticketdata/ListTicketsForWebsite/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TicketDto> RelatedTickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;
            ViewModel.RelatedTickets = RelatedTickets;

            return View(ViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Website/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Website/Create
        [HttpPost]
        public ActionResult Create(Website website)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(website);
            //objective: add a new user into our system using the API
            //curl -d @user.json -H "Content-type:application/json" https://localhost:44320/api/websitedata/addwebsite
            string url = "websitedata/addwebsite";

            string jsonpayload = jss.Serialize(website);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Website/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "websitedata/findwebsite/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WebsiteDto SelectedWebsite = response.Content.ReadAsAsync<WebsiteDto>().Result;
            return View(SelectedWebsite);
        }

        // POST: Website/Update/5
        [HttpPost]
        public ActionResult Update(int id, Website website)
        {
            string url = "websitedata/updatewebsite/" + id;
            Debug.WriteLine(url + "This?");
            string jsonpayload = jss.Serialize(website);
            Debug.WriteLine(jsonpayload + "or This?");
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Website/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "websitedata/findwebsite/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WebsiteDto SelectedWebsite = response.Content.ReadAsAsync<WebsiteDto>().Result;
            return View(SelectedWebsite);
        }

        // POST: Website/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "websitedata/deletewebsite/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
