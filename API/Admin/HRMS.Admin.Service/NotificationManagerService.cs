using System;
using System.Configuration;
using System.Net.Mail;
using HRMS.Admin.Infrastructure.Models;
using HRMS.Admin.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Net.Mime;
using System.IO;
using HRMS.Admin.Infrastructure;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using AWSMailer;
using NLog.Web;

namespace HRMS.Admin.Service
{
    public class NotificationManagerService : INotificationManagerService
    {
        #region Global variables
        private readonly IConfiguration m_configuration;
        private readonly ILogger<NotificationManagerService> m_Logger;
        private readonly EmailConfigurations m_EmailConfigurations;
        #endregion

        #region NotificationManagerService
        public NotificationManagerService(ILogger<NotificationManagerService> logger,
                                          IConfiguration configuration,
                                          IOptions<EmailConfigurations> emailConfigurations)
        {
            m_Logger = logger;
            m_configuration = configuration;
            m_EmailConfigurations = emailConfigurations.Value;
        }
        #endregion

        #region Send Email
        /// <summary>
        /// This method sends Email
        /// </summary>
        /// <param name="notificationDetail"></param>
        /// <returns>dynamic</returns>

        public async Task SendEmail(NotificationDetail notificationDetail)
        {
           
          m_Logger.LogInformation("NotificationManagerService: \"SendEmail\" ");
            MailMessage objMailMessage = new MailMessage();
            SmtpClient objSmtpClient = new SmtpClient();
            try
            {
                LinkedResource linked = null;
                LinkedResource logoImage = null;
                string smtpClientaddress = m_EmailConfigurations.SMTPClient;
                objMailMessage.From = new MailAddress(notificationDetail.FromEmail);

                //objMailMessage.To.Add(new MailAddress(ToAddress));
                string[] toAddresses = !string.IsNullOrEmpty(notificationDetail.ToEmail) ? notificationDetail.ToEmail.Split(';') : new string[] { };
                if (toAddresses.Length > 0)
                {
                    var distinctToAddress = toAddresses.Distinct().ToArray();
                    foreach (string address in distinctToAddress)
                    {
                        if (address != string.Empty)
                            objMailMessage.To.Add(address);
                    }
                }

                //objMailMessage.CC.Add(new MailAddress(ccAddress));
                if (notificationDetail.CcEmail != null)
                {
                    notificationDetail.CcEmail = notificationDetail.CcEmail.TrimEnd(';');
                }
                string[] addresses = !string.IsNullOrEmpty(notificationDetail.CcEmail) ? notificationDetail.CcEmail.Split(';') : new string[] { };
                if (addresses.Length > 0)
                {
                    var distinctCCAddress = addresses.Distinct().ToArray();
                    foreach (string address in distinctCCAddress)
                    {
                        if (address != string.Empty)
                             objMailMessage.CC.Add(address);
                    }
                   
                }
                if(notificationDetail.Attachments.Count>0)
                {
                    notificationDetail.Attachments.ForEach(attatchment =>
                    {
                        objMailMessage.Attachments.Add(new Attachment(attatchment));
                    }
                    );
                }
               
                if(notificationDetail.InlineFilePath != null)
                {
                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(notificationDetail.EmailBody, null, "text/html");
                    linked = new LinkedResource(notificationDetail.InlineFilePath, MediaTypeNames.Image.Jpeg);
                    linked.ContentId = "sample";
                    logoImage = new LinkedResource(m_EmailConfigurations.LogoPath, MediaTypeNames.Image.Jpeg);
                    logoImage.ContentId = "sample1";
                    alternateView.LinkedResources.Add(linked);
                    alternateView.LinkedResources.Add(logoImage);
                    objMailMessage.AlternateViews.Add(alternateView);
                }  
                
                objMailMessage.Subject = notificationDetail.Subject;
                objMailMessage.IsBodyHtml = true;
                objMailMessage.Body = notificationDetail.EmailBody;

                objSmtpClient = new SmtpClient(smtpClientaddress);
                objSmtpClient.UseDefaultCredentials = false;
                objSmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (Convert.ToBoolean(m_EmailConfigurations.IsSendMail))
                {
                    m_Logger.LogInformation("Sending email");
                    //objSmtpClient.Send(objMailMessage);
                    await Mailer.SendEmail(objMailMessage);
                    linked?.Dispose();
                    logoImage?.Dispose();
                }
            }
            catch (Exception ex)
            {
                m_Logger.LogError("Error occurred while sending email" + ex.StackTrace);
            }
        }

        #endregion
    }
}
