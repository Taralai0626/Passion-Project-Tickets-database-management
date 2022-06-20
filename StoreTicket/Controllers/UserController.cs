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
    public class UserController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static UserController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }

        // GET: User/List
        public ActionResult List()

        {
            //objective: communicate with our animal data api to retrieve a list of animals
            //curl https://localhost:44320/api/userdata/listusers

            string url = "userdata/listusers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PassionuserDto> users = response.Content.ReadAsAsync<IEnumerable<PassionuserDto>>().Result;
            //Debug.WriteLine("Number of users received : ");
            //Debug.WriteLine(users.Count());

            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            DetailsUser viewModel = new DetailsUser();
            //objective: communicate with our animal data api to rwtrieve one user
            //curl https://localhost:44320/api/userdata/finduser/{id}

            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PassionuserDto SelectedUser = response.Content.ReadAsAsync<PassionuserDto>().Result;
            //Debug.WriteLine("User received : ");
            //Debug.WriteLine(selectedUser.UserName);
            viewModel.SelectedUser = SelectedUser;

            //show all tickets under the care of this user
            url = "ticketdata/listticketsforuser/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<TicketDto> KeptTickets = response.Content.ReadAsAsync<IEnumerable<TicketDto>>().Result;

            viewModel.KeptTickets = KeptTickets;

            return View(viewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: User/New
        public ActionResult New()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(Passionuser user)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(user.UserLastName);
            //objective: add a new user into our system using the API
            //curl -d @user.json -H "Content-type:application/json" https://localhost:44320/api/userdata/adduser
            string url = "userdata/adduser";

            string jsonpayload = jss.Serialize(user);

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

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassionuserDto selectedUser = response.Content.ReadAsAsync<PassionuserDto>().Result;
            return View(selectedUser);
        }

        // POST: User/Update/5
        [HttpPost]
        public ActionResult Update(int id, Passionuser user)
        {
            string url = "userdata/updateuser/" + id;
            string jsonpayload = jss.Serialize(user);
            Debug.WriteLine(jsonpayload + "or This?");
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response);
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

        // GET: User/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "userdata/finduser/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PassionuserDto selecteduser = response.Content.ReadAsAsync<PassionuserDto>().Result;
            return View(selecteduser);
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "userdata/deleteuser/" + id;
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
