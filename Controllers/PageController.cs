/*This file is part of CustomPages.
 * 
 * Foobar is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * any later version.
 *
 * CustomPages is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
 *  
 * Copyright (c) Meï-Garino Jérémy 
*/
using System;
using System.Web.Mvc;
using CustomPages.Models;

namespace CustomPages.Controllers
{
    /// <summary>
    /// <remarks>Not used right now.</remarks>
    /// </summary>
    class PageController : Controller
    {
    /*    ///<summary>
        /// cache à 1h
        /// </summary>
        //[OutputCache(Duration = 3600, VaryByParam = "none")]
        public ActionResult Page(string id)
        {
            Tuple<string, string> data = PageLogik.ReadHtmlData(Server, id, "fr");
            GenericPageModel model = new GenericPageModel { Title = data.Item1, HtmlContent = data.Item2 };
            ViewBag.Title = PageLogik.SiteName + " " + model.Title;
            return View("GenericHomePageView", model);
        }*/
    }
}
