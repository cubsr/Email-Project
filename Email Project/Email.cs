using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;


namespace Email_Project
{
    public class Email
    {

        public Email()
        {

            string EmailLog = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "EmailLog.txt");
            if (!File.Exists(EmailLog))
            {
                File.Create(EmailLog);
                Console.WriteLine("new file created");
            }
        }


        public static async Task Sendemail(String sender, String recipient, String subject, String body)
        {
            Boolean sent = false;
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = "Email Project.exe.config" };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    //setup SMTP setting from appsettings
                    var smtpClient = new SmtpClient(config.AppSettings.Settings["Mailserver"].Value)
                    {
                        Port = Int32.Parse(config.AppSettings.Settings["Port"].Value),
                        Credentials = new NetworkCredential(sender, config.AppSettings.Settings["Password"].Value),
                        EnableSsl = true,
                    };

                    //send email
                    smtpClient.Send(sender, recipient, subject, body);
                    //break from loop
                    sent = true;
                    i = 3;


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        
            //create a file to log email attempts
            string EmailLog = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "EmailLog.txt");
            if (!File.Exists(EmailLog))
            {
                File.Create(EmailLog);
            }
            //"using" for memory management
            using (StreamWriter logger = new StreamWriter(EmailLog, true))
            {
                //log sender, recipient, subject, email status, and body of mail message
                logger.WriteLine("Sender: {0}", sender);
                logger.WriteLine("Recipient: {0}", recipient);
                logger.WriteLine("Subject: {0}", subject);
                if (sent) { logger.WriteLine("Email Status: Sent"); } else { logger.WriteLine("Email Status: Not sent"); }
                logger.WriteLine("Email contents: {0}", body);
                logger.WriteLine("");
                logger.Close();
            }

            return;

        }
    }
}
