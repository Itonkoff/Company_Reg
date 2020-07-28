using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Company_Reg.Helpers
{
    public class CommunicationHelper
    {
        public bool EmailAddressSent = false;
        public void SendEmail()
        {
            SmtpClient client = new SmtpClient("mail.ttcsglobal.com");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("companiesonlinezw", "N3wPr0ducts@1");
            // client.Credentials = new NetworkCredential("username", "password");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("bkofu@ttcsglobal.com");
            mailMessage.To.Add("kofubrighton@gmail.com");
            mailMessage.Subject = "TEST EGOV";
            mailMessage.Body = "This is a test email";
            try
            {
                client.Send(mailMessage);
                EmailAddressSent = true;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task SendSMS(string message, string destinations)
        {
            string username = "obiematan";
            var client = new HttpClient();

            // Webservices token for above Webservice username
            string token = "72cceb3c5553eb552d0314b227395c61";

            // BulkSMS Webservices URL
            string bulksms_ws = "http://portal.bulksmsweb.com/index.php?app=ws";
            // destination numbers, comma seperated or use #groupcode for sending to group
            // $destinations = '#devteam,263071077072,26370229338';
            // $destinations = '26300123123123,26300456456456';  for multiple recipients
            
            // SMS Message to send
            
            // send via BulkSMS HTTP API

            string ws_str = bulksms_ws + "&u=" + username + "&h=" + token + "&op=pv";
            ws_str += "&to=" + Uri.EscapeDataString(destinations) + "&msg=" + Uri.EscapeDataString(message);

            HttpResponseMessage response = await client.GetAsync(ws_str);

            response.EnsureSuccessStatusCode();

            using (HttpContent content = response.Content)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody + "........");
            }
        }
    }
}
