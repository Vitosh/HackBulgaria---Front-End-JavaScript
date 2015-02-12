using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;

class SimpleLogger
{
    public static void Main()
    {
        var log1 = new ConsoleLogger();
        var log2 = new FileLogger();
        var log3 = new HTTPLogger();

        log1.log(1, "Hello World");
        log1.log(2, "Hello World");
        log1.log(3, "Hello Beautiful World");


        log2.log(2, "Hello FileLogger");

        log3.log(3, "Hello HTTP Logger");
    }
}

interface IMyLogger
{
    void log(int iLevel, string sMessage);
}

public class ConsoleLogger : IMyLogger
{
    public void log(int iLevel, string sMessage)
    {
        Console.WriteLine(GenerateString(iLevel, sMessage));
    }

    public string GenerateString(int ilevel, string sMessage)
    {
        string sTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK");
        string sColumns = "::";
        string sLevel = "";

        switch (ilevel)
        {
            case 1:
                sLevel = "INFO";
                break;

            case 2:
                sLevel = "WARNING";
                break;

            case 3:
                sLevel = "PLSCHECKFFS";
                break;

            default:
                break;
        }
        return  sLevel + sColumns + sTime + sColumns + sMessage;
    }
}

class FileLogger : IMyLogger
{
    public void log(int iLevel, string sMessage)
    {
        ConsoleLogger Text = new ConsoleLogger();
        string sMessageInside = Text.GenerateString(iLevel,sMessage);
        File.WriteAllText(@"GivenFile.txt", sMessageInside);
    }
}

class HTTPLogger : IMyLogger
{
    public void log(int iLevel, string sMessage)
    {
        ConsoleLogger Text2 = new ConsoleLogger();
        string postData = Text2.GenerateString(iLevel, sMessage);

        //Taken from:
        //http://stackoverflow.com/questions/1502500/how-to-use-webrequest-to-post-data-and-get-response-from-a-webpage 

        // Create a request using a URL that can receive a post. 
        WebRequest request = WebRequest.Create("http://www.vitoshacademy.com");
        // Set the Method property of the request to POST.
        request.Method = "POST";
        // Create POST data and convert it to a byte array.
        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        // Set the ContentType property of the WebRequest.
        request.ContentType = "application/x-www-form-urlencoded";
        // Set the ContentLength property of the WebRequest.
        request.ContentLength = byteArray.Length;
        // Get the request stream.
        Stream dataStream = request.GetRequestStream();
        // Write the data to the request stream.
        dataStream.Write(byteArray, 0, byteArray.Length);
        // Close the Stream object.
        dataStream.Close();
        // Get the response.
        WebResponse response = request.GetResponse();
        // Display the status.
        //Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        // Get the stream containing content returned by the server.
        dataStream = response.GetResponseStream();
        // Open the stream using a StreamReader for easy access.
        StreamReader reader = new StreamReader(dataStream);
        // Read the content.
        string responseFromServer = reader.ReadToEnd();
        // Display the content.
        //Console.WriteLine(responseFromServer);
        // Clean up the streams.
        reader.Close();
        dataStream.Close();
        response.Close();
    }
}