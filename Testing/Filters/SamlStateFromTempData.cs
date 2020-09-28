using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyPortal.Connexus.Web.Filters
{
    public class SamlStateFromTempData : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext) {
            if (filterContext.Controller is Controller controller) {
                var modelState = controller.TempData["Saml"] as ModelStateDictionary;

                if (modelState != null) {
                    if (filterContext.Result is ViewResult) {
                        controller.ModelState.Merge(modelState);
                    } else {
                        controller.TempData.Remove("Saml");
                    }
                }
            }
            base.OnActionExecuted(filterContext);
        }
    }
}
