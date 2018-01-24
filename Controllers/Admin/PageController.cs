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

using System.Collections.Generic;
using System.Text;
using System.Web;
using CustomPages.Models.Admin;

namespace CustomPages.Controllers.Admin
{
    public class PageController
    {
        /// <summary>
        /// Default value of languages is fr.
        /// </summary>
        private static string currentLang = "fr";
       /* internal HttpServerUtilityBase Server;
        public PageController(HttpServerUtilityBase server)
        {
            Server = server;
        }*/

        /// <summary>
        /// permet la creation d'une page perso
        /// </summary>
        /// <param name="model"></param>
        /// <param name="la">Language to use </param>
        /// <returns></returns>
        public static bool Create(GenericPageModel model,string la=null)
        {
            model.Name =CleanString(model.Name);
            if (la != null) currentLang = la;
            try
            {
                string finalPath =
                    new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageLogik.PageFolderPath +
                                                                                     model.Name + "_" + currentLang +
                                                                                     ".html");
                if (!System.IO.Directory.Exists(finalPath))
                    System.IO.Directory.CreateDirectory(finalPath);
                System.IO.File.Create(finalPath).Close();
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
        /// <summary>
        /// Return a list of string of all the avaiable languages, permitting to set/edit the default language value with a selection box for example.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvaiableLanguages()
        {
            List<string> ss=new List<string>();
            foreach (string pageName in PageLogik.GetAllPagesNames())
            {
                string langName = pageName.Split('_')[1].Split('.')[0];
                if(ss.Contains(langName))continue;
                ss.Add(langName);
            }
            return ss;
        }
        /// <summary>
        /// Clean a string object for filesystem or/and SEO purposes.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CleanString(string str)
        {
            while (str.EndsWith("."))
            {
                str = str.Remove(str.Length - 1);
            } while (str.EndsWith(" "))
            {
                str = str.Remove(str.Length - 1);
            }
            StringBuilder u = new StringBuilder(str);
            u = u.Replace("ù", "u");
            u = u.Replace("û", "u");
            u = u.Replace("ï", "i");
            u = u.Replace("î", "i");
            u = u.Replace("ô", "o");
            u = u.Replace("œ", "o");
            u = u.Replace("ö", "o");
            u = u.Replace("é", "e");
            u = u.Replace("ê", "e");
            u = u.Replace("ë", "e");
            u = u.Replace("è", "e");
            u = u.Replace("â", "a");
            u = u.Replace("à", "a");
            u = u.Replace("'", "");
            u = u.Replace("’", "");
            u = u.Replace("&", "et");
            u = u.Replace("ç", "c");
            u = u.Replace("%", "");
            u = u.Replace("?", "");
            u = u.Replace("/", "-");
            u = u.Replace("\"", "");
            u = u.Replace(",", "");
            u = u.Replace("°", "");
            u = u.Replace(" ", "-");
            return u.ToString().ToLowerInvariant();
        }
    }
}
