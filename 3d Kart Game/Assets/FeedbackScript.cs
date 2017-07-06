using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using GameAnalyticsSDK;

public class FeedbackScript : MonoBehaviour {

	public static void sendEmail (string feedbackText)
	{
		MailMessage mail = new MailMessage();

		mail.From = new MailAddress("innovationward@gmail.com");
		mail.To.Add("InnovationWard@outlook.com");
		mail.Subject = "App Feedback";
		mail.Body = feedbackText;

		SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
		smtpServer.Port = 587;
		smtpServer.Credentials = new System.Net.NetworkCredential("innovationward@gmail.com", "JrmfH9rbTKhr") as ICredentialsByHost;
		smtpServer.EnableSsl = true;
		smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
		ServicePointManager.ServerCertificateValidationCallback = 
			delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) 
		{ return true; };
		try{
			smtpServer.Send(mail);
		} catch ( Exception ex) {
			GameAnalytics.NewErrorEvent (GAErrorSeverity.Error, ex.Message);
		}
	}
}