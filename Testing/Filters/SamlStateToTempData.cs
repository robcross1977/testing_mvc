using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyPortal.Connexus.Web.Filters
{
    public class SamlStateToTempData : ActionFilterAttribute {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller is Controller controller) {
                if (IsInvalidResult(filterContext)) {
                    return;
                }
                controller.TempData["Saml"] = controller.ViewData.ModelState;
            }
            base.OnActionExecuted(filterContext);
        }

        private static bool IsInvalidResult(ActionExecutedContext context) =>
            !(context.Result is RedirectToActionResult) &&
            !(context.Result is RedirectResult) &&
            !(context.Result is RedirectToRouteResult);
    }
}
