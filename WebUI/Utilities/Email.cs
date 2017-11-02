using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebUI.Utilities
{
    public class Email
    {

        //ConfigurationManager es utilizado para leer del web.config. En este caso leemos de appSettings
        private static string appSMTPServer = ConfigurationManager.AppSettings["AppSMTPServer"];
        private static int appSMTPPort = int.Parse(ConfigurationManager.AppSettings["AppSMTPPort"]);
        private static string appEmailFrom = ConfigurationManager.AppSettings["AppEmailFrom"];
        private static string appEmailFromName = ConfigurationManager.AppSettings["AppEmailFromName"];
        private static string appEmailFromPassword = ConfigurationManager.AppSettings["appEmailFromPassword"];

        /// <summary>
        /// Envia un correo a traves de una cuenta de Gmail.
        /// </summary>
        /// <param name="toEmail">Email del destinatario</param>
        /// <param name="toName">Nombre del destinatario</param>
        /// <param name="subject">Asunto del mail</param>
        /// <param name="body">Cuerpo o Mensaje del mail</param>
        public static void Enviar(string toEmail, string toName, string subject, string body)
        {
            var fromAddress = new MailAddress(appEmailFrom, appEmailFromName);
            var toAddress = new MailAddress(toEmail, toName);

            //crea la instancia SMTP de Gmail, autenticada con cuenta valida.
            //Recordar permitir acceso en GMAIL.
            var smtp = new SmtpClient
            {
                Host = appSMTPServer,
                Port = appSMTPPort,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, appEmailFromPassword)
            };

            //Using es usado para utilizar un bloque de código que debe ser liberado al finalizar..
            //Mas info en http://www.variablenotfound.com/2009/02/usando-using-valga-la-redundancia-c.html
            using (var message = new MailMessage(fromAddress, toAddress))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                smtp.Send(message); //usa la instancia SMTP, y le dice Enviar..
            }
        }

    }
}