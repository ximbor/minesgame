using System;

namespace EscapeMines
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    System.Console.WriteLine("Please enter a game file.");
                    return 1;
                }

                string testFile = args[0];
                var fileReaderService = new SimpleFileSceneReader(testFile);
                var scene = SimpleFileSceneBuilder.Build(fileReaderService);

                foreach (var match in scene.Play())
                {
                    Console.WriteLine(match.Status);

                }
                return 0;
                
            } catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return 1;
            }
        }
    }
}
