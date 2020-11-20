using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Infatlan_AuditControl.classes
{
    public enum typeBody{
        Informe,
        HallazgoCreado,
        HallazgoAsignacion,
        Solicitud,
        General
    }

    public class SmtpService : Page{

        public SmtpService() { }

        public Boolean EnviarMensaje(String To, typeBody Body, String UsuarioPara, String NombreAccion, String Copy){
            Boolean vRespuesta = false;
            try{
                MailMessage mail = new MailMessage("Auditoria Interna<" + ConfigurationManager.AppSettings["SmtpFrom"] + ">", To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                if(!Copy.Equals(""))
                    mail.CC.Add(Copy);
                mail.Subject = "Auditoria - Sistema de informes";
                mail.IsBodyHtml = true;

                switch (Body){
                    case typeBody.Informe:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            UsuarioPara,
                            "Se ha creado el informe: " + NombreAccion,
                            ConfigurationManager.AppSettings["Host"] + "/pages/findingsSearch.aspx",
                            "Te informamos que se ha creado un informe, por favor entrar al portal."
                            ), Server.MapPath("/assets/images/logored.png")));
                        break;
                    
                    case typeBody.HallazgoAsignacion:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            UsuarioPara,
                            NombreAccion,
                            ConfigurationManager.AppSettings["Host"] + "/pages/findingsSearch.aspx",
                            "Cualquier consultar o comentario ponte en contacto con auditoria o por favor entrar al portal."
                            ), Server.MapPath("/assets/images/logored.png")));
                        break;
                    case typeBody.General:
                        mail.AlternateViews.Add(CreateHtmlMessage(PopulateBody(
                            UsuarioPara,
                            NombreAccion,
                            ConfigurationManager.AppSettings["Host"] + "/pages/findingsSearch.aspx",
                            "Cualquier consultar o comentario ponte en contacto con auditoria o por favor entrar al portal."
                            ), Server.MapPath("/assets/images/logored.png")));
                        break;
                }
                //client.Send(mail);
                vRespuesta = true;
            }catch (System.Net.Mail.SmtpException Ex){
                String vError = Ex.Message;
                throw;
            }catch (Exception Ex){
                throw;
            }
            return vRespuesta;
        }

        public Boolean EnviarMensaje(String To, String Body, String Asunto){
            Boolean vRespuesta = false;
            try{
                MailMessage mail = new MailMessage("Correo test<" + ConfigurationManager.AppSettings["SmtpFrom"] + ">", To);
                SmtpClient client = new SmtpClient();
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                client.UseDefaultCredentials = false;
                client.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mail.Subject = Asunto;
                mail.IsBodyHtml = true;
                mail.Body = Body;

                //client.Send(mail);
            }catch (System.Net.Mail.SmtpException Ex){
                String vError = Ex.Message;
                throw;
            }catch (Exception Ex){
                throw;
            }
            return vRespuesta;
        }

        private AlternateView CreateHtmlMessage(string message, string logoPath)
        {
            var inline = new LinkedResource(logoPath, "image/png");
            inline.ContentId = "companyLogo";

            var alternateView = AlternateView.CreateAlternateViewFromString(
                                    message,
                                    Encoding.UTF8,
                                    MediaTypeNames.Text.Html);
            alternateView.LinkedResources.Add(inline);

            return alternateView;
        }

        public string PopulateBody(string vNombre, string vTitulo, string vUrl, string vDescripcion){
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/pages/mail/TemplateMail.html"))){
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Host}", ConfigurationManager.AppSettings["Host"]);
            body = body.Replace("{Nombre}", vNombre);
            body = body.Replace("{Titulo}", vTitulo);
            body = body.Replace("{Url}", vUrl);
            body = body.Replace("{Descripcion}", vDescripcion);
            return body;
        }
    }

    public class Correo{
        public String Usuario { get; set; }
        public String Para { get; set; }
        public String Copia { get; set; }
    }
}