using dsccUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace dsccUI.Controllers
{
    public class PhoneController : Controller
    {
        [Route("Phone/Index")]
        public async Task<ActionResult> Index()
        {
            string Baseurl = Constants.Index.BaseUrl;
            List<Phone> phonesInfo = new List<Phone>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("api/phone");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    phonesInfo = JsonConvert.DeserializeObject<List<Phone>>(result);
                }
                return View(phonesInfo);
            }
        }

        [Route("Phone/Details/{id}")]
        public async Task<ActionResult> Details(Guid id)
        {
            string Baseurl = Constants.Index.BaseUrl;
            Phone phoneInfo = new Phone();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new
                MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/phone/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    phoneInfo = JsonConvert.DeserializeObject<Phone>(result);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return HttpNotFound();
                    }
                }
                return View(phoneInfo);
            }
        }

        public async Task<ActionResult> Create([Bind(Include = "Name, Image, Price")] Phone phone)
        {
            Phone phoneInfo = new Phone();
            try
            {
                string Baseurl = Constants.Index.BaseUrl;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync($"api/phone", phone);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        phoneInfo = JsonConvert.DeserializeObject<Phone>(result);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch 
            {
                return View();
            }
            return View(phoneInfo);
        }

        // GET: phone/edit/{guid}
        public async Task<ActionResult> Edit(Guid id)
        {
            string Baseurl = Constants.Index.BaseUrl;
            Phone phone = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage res = await client.GetAsync($"api/phone/{id}");
            if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    phone = JsonConvert.DeserializeObject<Phone>(result);
                }
                else { 
                    ModelState.AddModelError(string.Empty, "Server Error. Please contactadministrator.");
                }
            }

            return View(phone);
        }
        // POST: phone/edit/{guid}
        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, Phone ph)
        {
            try
            {
                string Baseurl = Constants.Index.BaseUrl;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage response = await client.GetAsync($"api/phone/{id}");
                    Phone phone = null;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        phone = JsonConvert.DeserializeObject<Phone>(result);
                    }
                    phone.Name = ph.Name;
                    phone.Price = ph.Price;
                    phone.Image = ph.Image;

                    var putResponse = await client.PutAsJsonAsync<Phone>($"api/phone/{id}", phone);
                    var putResult = putResponse.Content.ReadAsStringAsync().Result;
                    if (putResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

        // DELETE: phone/delete/{guid}
        public async Task<ActionResult> Delete(Guid? id)
        {
            string Baseurl = Constants.Index.BaseUrl;
            Phone phone = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage res = await client.GetAsync($"api/phone/{id}");
                if (res.IsSuccessStatusCode)
                {
                    var result = res.Content.ReadAsStringAsync().Result;
                    phone = JsonConvert.DeserializeObject<Phone>(result);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
                }
            }

            return View(phone);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                string Baseurl = Constants.Index.BaseUrl;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    HttpResponseMessage response = await client.GetAsync($"api/phone/{id}");
                    Phone phone = null;
                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        phone = JsonConvert.DeserializeObject<Phone>(result);
                    }
                    var deleteResponse = await client.DeleteAsync($"api/phone/{id}");
                    var deleteResult = deleteResponse.Content.ReadAsStringAsync().Result;
                    if (deleteResponse.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }

    }
}
