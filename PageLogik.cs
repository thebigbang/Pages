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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using CustomPages.Models;

namespace CustomPages
{
    internal enum PageParameters
    {
        Title,
        IsDraft,
        IsReadOnly
    }
    public static class PageLogik
    {
        /// <summary>
        /// dossier ou sont rangés les pages .html
        /// </summary>
        internal const string PageFolderPath = "~/Content/pages/";
        /// <summary>
        /// Permet de séparer via nom de fichier les brouillons des autres pages.
        /// Ceux dont le nom commence par "_dr_" sont brouillons, pas les autres.
        /// </summary>
        internal const string PageDraft = "_dr_";
        /// <summary>
        /// Définit le début de nos informations de parametres
        /// </summary>
        private const string HtmlParamIndicatorBegin = "<!--?PARAMS";
        /// <summary>
        /// Définit la fin de notre zone d'informations de parametres
        /// </summary>
        private const string HtmlParamIndicatorEnd = "?PARAMS-->";
        /// <summary>
        /// Définit le début de notre zone de parametre:titre de page
        /// </summary>
        private const string HtmlParamTitleBegin = "#TITLE#";
        /// <summary>
        /// Définit la fin de notre zone de parametre:titre de page
        /// </summary>
        private const string HtmlParamTitleEnd = "#!TITLE#";
        /// <summary>
        /// Définit le début de notre zone de parametre:brouillon de page
        /// </summary>
        private const string HtmlParamDraftBegin = "#ISDRAFT#";
        /// <summary>
        /// Définit la fin de notre zone de parametre:brouillon de page
        /// </summary>
        private const string HtmlParamDraftEnd = "#!ISDRAFT#";
        /// <summary>
        /// Définit le début de notre zone de parametre:lecture_seule de page
        /// </summary>
        private const string HtmlParamReadOnlyBegin = "#ISREADONLY#";
        /// <summary>
        /// Définit la fin de notre zone de parametre:lecture_seule de page
        /// </summary>
        private const string HtmlParamReadOnlyEnd = "#!ISREADONLY#";

        /// <summary>
        /// Définit le début de zone de parametre: ordre d'affichage de la page.
        /// </summary>
        private const string HtmlParamSortOrderBegin = "#SORTORDER#";
        /// <summary>
        /// Définit la fin de zone de parametre: ordre d'affichage de la page.
        /// </summary>
        private const string HtmlParamSortOrderEnd = "#!SORTORDER#";

