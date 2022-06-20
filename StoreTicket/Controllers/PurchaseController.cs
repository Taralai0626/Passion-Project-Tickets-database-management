using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using StoreTicket.Models;
//using passionProject_storeTickets2022.Models.ViewModels;
using System.Web.Script.Serialization;

namespace StoreTicket.Controllers
{
    public class PurchaseController : Controller
    {
        private static readonly HttpClient client;
        JavaScriptSerializer jss = new JavaScriptSerializer();

        static PurchaseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }
        // GET: Purchase/List
        public ActionResult List()
        {
            // objective: communicate with purchase data api to retrieve a list of purchases
            //curl https://localhost:44320/api/purchasedata/listpurchases

            string url = "purchasedata/listpurchases";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            IEnumerable<PurchaseDto> purchases = response.Content.ReadAsAsync<IEnumerable<PurchaseDto>>().Result;
            //Debug.WriteLine("Number of purchases received : ");
            //Debug.WriteLine(purchase.Count());

            return View(purchases);
        }

        // GET: Purchase/Details/5
        public ActionResult Details(int id)
        {
            //objective: communicate with our purchase data api to rwtrieve one purchase
            //curl https://localhost:44320/api/purchasedata/findpurchase/{id}

            string url = "purchasedata/findpurchase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PurchaseDto selectedpurchase = response.Content.ReadAsAsync<PurchaseDto>().Result;
            //Debug.WriteLine("purchase received : ");
            //Debug.WriteLine(selectedpurchase.purchaseName);
            return View(selectedpurchase);
        }

        public ActionResult Error()
        {
            return View();
        }
        // GET: Purchase/Create
        public ActionResult New()
        {
            string url = "purchasedata/listpurchases";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<PurchaseDto> purchases = response.Content.ReadAsAsync<IEnumerable<PurchaseDto>>().Result;
            return View();
        }

        // POST: Purchase/Create
        [HttpPost]
        public ActionResult Create(Purchase purchase)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(purchase.PurchaseId);
            //objective: add a new purchase into our system using the API
            //curl -d @purchase.json -H "Content-type:application/json" https://localhost:44320/api/purchasedata/addpurchases
            string url = "purchasedata/addpurchase";

            string jsonpayload = jss.Serialize(purchase);

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

        // GET: Purchase/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "purchasetdata/findpurchase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PurchaseDto selectedpurchase = response.Content.ReadAsAsync<PurchaseDto>().Result;
            return View(selectedpurchase);
        }

        // POST: Purchase/Update/5
        [HttpPost]
        public ActionResult Edit(int id, Purchase purchase)
        {
            string url = "purchasedata/updatepurchase/" + id;
            string jsonpayload = jss.Serialize(purchase);
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

        // GET: Purchase/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "purchasedata/findpurchase/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PurchaseDto selectedpurchase = response.Content.ReadAsAsync<PurchaseDto>().Result;
            return View(selectedpurchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "purchasetdata/deletepurchase/" + id;
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
