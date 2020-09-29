using System;
using Microsoft.AspNetCore.Mvc;
using MyPortal.Connexus.Web.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Testing.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return View();
        }

        [SamlStateToTempData]
        [Route("/consumer"), HttpPost]
        public IActionResult Consumer(string samlResponse) {
            return Confirm();
        }

        [Route("/confirm"), HttpGet]
        [SamlStateFromTempData]
        public IActionResult Confirm() {
            if (this.ModelState.TryGetValue("samlResponse", out ModelStateEntry modelStateEntry)) {
                HomeModel homeModel = new HomeModel {
                    thing1 = "this is thing1 " + modelStateEntry.RawValue,
                    thing2 = "this is thing2 " + modelStateEntry.RawValue
                };

                return View("Index", homeModel);
            } else {
                return new RedirectResult("/");
            }
        }
    }
}

[Serializable]
class HomeModel {
    public string thing1;
    public string thing2;
}