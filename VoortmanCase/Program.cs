using System.Data;
using System.IO;

namespace VoortmanCase
{
    internal class Program
    {
        static void Main(string[] args)
        {

            List<string> files = new List<string>();
            files.Add("socker.dat");
            files.Add("temperature.dat");

            //DataTable file1 = ReadDataFile("C:\\Users\\Max\\Documents\\Werk\\solliciteren\\VoortmanCase\\VoortmanCase\\temperature.dat"); 
            DataTable file1 = ReadDataFile("C:\\Users\\Max\\Documents\\Werk\\solliciteren\\VoortmanCase\\VoortmanCase\\temperatureCopy.dat"); //kleine cheats met verwijderen header en aanpassen duplicate mxd
            //DataTable file2 = ReadDataFile("C:\\Users\\Max\\Documents\\Werk\\solliciteren\\VoortmanCase\\VoortmanCase\\socker.dat");
            DataTable file2 = ReadDataFile("C:\\Users\\Max\\Documents\\Werk\\solliciteren\\VoortmanCase\\VoortmanCase\\sockerCopy.dat"); //kleine cheat met aanpassen duplicate P

            Console.WriteLine("smallest temperature spread occured on day " + SmallestTemperatureSpread(file1));
            // regel hier met output socker.dat
        }
        
        static DataTable ReadDataFile(string filePath)
        {
            DataTable fileData = new DataTable(); //werken met datatable leek het handigst

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    int k = 0; // row iterator, gebruikt om eerste regel te onderscheiden van de rest
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == "") { continue; }; //skip als rij leeg is
                        string[] words = line.Split(" "); //gebruik de witregel als delimiter

                        // first loop is column names
                        if (k == 0)
                        {
                            foreach (string word in words) //uitgesplitse string testen voor content
                            {
                                if (word == "")
                                {
                                    continue;
                                }
                                else
                                {
                                    fileData.Columns.Add(word);
                                }
                            }
                        }
                        else // de rest van de rijen worden toegevoegd aan de datatable
                        {   
                            DataRow row = fileData.NewRow();
                            int n = 0; //column iterator
                            foreach (string word in words)
                            {
                                if (word == "") //uitgesplitse string testen voor content
                                {
                                    continue;
                                }
                                else
                                {
                                    row[n] = word; //niet super robuust
                                    n += 1;
                                }
                            }
                            fileData.Rows.Add(row); //niet super robuust
                        }

                        k += 1;

                    }
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            

            return fileData;
        }

        static string SmallestTemperatureSpread(DataTable fileData)
        {
            string smallestDay = "unknown";
            float smallestDifference = float.MaxValue;

            for (int i = 0; i < fileData.Rows.Count; i++) //loop door alle rijen heen en analyseer temperatuur verschil
            {
                DataRow row = fileData.Rows[i];

                // only works if values are proper integers, so for now wrap in exception block which skips non-valid entries
                float difference = float.MaxValue;
                try
                {
                    difference =
                        Int32.Parse(row.Field<string>("Max")) -
                        Int32.Parse(row.Field<string>("Min"));
                }
                catch
                {}

                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    smallestDay = row.Field<string>("Dd");
                }
            }

            return smallestDay;
        }
    }

}
