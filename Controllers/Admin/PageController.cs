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
using System.Web;
using CustomPages.Models.Admin;

namespace CustomPages.Controllers.Admin
{
    public class PageController
    {
       /* internal HttpServerUtilityBase Server;
        public PageController(HttpServerUtilityBase server)
        {
            Server = server;
        }*/
        /// <summary>
        /// permet la creation d'une page perso
        /// todo: ajouter la possibilité de changer la langue depuis un EnumString.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool Create(GenericPageModel model)
        {
            try
            {
                System.IO.File.Create(new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageLogik.PageFolderPath + model.Name + "_fr.html")).Close();
                return true;
            }
            catch { return false; }
        }
        public bool Update()
        {

            return false;
        }
        /// <summary>
        /// Supprime simplement une page.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Delete(string name)
        {
            System.IO.File.Delete(new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageLogik.PageFolderPath + name + "_fr.html"));
            return false;

        }

    }
}
