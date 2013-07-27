﻿/*This file is part of CustomPages.
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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CustomPages.Models
{
    public class GenericPageModel
    {
        /// <summary>
        /// Nom du fichier
        /// </summary>
        public string Name { get; set; }        
        /// <summary>
        /// Contenu de l'article
        /// </summary>
        public string HtmlData { get; set; }
        /// <summary>
        /// Titre de la page (peut différer du nom.)
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Détermine si la page s'affiche ou non sur le site.
        /// note: un brouillon est de la forme "_dr_[nomfichier]-[lang].html"
        /// </summary>
        public bool IsDraft { get; set; }
        /// <summary>
        /// Détermine l'affichage ou non du boutton supprimer pour la page actuelle.
        /// </summary>
        public bool IsReadonly { get; set; }

        /// <summary>
        /// Ordre d'affichage de la page sur le site
        /// </summary>
        public int SortOrder { get; set; }


        /// <summary>
        /// Cette page est-elle système ou non?
        /// </summary>
        public bool IsSystem { get; set; }

    }
}
