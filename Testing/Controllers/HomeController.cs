using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Testing.Models;
using Microsoft.AspNetCore.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using MyPortal.Connexus.Web.Filters;

namespace Testing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [SamlStateToTempData]
        [Route("/consumer"), HttpPost]
        public void Consumer(string samlResponse) {
            var homeModel = new HomeModel
            {
                thing1 = samlResponse + " thing is here",
                thing2 = samlResponse + " other thing here"
            };

            var objAsString = ObjectToString(homeModel);
            HttpContext.Session.SetString("HomeModel", objAsString);
            var result = HttpContext.Session.GetString("HomeModel");
            Response.Redirect(Url.Action("Confirm", "Home"));
            //return RedirectToAction("Confirm", "Home");
        }

        [Route("/confirm"), HttpGet]
        [SamlStateFromTempData]
        public IActionResult Confirm()
        {
            var result = HttpContext.Session.GetString("HomeModel");
            if (result != null && StringToObject(result) is HomeModel homeModel)
            {
                var count = this.ModelState.Count;
                return View("Index", homeModel);
            }
            else
            {
                return new RedirectResult("/");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private IActionResult GenerateBadRequest(string message)
        {
            return BadRequest(message); // Need to return Status Code that helps to redirect to ErrorController 
        }

        public string ObjectToString(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, obj);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public object StringToObject(string base64String)
        {
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return new BinaryFormatter().Deserialize(ms);
            }
        }
    }
}

[Serializable]
class HomeModel
{
    public string thing1;
    public string thing2;
}