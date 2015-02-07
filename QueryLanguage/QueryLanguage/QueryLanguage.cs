using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

class CSV_query_language
{
    public static string[] sCommandActivate;
    public static string[] sCommandDetails;

    public const string SHOW = "show";
    public const string SUM = "sum";
    public const string FIND = "find";
    public const string SELECT = "select";
    public const string LIMIT = "limit";

    static void Main()
    {
        try
        {
            string sCommand = Console.ReadLine();
            sCommandActivate = sCommand.Split('>');
            sCommandDetails = sCommandActivate[1].Split(' ');

            if (sCommandDetails.Contains(SHOW))
            {
                ShowHeader();
            }

            if (sCommandDetails.Contains(SUM))
            {
                SumValues();
            }

            if (sCommandDetails.Contains(FIND))
            {
                FindTheResults();
            }
            if (sCommandDetails.Contains(SELECT))
            {
                ShowMeWhatYouGotGeneral();
            }
            Console.WriteLine("\nThank you for using the cusom query language!\nPress Y for a new query.");
            string sNewQuery = Console.ReadLine();
            if (sNewQuery == "Y" || sNewQuery == "y")
            {
                Main();
            }
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            Console.WriteLine("\nAn error has occurred. Try again!");
            Console.WriteLine("Possible queries:\n");
            Console.WriteLine("query> show");
            Console.WriteLine("query> sum id");
            Console.WriteLine("query> select id, name, age limit 5");
            Console.WriteLine("query> select id, experience");
            Console.WriteLine("query> find \"-\"");
            Console.WriteLine("Please, enter a query and enjoy the result:\n");
            Main();
        }
    }
    static void ShowMeWhatYouGotGeneral()
    {

        string file1 = @"New Text Document.csv";
        string[] allLines = File.ReadAllLines(file1);

        if (sCommandDetails.Contains(LIMIT))
        {
            SetALimit();
        }
        else
        {
            ShowMeWhatYouGot();
        }
    }
    static void FindTheResults()
    {
        string file1 = @"New Text Document.csv";
        string[] allLines = File.ReadAllLines(file1);
        int iLastValue = sCommandDetails.Length - 1;

        string sStringToSearch = sCommandDetails[iLastValue];
        sStringToSearch = sStringToSearch.Replace("\"", "");

        string sBeautiful = "";
        Boolean first = true;
        int iItem = 0;
        string[] display;
        string sDisplayResult;
        string[] getColumnName = allLines[0].Split(',');

        sBeautiful = new String('*', 20 * allLines.Length - 1);
        Console.WriteLine(sBeautiful);

        for (int i = 0; i < getColumnName.Length; i++)
        {
            sDisplayResult = getColumnName[i];
            Console.Write(string.Format("| {0,-20}", sDisplayResult));
        }
        Console.WriteLine();
        Console.WriteLine(sBeautiful);
        first = true;
        Boolean newLineNeeded = false;

        foreach (var item in allLines)
        {
            if (first)
            {
                first = false;
                continue;
            }
            if (item.Contains(sStringToSearch))
            {
                iItem = item.Split(',').Length;
                display = item.Split(',');

                for (int i = 0; i < iItem; i++)
                {
                    sDisplayResult = display[i];
                    Console.Write(string.Format("| {0,-20}", sDisplayResult));
                    newLineNeeded = true;
                }
                if (newLineNeeded)
                {
                    Console.WriteLine();
                }
                newLineNeeded = false;
            }
        }
        Console.WriteLine(sBeautiful);
    }

    static void ShowMeWhatYouGot()
    {   

        int iItem = 0;
        string sDisplayResult = "";
        Boolean first = true;
        string sBeautiful = "";
        string[] display;
        string Translator2 = "";

        string file1 = @"New Text Document.csv";
        string[] allLines = File.ReadAllLines(file1);

        string sArrDisplay2 = String.Join(",", sCommandDetails, 1, sCommandDetails.Length - 1);
        string[] sArrDisplay = sArrDisplay2.Split(',');

        int iCountColumns = sCommandDetails.Length - 2;
        string[] getColumnName = allLines[0].Split(',');

        foreach (string item in allLines)
        {
            iItem = item.Split(',').Length;
            display = item.Split(',');
            for (int i = 0; i < iItem; i++)
            {
                if (sArrDisplay.Contains(display[i]))
                {
                    Translator2 = i + "," + Translator2;
                }
            }
        }
        
        //Removing the word SHOW from the Translator2
        Translator2 = Translator2.Remove(Translator2.Length - 1);

        foreach (string item in allLines)
        {
            sBeautiful = new String('*', 20 * Translator2.Length - 1);
            if (first)
            {
                Console.WriteLine(sBeautiful);
            }

            iItem = item.Split(',').Length;
            display = item.Split(',');

            for (int i = 0; i < iItem; i++)
            {
                if (Translator2.Contains(i.ToString()))
                {
                    sDisplayResult = display[i];
                    Console.Write(string.Format("| {0,-20}", sDisplayResult));
                }
            }

            Console.WriteLine();
            if (first)
            {
                Console.WriteLine(sBeautiful);
                first = false;
            }

        }
        Console.WriteLine(sBeautiful);
    }

