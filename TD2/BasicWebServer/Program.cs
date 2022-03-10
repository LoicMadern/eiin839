using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Web;

namespace BasicServerWebServerUrlParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            //if HttpListener is not supported by the Framework
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("A more recent Windows version is required to use the HttpListener class.");
                return;
            }


            // Create a listener.
            HttpListener listener = new HttpListener();

            // Add the prefixes.
            if (args.Length != 0)
            {
                foreach (string s in args)
                {
                    listener.Prefixes.Add(s);
                    // don't forget to authorize access to the TCP/IP addresses localhost:xxxx and localhost:yyyy 
                    // with netsh http add urlacl url=http://localhost:xxxx/ user="Tout le monde"
                    // and netsh http add urlacl url=http://localhost:yyyy/ user="Tout le monde"
                    // user="Tout le monde" is language dependent, use user=Everyone in english 

                }
            }
            else
            {
                Console.WriteLine("Syntax error: the call must contain at least one web server url as argument");
            }
            listener.Start();

            // get args 
            foreach (string s in args)
            {
                Console.WriteLine("Listening for connections on " + s);
            }

            // Trap Ctrl-C on console to exit 
            Console.CancelKeyPress += delegate {
                // call methods to close socket and exit
                listener.Stop();
                listener.Close();
                Environment.Exit(0);
            };


            while (true)
            {
                // Note: The GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerRequest request = context.Request;

                string documentContents;
                using (Stream receiveStream = request.InputStream)
                {
                    using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                    {
                        documentContents = readStream.ReadToEnd();
                    }
                }

                // get url 
                Console.WriteLine($"Received request for {request.Url}");

                //get url protocol
                Console.WriteLine(request.Url.Scheme);
                //get user in url
                Console.WriteLine(request.Url.UserInfo);
                //get host in url
                Console.WriteLine(request.Url.Host);
                //get port in url
                Console.WriteLine(request.Url.Port);
                //get path in url 
                Console.WriteLine(request.Url.LocalPath);

                // parse path in url 
                foreach (string str in request.Url.Segments)
                {
                    Console.WriteLine(str);
                }


                Console.WriteLine(request.Url.Segments[0]);
               
                if (request.Url.Segments.Length >= 2) Console.WriteLine("methode : " + request.Url.Segments[1]);

                //get params un url. After ? and between &

                Console.WriteLine(request.Url.Query);

                //parse params in url
                Console.WriteLine("param1 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param1"));
                Console.WriteLine("param2 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param2"));
                Console.WriteLine("param3 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param3"));
                Console.WriteLine("param4 = " + HttpUtility.ParseQueryString(request.Url.Query).Get("param4"));

                //
                Console.WriteLine(documentContents);

               

                // Obtain a response object.
                HttpListenerResponse response = context.Response;

                // Construct a response.
                //string responseString = "<HTML><BODY> Presentation!</BODY></HTML>";

                Type type = typeof(MyMethods);
             
               
                
                var mymethods = type.GetMethods();

                bool no_problem_param = false; 
                foreach (var mymethod in mymethods)
                {
                    if(mymethod.Name == request.Url.Segments[1])
                    {
                        no_problem_param = true;
                        break;
                    }

                }
                //pour evtier le no param (falcon)
                if (no_problem_param)
                {
                    MethodInfo method = type.GetMethod(request.Url.Segments[1]);
                    Console.WriteLine(method.Name);
                    MyMethods methods = new MyMethods();

                    string result;


                    //exo 3 a besoin d'un param pour cela on selectionne uniquement
                    string param1 = HttpUtility.ParseQueryString(request.Url.Query).Get("param1");
                    Console.WriteLine(param1);

                    if (HttpUtility.ParseQueryString(request.Url.Query).Count > 1)
                    {
                        string param2 = HttpUtility.ParseQueryString(request.Url.Query).Get("param2");
                        result = (string)method.Invoke(methods, new object[] { param1, param2 });
                    }
                    else
                    {
                        result = (string)method.Invoke(methods, new object[] { param1 });
                    }

                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(result);
                    // Get a response stream and write the response to it.
                    response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // You must close the output stream.
                    output.Close();

                    Console.WriteLine(result);
                }
               


            }
            // Httplistener neither stop ... But Ctrl-C do that ...
            // listener.Stop();
        }
    }
}

public class MyMethods
{
    //Exercice 1
    public string MyMethod(string param1, string param2)
    {
        return "<html><body > Bonjour je m'apelle " + param1 + " " +  param2 + " </body></html>";
        
    }

    //Exercice 2
    public string CallAnExecutable(string param1, string param2)
    {
     
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = @"C:\Users\Loic\Documents\Polytech\S8\soc\eiin839\TD2\ExecTest\bin\Debug\ExecTest.exe"; // Specify exe name.
        start.Arguments = param1 + " " + param2; // Specify arguments.
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;
        //
        // Start the process.
        //
        using (Process process = Process.Start(start))
        {
            //
            // Read in all the text from the process with the StreamReader.
            //
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }

    public class NewValue
    {
        public int value { get; set; }
    }


    //Exercice 3
    public string incr(string value)
    {
        int myInt = int.Parse(value);
        myInt ++;
        var newValueIncr = new NewValue
        {
            value = myInt
        };
        Console.WriteLine("my int incr :" + myInt);
        
        string content = JsonSerializer.Serialize(newValueIncr);
        return content;
    }
}
