using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EscapeMines
{
    public class SimpleFileSceneReader : ISimpleFileSceneReader
    {

        private string _fileName;

        public SimpleFileSceneReader(string fileName) {
            this._fileName = fileName;
        }


        public string[] LoadData()
        {
            string fileName = this._fileName;

            try
            {                
                string[] lines = File.ReadAllLines(fileName);
                int length = lines.Length;
                
                if(length == 0)
                {
                    throw new ApplicationException("Game file does not contain any information.");
                }

                if (length == 1)
                {
                    throw new ApplicationException("Missing data in game file: mines locations, exit point, turtle start point and moves.");
                }

                if (length == 2)
                {
                    throw new ApplicationException("Missing data in game file: exit point, turtle start point and moves.");
                }

                if (length == 3)
                {
                    throw new ApplicationException("Missing data in game file: turtle start point and moves.");
                }

                // Lines spaces correction:
                lines = lines.Where(line => !string.IsNullOrWhiteSpace(line))
                            .Select(line => System.Text.RegularExpressions.Regex.Replace(line.Trim(), @"\s+", " "))
                            .ToArray();

                //lines =  lines.Take(4)
                //            .Union(new List<string>() { string.Join("", lines.Skip(4).ToArray()) })
                //            .Where(line => !string.IsNullOrWhiteSpace(line))
                //            .Select(line => System.Text.RegularExpressions.Regex.Replace(line.Trim(), @"\s+", " "))
                //            .ToArray();

                return lines;
            }
            catch (ArgumentNullException exc)
            {
                throw new ApplicationException("Game file has not been provided.", exc);
            }
            catch (ArgumentException exc)
            {
                throw new ApplicationException("Game file has not been provided.", exc);
            }
            catch (PathTooLongException exc)
            {
                throw new ApplicationException($"Game file name '{fileName}' is too long.", exc);
            }
            catch (DirectoryNotFoundException exc)
            {
                throw new ApplicationException($"Invalid game file path '{fileName}'.", exc);
            }
            catch (FileNotFoundException exc)
            {
                throw new ApplicationException($"Could not find the specified game file '{fileName}'", exc);
            }
            catch (IOException exc)
            {
                throw new ApplicationException($"Could not load the game file '{fileName}'.", exc);
            }

        }

    }
}
