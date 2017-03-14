using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.VisualBasic.FileIO;
namespace NameSort
{
    public class Program
    {
        //write the final sorted List to a file
        public static String[] WriteList(List<String[]> fileContent, String arg)
        {
            String[] r = new String[3];
            try
                {
                    //Append -sorted to the file path before the extension name before saving - files without an extension are not supported
                    string filePath = @"" + arg.Substring(0,arg.LastIndexOf(".")) + "-sorted" + arg.Substring(arg.LastIndexOf("."));  
                    String result = "";
                    foreach(String[] f in fileContent)
                    {
                        result += f.GetValue(0) + ", " + f.GetValue(1) + '\r' + '\n';
                    }

                    //remove the last line break
                    result = result.Substring(0, result.Length - 2);
                    File.WriteAllText(filePath, result);
                    r.SetValue("Success",0);
                    r.SetValue(filePath, 1);
                    r.SetValue(result, 2);
                }
            catch
            {
                r.SetValue("FailWrite",0);
            }
            return r;
        }

        //sort the contents of the List object by last name then by first name
        public static List<String[]> SortList(List<String[]> fileContent)
        {
            //Sort by first name initially
            fileContent.Sort((a1, a2) => a1.GetValue(1).ToString().CompareTo(a2.GetValue(1).ToString()));
            //Then by last name
            fileContent.Sort((a1, a2) => a1.GetValue(0).ToString().CompareTo(a2.GetValue(0).ToString()));
            return fileContent;
        }

        //read the entered file name's contents into a variable
        public static List<String[]> ReadFile(string arg)
        {
            try
            {
                String[] r = new String[2];
                List<String[]> fileContent = new List<string[]>();
                using (FileStream reader =
                            File.OpenRead(@"" + arg))
                using (TextFieldParser parser = new TextFieldParser(reader))
                {
                    // we want to trim the white space in case there is a peculiarity in the entered format
                    parser.TrimWhiteSpace = true;
                    parser.Delimiters = new[] { "," };
                    //Supporting fields enclosed in quotes in case this is required
                    parser.HasFieldsEnclosedInQuotes = true;
                    while (!parser.EndOfData)
                    {
                        string[] line = parser.ReadFields();
                        fileContent.Add(line);
                    }
                }
                return fileContent;
            }
            catch
            {
                return null;
            }
        }

        //primary handler function to call other functions
        public static string[] RunSort(string arg)
        {
            String[] r = new String[3];
            List<String[]> fileContent = new List<string[]>();
            fileContent = ReadFile(arg);
            if(fileContent == null)
            {
                r.SetValue("FailRead",0);
            }
            else
            {
                fileContent = SortList(fileContent);
                String[] result = WriteList(fileContent, arg);
                return result;
            }
            return r;
         }

        //Main function to take command line arguments and write back messages to the user
        static void Main(string[] args)
        {
            //User needs to supply one argument and one argument only
            if (args.Length != 1)
            {
                System.Console.WriteLine("Please provide one parameter, representing the file name, to the application, e.g. C:\\sort.txt - you have provided {0} parameters.", args.Length);
            }
            else
            {
                String arg = args.GetValue(0).ToString();
                String[] result  = RunSort(arg);
                if(result.GetValue(0).ToString() == "Success")
                {
                    System.Console.WriteLine("File successfully sorted and saved to " + result.GetValue(1));
                }
                else if(result.GetValue(0).ToString() == "FailRead")
                {
                    System.Console.WriteLine("There was an error opening the provided file - please ensure you have provided a valid file path, the application has access to it and the file is comma-separated in the format FirstName, LastName");
                }
                else if(result.GetValue(0).ToString() == "FailWrite")
                {
                    System.Console.WriteLine("An error occurred writing the final sorted file - please ensure the application has permission to write to the directory it read the file from.");
                }
            }
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }
}
