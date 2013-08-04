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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomPages.Models.Admin
{
    /// <summary>
    /// Class specifically made to be used in admin context.
    /// </summary>
    public class GenericPageModel: Models.GenericPageModel
    {
        /// <summary>
        /// Contenu de l'article
        /// </summary>
        [DataType(DataType.Html)]
        [AllowHtml]
        new public string HtmlData { get; set; }

        /// <summary>
        /// Lien final du fichier
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// <remarks>Not Used right now!</remarks>
        /// </summary>
        public string Language { get; set; }
        /// <summary>
        /// Utilisé en cas d'erreurs
        /// </summary>
        public static Exception Error;
        /// <summary>
        /// Convert a GenericPageModel to Admin.GenericPageModel.
        /// </summary>
        /// <param name="genericPageModel"></param>
        /// <returns></returns>
        public static GenericPageModel From(Models.GenericPageModel genericPageModel)
        {
            return new GenericPageModel
                       {
                           HtmlData = genericPageModel.HtmlData,
                           Name = genericPageModel.Name,
                           Title = genericPageModel.Title,
                           IsDraft = genericPageModel.IsDraft,
                           IsReadonly = genericPageModel.IsReadonly,
                           SortOrder = genericPageModel.SortOrder,
                           IsSystem = genericPageModel.IsSystem
                       };
        }
    }
}
