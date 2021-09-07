using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Zhao.Backend.Extra
{
    public static class HtmlExtension
    {
        public static string IsSelected(this IHtmlHelper htmlHelper, string controllers, string actions)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.Values["action"] as string;
            var currentController = htmlHelper.ViewContext.RouteData.Values["controller"] as string;
            const string cssClass = "nav__item--is-active";

            IEnumerable<string> acceptedActions = (actions ?? currentAction)?.Split(',');
            IEnumerable<string> acceptedControllers = (controllers ?? currentController)?.Split(',');

            return (acceptedActions ?? Array.Empty<string>()).Contains(currentAction) &&
                   (acceptedControllers ?? Array.Empty<string>()).Contains(currentController)
                ? cssClass
                : string.Empty;
        }
    }
}