﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace blabla
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("To create server press 0");
            Console.WriteLine("To connect press 1");
            int input = Convert.ToInt32(Console.ReadLine());
            switch (input)
            {
                case 0:
                    server();
                    break;
                case 1:
                    client();
                    break;
                default:
                    Console.WriteLine("Option not available");
                    break;

            }
        }
        static void server()
        {
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());
            TcpListener server = new TcpListener(IPAddress.Any, port);

            try
            {
                server.Start();
                TcpClient client = server.AcceptTcpClient();
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                StreamReader sr = new StreamReader(client.GetStream());
                Thread readThread = new Thread(() => readSocket(sr));
                readThread.Start();
                string message = "";
                while (message != "exit bla")
                {
                    message = Console.ReadLine();
                    sw.WriteLine(message);
                }
                client.Close();
                return;
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        static void client()
        {
            TcpClient client = new TcpClient();

            Console.Write("IP: ");
            string ip = Console.ReadLine();
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());

            try
            {
                client.Connect(ip, port);
                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                StreamReader sr = new StreamReader(client.GetStream());
                Thread readThread = new Thread(() => readSocket(sr));
                readThread.Start(); //Run readSocket() at the same time
                string message = "";
                while (message != "exit")
                {
                    message = Console.ReadLine();
                    sw.WriteLine(message);
                }
                client.Close();
                return;
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }
        static void readSocket(StreamReader sr)
        {
            try
            {
                string message = "";
                while ((message = sr.ReadLine()) != null)
                {
                    Console.WriteLine(message);
                }
            }
            catch (System.IO.IOException) { /*when we force close, this goes off, so ignore it*/ }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}