    static void ShowHeader()
    {
        string file1 = @"New Text Document.csv";
        StreamReader sReader = new StreamReader(file1, Encoding.GetEncoding("Windows-1251"));
        Console.WriteLine(sReader.ReadLine());
    }

    static void SumValues()
    {
        string file1 = @"New Text Document.csv";
        string[] allLines = File.ReadAllLines(file1);

        int iLastValue = sCommandDetails.Length - 1;
        int iResult = -1;
        int iCounter = -1;

        string sColumnToWork = sCommandDetails[iLastValue];
        string[] getColumnName = allLines[0].Split(',');

        foreach (var item in getColumnName)
        {
            iCounter++;
            if (item == sColumnToWork)
            {
                iResult = iCounter;
            }
        }

        int iCountColumns = allLines[0].Split(',').Length;
        string sGetColumnThatWeAreTalkingAboutName = getColumnName[iResult];
        string sFirstLine = "";

        //Setting the header to look like: 0,0,0,0... etc, in order to be excluded from the sum:
        if (iCountColumns > 1)
        {
            for (int i = 0; i < iCountColumns - 1; i++)
            {
                sFirstLine = sFirstLine + "0,";
            }
            sFirstLine = sFirstLine + "0";
        }
        else
        {
            sFirstLine = "0";
        }

        allLines[0] = sFirstLine;

        IEnumerable<string> strs = allLines;
        var columnQuery =
            from line in strs
            let elements = line.Split(',')
            select Convert.ToInt32(elements[iResult]);

        var results = columnQuery.ToList();
        double dSum = results.Sum();
        Console.WriteLine("\nThe sum of column [{0}] is  {1:0.##}!", sGetColumnThatWeAreTalkingAboutName, dSum);
    }

    static void SetALimit()
    {
        Boolean first = true;
        int iItem = 0;
        string sDisplayResult = "";
        string sBeautiful = "";
        string Translator2 = "";
        int z = -2;

        string file1 = @"New Text Document.csv";
        string[] allLines = File.ReadAllLines(file1);
        string sArrDisplay2 = String.Join(",", sCommandDetails, 1, sCommandDetails.Length - 1);
        string[] sArrDisplay = sArrDisplay2.Split(',');

        int Limit = int.Parse(sArrDisplay[sArrDisplay.Length - 1])-1;

        sArrDisplay = sArrDisplay.Reverse().Skip(2).Reverse().ToArray();
        int iCountColumns = sCommandDetails.Length - 2;

        string[] getColumnName = allLines[0].Split(',');
        string[] display;


        foreach (string item in allLines)
        {
            iItem = item.Split(',').Length;
            display = item.Split(',');
            for (int i = 0; i < iItem; i++)
            {
                if (sArrDisplay.Contains(display[i]))
                {
                    Translator2 = i + "," + Translator2;
                }
            }
        }

        //We remove the last comma from the string here:
        Translator2 = Translator2.Remove(Translator2.Length - 1);

        foreach (string item in allLines)
        {
            z++;
            if (z > Limit)
            {
                break;
            }

            sBeautiful = new String('*', 20 * Translator2.Length - 1);
            if (first)
            {
                Console.WriteLine(sBeautiful);
            }

            iItem = item.Split(',').Length;
            display = item.Split(',');

            for (int i = 0; i < iItem; i++)
            {
                if (Translator2.Contains(i.ToString()))
                {
                    sDisplayResult = display[i];
                    Console.Write(string.Format("| {0,-20}", sDisplayResult));
                }
            }
            Console.WriteLine();
            if (first)
            {
                Console.WriteLine(sBeautiful);
                first = false;
            }
        }
        Console.WriteLine(sBeautiful);
    }
}