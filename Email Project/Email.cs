using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Data.SqlClient;

namespace Email_Project
{
    public class Email
    {
        //holds the log file path
        private String logFile;



        //default constructor for no file input
        public Email()
        {

            string EmailLog = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "EmailLog.txt");
            if (!File.Exists(EmailLog))
            {
                File.Create(EmailLog);
                Console.WriteLine("new file created");
            }
            logFile = EmailLog;
        }

        //overloaded constructor if file is given
        public Email(String fileName)
        {

            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            logFile = fileName;

        }

        public async void Sendemail(String sender, String recipient, String subject, String body)
        {
            Boolean sent = false;
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap { ExeConfigFilename = "Email Project.exe.config" };
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    
                       var smtpClient = new SmtpClient(config.AppSettings.Settings["Mailserver"].Value)
                    {
                        Port = Int32.Parse(config.AppSettings.Settings["Port"].Value),
                        Credentials = new NetworkCredential(sender, config.AppSettings.Settings["Password"].Value),
                        EnableSsl = true,
                    };
                    
                    smtpClient.Send("email", "recipient", "subject", "body");
                    sent = true;
                    i = 3;
                    

                } catch(Exception e)
                {
                    //Console.WriteLine(e);
                }
            }
            //"using" for memory management
            using (StreamWriter logger = new StreamWriter(logFile, true))
            {
                logger.WriteLine("Sender: {0}", sender);
                logger.WriteLine("Recipient: {0}", recipient);
                logger.WriteLine("Subject: {0}", subject);
                if (sent) { logger.WriteLine("Email Status: Sent"); } else { logger.WriteLine("Email Status: Not sent"); }
                logger.WriteLine("Email contents: {0}", body);
                logger.WriteLine("");
                logger.Close();
            }

        }
    }
}