        /// <summary>
        /// Permet de définir les pages présentes d'origines sur le site, non ordonnables et non supprimables.
        /// (zone de départ)
        /// </summary>
        private const string HtmlParamSystemBegin = "#SYSTEM#";
        /// <summary>Permet de définir les pages présentes d'origines sur le site, non ordonnables et non supprimables.
        /// (zone de fin)
        /// </summary>
        private const string HtmlParamSystemEnd = "#!SYSTEM#";
        /// <summary>
        /// Nom du site.
        /// </summary>
        public const string SiteName = "Osmosource";
        /// <summary>
        /// Renvoie la liste des informations et des pages complètes. La liste est rangée selon SortOrder.
        ///<para>old: ne renvoie que les noms des pages(comprendre nom des fichiers) car ne lit pas les fichiers.</para> 
        /// Ne renvoie que les pages non brouillons et non systèmes. (au niveau des noms de fichiers, pas des contenu définis)
        /// Nettoie les noms avant de les renvoyer: enlève la langue et le .html
        /// </summary>
        /// <returns></returns>
        public static GenericPageModel[] GetAllPages()
        {
            string path = new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageFolderPath);
            string[] li = Directory.GetFiles(path).Where(p => p.EndsWith("html")).ToArray();
            GenericPageModel[] re = new GenericPageModel[li.Length];
            for (int i = 0; i < re.Length; i++)
            {
                GenericPageModel g = ReadHtmlData(li[i]);
                if (g.IsDraft || g.IsSystem) continue;
                re[i] = g;
            }
            return re.Where(n => n != null).OrderBy(p => p.SortOrder).ToArray();
            //return Directory.GetFiles(path).Where(s =>!s.Contains(PageDraft)).Select(s => s.Replace(path, "")).ToArray();
        }
        private static IEnumerable<string> GetAllPagesNames()
        {
            string path = new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageFolderPath);
            return Directory.GetFiles(path).Where(s => !s.Contains(PageDraft)).Select(s => s.Replace(path, "")).ToArray();
        }
        /// <summary>
        /// renvoie les pages en objets pour l'Administration, modèles génériques des pages.
        /// Renvoie aussi les brouillons ET les pages systèmes.
        /// La liste est rangée dans l'ordre d'affichage choisit.
        /// </summary>
        /// <returns></returns>
        public static List<Models.Admin.GenericPageModel> GetAll()
        {
            List<Models.Admin.GenericPageModel> pages = new List<Models.Admin.GenericPageModel>();
            foreach (string page in GetAllPagesNames())
            {
                if (!page.EndsWith("html")) continue;
                // string name = page.Replace(PageDraft, "").Split('_')[0];// page.Split('_')[0];
                //   StreamReader reader =
                //      new StreamReader(new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageFolderPath + page));
                //    string htmldata = reader.ReadToEnd();
                //     reader.Close();
                //     reader.Dispose();
                pages.Add(Models.Admin.GenericPageModel.From(ReadHtmlData(page, "fr"))/*new Models.Admin.GenericPageModel
                {
                    Name = name,
                    HtmlData = htmldata
                }*/);
            }
            return pages.OrderBy(p => p.SortOrder).ToList();
            /*            return GetAllPages(server).Where(page => page.EndsWith("html")).Select(page => new PagesModel
                                                                      {
                                                                          Name = page.Split('_')[0], //ne récupère que le nom de la page format: [nom]_[lang.html]
                                                                          HtmlData = new StreamReader(server.MapPath("~/Content/data/" + page)).ReadToEnd()
                                                                      }).ToList();
              */
        }
        /// <summary>
        /// Permet de lire le contenu html des fichiers configurables userfriendly.
        /// <para>v0.4: le nom simple peut être passé en parametre, il sera parsé par la suite.</para>
        /// <para>v0.3: vérifie si le fullPathFile est un chemin complet si c'est juste le nom du fichier trouve le dossier complet pour continuer.</para>
        /// <para>voir ReadHtmlData(string filename,string language)</para>
        /// <para>v0.2: Nettoie le name afin d'enlever la langue et le .html si necessaire...</para>
        /// </summary>
        /// <param name="fullPathFile">Nom du fichier qui peut être:
        ///     <para>-Un chemin complet.</para>
        ///     <para>-Un nom de fichier à lire.</para>
        ///     <para>-Un nom convivial qui sera reconstruit en nom de fichier à lire (langue par défaut: fr)</para>
        /// </param>
        /// <returns></returns>
        public static GenericPageModel ReadHtmlData(string fullPathFile)
        {

            Models.Admin.GenericPageModel.Error = null;
            string s, title, name = fullPathFile;
            bool isDraft = false, isReadonly = false, isSystem = false;
            int sortOrder = 0;
            if (!fullPathFile.Contains("_"))
            {
                //partons du principe qu'il manque la langue...
                fullPathFile += "_fr";
            }
            if (!fullPathFile.Contains(".html"))
            {
                fullPathFile += ".html";
            }
            if (!fullPathFile.Contains(":\\"))
            {
                fullPathFile = new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageFolderPath + fullPathFile);

            }
            try
            {
                StreamReader reader =
                    new StreamReader(fullPathFile);
                s = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
                name = title = fullPathFile.Split('\\').Last().Replace(".html", "").Split('_')[0];
                if (s.Contains(HtmlParamIndicatorBegin))
                {
                    int paramBeginIndex = s.IndexOf(HtmlParamIndicatorBegin, StringComparison.Ordinal);
                    int paramEndIndex = s.IndexOf(HtmlParamIndicatorEnd, StringComparison.Ordinal) +
                                        HtmlParamIndicatorEnd.Length;
                    string parameters = s.Substring(paramBeginIndex, paramEndIndex - paramBeginIndex);
                    //ici on devraient avoir récupéré tout les parametres disponibles.
                    //title parameter:
                    int paramIndexStart = parameters.IndexOf(HtmlParamTitleBegin, StringComparison.Ordinal) +
                                          HtmlParamTitleBegin.Length;
                    int paramIndexEnd = parameters.IndexOf(HtmlParamTitleEnd, StringComparison.Ordinal) -
                                        paramIndexStart;
                    title = parameters.Substring(paramIndexStart, paramIndexEnd);
                    //draft parameter:
                    paramIndexStart = parameters.IndexOf(HtmlParamDraftBegin, StringComparison.Ordinal) +
                                      HtmlParamDraftBegin.Length;
                    paramIndexEnd = parameters.IndexOf(HtmlParamDraftEnd, StringComparison.Ordinal) -
                                    paramIndexStart;
                    if (paramIndexEnd > 0)
                    {
                        bool.TryParse(parameters.Substring(paramIndexStart, paramIndexEnd), out isDraft);
                    }
                    //readonly parameter:
                    paramIndexStart = parameters.IndexOf(HtmlParamReadOnlyBegin, StringComparison.Ordinal) +
                                      HtmlParamReadOnlyBegin.Length;
                    paramIndexEnd = parameters.IndexOf(HtmlParamReadOnlyEnd, StringComparison.Ordinal) -
                                    paramIndexStart;
                    if (paramIndexEnd > 0)
                    {
                        bool.TryParse(parameters.Substring(paramIndexStart, paramIndexEnd), out isReadonly);
                    }
                    //sortOrder parameter:
                    paramIndexStart = parameters.IndexOf(HtmlParamSortOrderBegin, StringComparison.Ordinal) +
                                      HtmlParamSortOrderBegin.Length;
                    paramIndexEnd = parameters.IndexOf(HtmlParamSortOrderEnd, StringComparison.Ordinal) -
                                    paramIndexStart;
                    if (paramIndexEnd > 0)
                    {
                        int.TryParse(parameters.Substring(paramIndexStart, paramIndexEnd), out sortOrder);
                    }
                    //System parameter:
                    paramIndexStart = parameters.IndexOf(HtmlParamSystemBegin, StringComparison.Ordinal) +
                                      HtmlParamSystemBegin.Length;
                    paramIndexEnd = parameters.IndexOf(HtmlParamSystemEnd, StringComparison.Ordinal) -
                                    paramIndexStart;
                    if (paramIndexEnd > 0)
                    {
                        bool.TryParse(parameters.Substring(paramIndexStart, paramIndexEnd), out isSystem);
                    }

                    s = s.Replace(parameters, ""); //on supprime de la string finale ces informations la.
                }
            }
            catch (FileNotFoundException e)
            {
                Models.Admin.GenericPageModel.Error = e;
                ErrorManager.SendMailError(e);
                title = "Erreur d'affichage de la page.";
                s = e.Message + "<br/>" +
                    "Votre fichier de contenu n'a pas été trouvé...<br/>Nous nous excusons du désagrément encourut." +
                    "<br/>L'administrateur a été prévenu de l'erreur.";
            }
            catch (IOException e)
            {
                ErrorManager.SendMailError(e);
                title = "Erreur d'affichage de la page";
                s = e.Message + "<br/>" +
                    "Le chemin semblait erroné...<br/>Nous nous excusons du désagrément encourut." +
                    "<br/>L'administrateur a été prévenu de l'erreur.";
            }
            return new GenericPageModel { HtmlData = s, Name = name, Title = title, IsDraft = isDraft, IsReadonly = isReadonly, SortOrder = sortOrder, IsSystem = isSystem };

        }
        /// <summary>
        /// Permet de lire le contenu html des fichiers configurables userfriendly.
        /// <para>v0.3: not needed anymore to parse filename before calling. It is now automatically done.</para>
        /// <para>v0.2: renvoie un genericPageModel.</para>
        /// <para>v0.1: renvoie: Tuple[string,string]: [PageTitle,PageContent]</para>
        /// </summary>
        /// <param name="filename">le nom de la page (nom de l'action dans le controlleur)</param>
        /// <param name="language">la langue désirée (fr pour le moment)</param>
        /// <returns>Un GenericPageModel contenant les informations de la page proprement..</returns>
        public static GenericPageModel /*Tuple<string, string>*/ ReadHtmlData(string filename, string language)
        {
            filename = filename.Replace(PageDraft, "").Split('_')[0];
            return
                ReadHtmlData(
                    new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath("~/Content/pages/" + filename +
                                                                                     "_" + language + ".html"));
        }

        /// <summary>
        /// deprecated
        /// Récupère et génère une page en fonction des paramètres
        /// </summary>
        /// <param name="id">nom du fichier de la page</param>
        /// <param name="lang">langue désirée pour la page</param>
        /// <returns></returns>
        [Obsolete("deprecated: has become useless since version 0.2 of ReadHtmlData")]
        public static GenericPageModel GetPage(string id, string lang)
        {
            return ReadHtmlData(id, lang);
            //Tuple<string, string> data = ReadHtmlData(id, lang);
            //return new GenericPageModel { Title = data.Item1, HtmlContent = data.Item2 };
        }
        /// <summary>
        /// Renvoie une liste de tuple contenant le Nom du fichier et le Titre de la page 
        /// </summary>
        /// <returns>return null since deprecated...</returns>
        [Obsolete("deprecated since new GetPagesList method")]
        public static List<Tuple<string, string>> GetPagesList()
        {/*
            return GetAllPages().Select(
                page =>
                new Tuple<string, string>(page.Split('_')[0], ReadHtmlData(page.Split('_')[0], "fr").Item1)).
                ToList();*/
            return null;
        }
        /// <summary>
        /// Not very clean nor usefull method but replace the deprecated GetPagesList() one.
        /// </summary>
        /// <returns></returns>
        public static List<GenericPageModel> GetPagesListM()
        {
            return GetAllPagesNames().Select(page => ReadHtmlData(page, "fr")).ToList();
        }

        /// <summary>
        /// todo: write in comment header our custom parameters by building a SuperString.
        /// </summary>
        /// <param name="pagesModel"></param>
        /// <returns></returns>
        public static bool WriteHtmlData(Models.Admin.GenericPageModel pagesModel)
        {
            try
            {
                //toute cette partie sert à eliminer le domaine de l'url et renettoyer le code html généré par ckeditor.
                string rebuildHtmlData = "";
                string[] htmlDataSplitted = pagesModel.HtmlData.Split('<');
                foreach (string str in htmlDataSplitted.Where(st => !string.IsNullOrEmpty(st)))
                {
                    string str1 = "<" + str;
                    //modification des url pour les images sinon on ne fait rien.
                    if (str.StartsWith("img"))
                    {
                        int startIndex = str.IndexOf("src=\"", StringComparison.Ordinal);
                        if (str.Contains("http"))
                        {
                            int endIndex = str.IndexOf("/Content/", StringComparison.Ordinal);
                            int startIndexCorrected = startIndex + 5;
                            str1 = "<" + str.Remove(startIndexCorrected, endIndex - startIndexCorrected);
                        }
                    }
                    //ajout de l'élément en cours reconstitué
                    rebuildHtmlData += str1;
                }
                //ecriture et ajout de notre zone de paramètres:
                StringBuilder sb = new StringBuilder();
                sb.Append(HtmlParamIndicatorBegin)
                    .Append(HtmlParamTitleBegin).Append(pagesModel.Title).Append(HtmlParamTitleEnd)
                    .Append(HtmlParamDraftBegin).Append(pagesModel.IsDraft).Append(HtmlParamDraftEnd)
                    .Append(HtmlParamReadOnlyBegin).Append(pagesModel.IsReadonly).Append(HtmlParamReadOnlyEnd)
                    .Append(HtmlParamSortOrderBegin).Append(pagesModel.SortOrder).Append(HtmlParamSortOrderEnd)
                    .Append(HtmlParamSystemBegin).Append(pagesModel.IsSystem).Append(HtmlParamSystemEnd)
                    .Append(HtmlParamIndicatorEnd);
                rebuildHtmlData = sb.Append(rebuildHtmlData).ToString();
                //ecriture finale du fichier complet.
                StreamWriter s = new StreamWriter(new HttpServerUtilityWrapper(HttpContext.Current.Server).MapPath(PageFolderPath + pagesModel.Name + "_fr.html"));
                s.Write(rebuildHtmlData);
                s.Close();
                s.Dispose();
                return true;
            }
            catch (Exception e)
            {
                ErrorManager.SendMailError(e);
                Models.Admin.GenericPageModel.Error = e;
                return false;
            }
        }
        /// <summary>
        /// Allow us to get a "default system page", it will be automatically created and fill blank if not existing.
        /// </summary>
        /// <param name="pageName">the pageName, as in ReadHtmlData can be a full path, a filename, or a friendly name</param>
        /// <returns>The Model of that system page.</returns>
        public static GenericPageModel GetSystemPage(string pageName)
        {
            GenericPageModel model = ReadHtmlData(pageName);
            if (Models.Admin.GenericPageModel.Error != null &&
                Models.Admin.GenericPageModel.Error is FileNotFoundException)
            {
                WriteHtmlData(new Models.Admin.GenericPageModel() { IsDraft = false, IsReadonly = true, IsSystem = true, Name = pageName,HtmlData = ""});

                return GetSystemPage(pageName);
            }
            return model;

        }
    }
}
