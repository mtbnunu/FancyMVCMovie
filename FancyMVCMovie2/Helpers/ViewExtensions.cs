using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public static class ViewExtensions
    {
        public static MvcHtmlString MyValidationSummary(this HtmlHelper helper, string validationMessage = "")
        {
            string retVal = "";
            if (helper.ViewData.ModelState.IsValid)
                return new MvcHtmlString("");

            retVal += "<div class='card-panel validation-summary red lighten-4'><span class='red-text text-darken-3'><ul><li>";
            if (!String.IsNullOrEmpty(validationMessage))
                retVal += helper.Encode(validationMessage);
            retVal += "</li>";
            foreach (var key in helper.ViewData.ModelState.Keys)
            {
                foreach (var err in helper.ViewData.ModelState[key].Errors)
                    retVal += "<li>" + helper.Encode(err.ErrorMessage) + "</li>";
            }
            retVal += "<ul></span></div>";
            return new MvcHtmlString(retVal);
        }
    }
}