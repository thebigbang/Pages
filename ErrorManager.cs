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
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using CustomPages.Configuration;

namespace CustomPages
{
    class ErrorManager
    {
        private readonly string _email, _messageHtml, _messagePlainText, _sujet, _replyto;
        private bool _timeoutExceed;
        
        /// <summary>
        /// Permet d'envoyer un email automatiquement
        /// </summary>
        /// <param name="email">destinataire</param>
        /// <param name="subject">sujet</param>
        /// <param name="messageContentHtml">contenuHtml</param>
        /// <param name="messageContentPlainText">contenuPlainText</param>
        /// <param name="replyto">email de réponse </param>
        public ErrorManager(string email, string subject, string messageContentHtml, string messageContentPlainText, string replyto = null)
        {
            _sujet = subject;
            _messageHtml = messageContentHtml;
            _messagePlainText = messageContentPlainText;
            _email = email;
            if (replyto != null)
            {
                _replyto = replyto;
            }
            Send();
        }
        private void Send()
        {
            try
            {
                MailMessage email = new MailMessage
                {
                    From = new MailAddress(CustomPagesConfig.Configuration.Parameters.MailFrom),
                    Subject = _sujet,
                    Body = String.Format(_messagePlainText),
                    BodyEncoding = Encoding.UTF8,
                    HeadersEncoding = Encoding.UTF8,
                    SubjectEncoding = Encoding.UTF8
                };
                if (_replyto != null)
                {
                    email.ReplyToList.Add(new MailAddress(_replyto));
                }
                //partie html du message
                AlternateView view = AlternateView.CreateAlternateViewFromString(_messageHtml, Encoding.UTF8,"text/html");
                email.AlternateViews.Add(view);
                //fin partie html du message

                email.ReplyToList.Add(new MailAddress(CustomPagesConfig.Configuration.Parameters.MailGetter));
               
                    email.To.Add(_email);

                    SmtpClient smtp = new SmtpClient(CustomPagesConfig.Configuration.Parameters.SMTPServer, CustomPagesConfig.Configuration.Parameters.SMTPServerPort)
                {
                    Credentials = new NetworkCredential(CustomPagesConfig.Configuration.Parameters.MailFrom, CustomPagesConfig.Configuration.Parameters.MailFromPassword),
                    EnableSsl = false
                };
                bool sendingMail = true;
                new Thread(() =>
                {
                    int timout = 0;
                    while (sendingMail)
                    {
                        Thread.Sleep(500);
                        if (timout > 60)
                        {
                            sendingMail = false;
                            _timeoutExceed = true;
                            throw new TimeoutException("Impossible to send mail due to timeout delay...");
                        }
                        timout++;
                    }
                }).Start();
                smtp.Send(email);
                sendingMail = false;
            }
            catch (TimeoutException toe)
            {
                StreamWriter str = new StreamWriter("c:\\inetpub\\logs\\" + CustomPagesConfig.Configuration.Parameters.SiteName+ "\\errors.log", true);
                str.WriteLine(DateTime.Now + " dest: " + _email + " error: " + toe.Message);
                str.Close();
            }
            catch (Exception e)
            {
                StreamWriter str = new StreamWriter("c:\\inetpub\\logs\\" + CustomPagesConfig.Configuration.Parameters.SiteName + "\\errors.log", true);
                str.WriteLine(DateTime.Now + " dest: " + _email + " error: " + e.Message);
                str.Close();
                if (!_timeoutExceed)
                {
                    LogErrors(e);
                }
            }
        }

        /// <summary>
        /// Envoie un message d'erreur générique contenant le Message d'erreur, la InnerException et la StackTrace en mémoire.
        /// En espérant que tout cela suffise à corriger l'erreur.
        /// </summary>
        /// <param name="e"></param>
        public static void SendMailError(Exception e)
        {
            string errorMsg = "une erreur de requête est survenue sur le site d'" + CustomPagesConfig.Configuration.Parameters.SiteName + ". <br/>" +
                              "Cette erreur se situe au niveau de la bibliothèque \"CustomPages\"<br/>" +
                              "Le message d'erreur est le suivant:." +
                              "<br/>" + e.Message +
                              "<br/>" +
                              "La liste causant l'erreur la suivante:" +
                              "<br/>" + e.InnerException +
                              "<br/>" +
                              "Le contenu de la stackTrace:" +
                              "<br/>" + e.StackTrace +
                              "<br/>" +
                              "<br> Merci bien de vérifier l'authenticitié de la requête.";
            new ErrorManager(CustomPagesConfig.Configuration.Parameters.MailDev, "Erreur survenue sur le site " + CustomPagesConfig.Configuration.Parameters.SiteName + ".", errorMsg, errorMsg);
        }

        /// <summary>
        /// Permet d'envoyer un email lorsqu'une erreur se produit.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="subject"> </param>
        /// <param name="content"> </param>
        public static void LogErrors(Exception e, string subject = "", string content = "")
        {
            string mailData = !String.IsNullOrEmpty(subject) && !String.IsNullOrEmpty(content) ? "<br/>Contenu du mail d'origine:<br/>" : "";
            if (!String.IsNullOrEmpty(subject))
                mailData += "Sujet:<b>" + subject + "</b><br/>";
            if (!String.IsNullOrEmpty(content))
                mailData += "Contenu:<i><b>" + content + "</b></i><br/>";
            string errorMsg = "une erreur de requête est survenue sur le site d'" + CustomPagesConfig.Configuration.Parameters.SiteName + ". <br/>" +
                  "Le message d'erreur est le suivant:." +
                              "<br/>" + e.Message +
                              "<br/>" +
                              "La liste causant l'erreur la suivante:" +
                              "<br/>" + e.InnerException +
                              "<br/>" +
                              "Le contenu de la stackTrace:" +
                              "<br/>" + e.StackTrace +
                              "<br/>" +
                              mailData +
                  "<br> Merci bien de vérifier l'authenticitié de la requête.";
            new ErrorManager(CustomPagesConfig.Configuration.Parameters.MailDev, "Erreur survenue sur le site " + CustomPagesConfig.Configuration.Parameters.SiteName + ".", errorMsg, errorMsg);
        }
    }
}
