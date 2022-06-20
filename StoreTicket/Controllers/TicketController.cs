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
    public class TicketController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static TicketController()
        {   
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/"); 
        }
        // GET: Ticket/List
        public ActionResult List()
        {
            //objective: communicate with ticket data api to retrieve a list of tickets
            //curl https://localhost:44320/api/ticketdata/listtickets

            string url = "ticketdata/listtickets";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<TicketDto> tickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;
            //Debug.WriteLine("Number of tickets received : ");
            //Debug.WriteLine(ticket.Count());

            return View(tickets);
        }

        // GET: Ticket/Details/5
        public ActionResult Details(int id)
        {   
            DetailsTicket ViewModel = new DetailsTicket();
            //objective: communicate with our ticket data api to rwtrieve one Ticket
            //curl https://localhost:44320/api/ticketdata/findticket/{id}

            string url = "ticketdata/findticket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            TicketDto SelectedTicket = response.Content.ReadAsAsync<TicketDto>().Result;
            //Debug.WriteLine("Ticket received : ");
            //Debug.WriteLine(selectedTicket.TicketName);
            
            ViewModel.SelectedTicket = SelectedTicket;

            //show associated Users with this ticket
            url = "userdata/listusersforticket/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<PassionuserDto> ResponsibleUsers = response.Content.ReadAsAsync<IEnumerable<PassionuserDto>>().Result;

            ViewModel.ResponsibleUsers = ResponsibleUsers;


            url = "userdata/listusersnothavingticket/" + id;
            response= client.GetAsync(url).Result;
            IEnumerable<PassionuserDto> AvailableUsers = response.Content.ReadAsAsync<IEnumerable<PassionuserDto>>().Result;

            ViewModel.AvailableUsers = AvailableUsers;

            return View(ViewModel);
        }

        //POST: Ticket/Associate/{ticketid}
        [HttpPost]
        public ActionResult Associate(int id, int UserId)
        {
            Debug.WriteLine("Attempting to associate ticket :" + id + " with user " + UserId);

            //call our api to associate ticket with user
            string url = "ticketdata/associateticketwithuser/" + id + "/" + UserId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }


        //Get: Ticket/UnAssociate/{id}?userID={userID}
        [HttpGet]
        public ActionResult UnAssociate(int id, int UserId)
        {
            Debug.WriteLine("Attempting to unassociate ticket :" + id + " with user: " + UserId);

            //call our api to associate ticket with user
            string url = "ticketdata/unassociateticketwithuser/" + id + "/" + UserId;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Details/" + id);
        }

        
        public ActionResult Error()
        {
            return View();
        }

        // GET: Ticket/New
        public ActionResult New()
        {
            string url = "websitedata/listwebsites";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<WebsiteDto> WebsiteOptions = response.Content.ReadAsAsync<IEnumerable<WebsiteDto>>().Result;
           
            return View(WebsiteOptions);
        }

        // POST: Ticket/Create
        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(ticket.TicketId);
            //objective: add a new ticket into our system using the API
            //curl -d @ticket.json -H "Content-type:application/json" https://localhost:44320/api/ticketdata/addticket
            string url = "ticketdata/addticket";

            string jsonpayload = jss.Serialize(ticket);

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

        // GET: Ticket/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateTicket ViewModel = new UpdateTicket();

            string url = "ticketdata/findticket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto SelectedTicket = response.Content.ReadAsAsync<TicketDto>().Result;
            ViewModel.SelectedTicket = SelectedTicket;

            // all website to choose from when updating seleted ticket
            url = "websitedata/listWebsites/";
            response = client.GetAsync(url).Result;
            IEnumerable<WebsiteDto> WebsiteOptions = response.Content.ReadAsAsync<IEnumerable<WebsiteDto>>().Result;

            ViewModel.WebsiteOptions = WebsiteOptions;
           // Debug.WriteLine("pleease tell me what went wrong");
           // Debug.WriteLine(ViewModel);
            //Debug.WriteLine(url);

            return View(ViewModel);
        }

        // POST: Ticket/Update/5
        [HttpPost]
        public ActionResult Update(int id, Ticket ticket, HttpPostedFileBase TicketPic)
        {
            string url = "ticketdata/updateticket/" + id;
            string jsonpayload = jss.Serialize(ticket);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine("response" + response);
            Debug.WriteLine("content" + content);

            //update request is successful, and we have image data
            if (response.IsSuccessStatusCode && TicketPic != null)
            {
                //Updating the Ticket picture as a separate request
                Debug.WriteLine("Calling Update Image method.");
                //Send over image data for player
                url = "TicketData/UploadTicketPic/" + id;
                //Debug.WriteLine("Received Ticket Picture "+TicketPic.FileName);

                MultipartFormDataContent requestcontent = new MultipartFormDataContent();
                HttpContent imagecontent = new StreamContent(TicketPic.InputStream);
                requestcontent.Add(imagecontent, "TicketPic", TicketPic.FileName);
                response = client.PostAsync(url, requestcontent).Result;

                return RedirectToAction("List");
            }
            else if (response.IsSuccessStatusCode)
            {
                //No image upload, but update still successful
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Ticket/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "ticketdata/findticket/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TicketDto SelectedTicket = response.Content.ReadAsAsync<TicketDto>().Result;
            return View(SelectedTicket);
        }

        // POST: Ticket/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "ticketdata/deleteticket/" + id;
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
