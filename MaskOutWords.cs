using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;

class MAskOutWords
{
    static void Main()
    {
        string sPattern = "";
        string sInitial = "Vitosh is my name, football is my favourite game.\nWhat is your name? \nHow do you do? Are you from Sofia or from Berlin? Lalla - lala.";
        string sResult = sInitial;
        List<string> lToChange = new List<string>(new string [] {"my","game","is","do","Berlin","Gotheborg","JustSomethingNotAvailable","lala"});

        foreach (string item in lToChange)
        {
            sPattern = new string('*', item.Length);
            sResult = Regex.Replace(sInitial, item, sPattern);
            sInitial = sResult;
        }

        Console.WriteLine(sResult);

    }
}