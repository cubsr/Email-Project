using System;
using System.IO;

namespace Email_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            String Sender;
            String Reciever;
            String Subject;
            String Body;
            Console.Write("Enter email Sender: ");
            Sender = Console.ReadLine();
            Console.Write("Enter email Reciever: ");
            Reciever = Console.ReadLine();
            Console.Write("Enter email Subject: ");
            Subject = Console.ReadLine();
            Console.Write("Enter email Body: ");
            Body = Console.ReadLine();
            Email.Sendemail(Sender, Reciever, Subject, Body).Wait();

        }
    }
}
