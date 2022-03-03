﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Echo
{
    class EchoServer
    {
        [Obsolete]
        static void Main(string[] args)
        {

            Console.CancelKeyPress += delegate
            {
                System.Environment.Exit(0);
            };

            TcpListener ServerSocket = new TcpListener(5000);
            ServerSocket.Start();

            Console.WriteLine("Server started.");
            while (true)
            {
                TcpClient clientSocket = ServerSocket.AcceptTcpClient();
                handleClient client = new handleClient();
                client.startClient(clientSocket);
            }


        }
    }

    public class handleClient
    {
        string HTTP_ROOT = "../../../www/";
        TcpClient clientSocket;
        public void startClient(TcpClient inClientSocket)
        {
            this.clientSocket = inClientSocket;
            Thread ctThread = new Thread(Echo);
            ctThread.Start();
        }



        private void Echo()
        {
            NetworkStream stream = clientSocket.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);

            while (true)
            {

                string str = reader.ReadString();
                Console.WriteLine(str);
                string[] request = str.Split(' ');

                if(request.Length == 2)
                { 
                    if (request[1] == "index.html" || request[1] == "index2.html" && request.Length == 2 && request[0] == "GET")
                    {
              
                        writer.Write("http / 1.0 200 OK");
                        Console.WriteLine(HTTP_ROOT + request[1]);

                        string file = System.IO.File.ReadAllText(HTTP_ROOT + request[1]);
                        Console.WriteLine(file);
                        writer.Write(file);                    }
                    else
                    {
                        Console.WriteLine("the file that you searched does not exist or you made a bad request");
                    }                
                }

                else { Console.WriteLine("Problem with the number of arguments"); }
            
                
            }
        }



    }

}