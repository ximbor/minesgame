using EscapeMines;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMinesTests.MockServices
{
    public class SimpleFileSceneReaderMock : ISimpleFileSceneReader
    {

        public string BoardSizeStr;
        public string MinesStr;
        public string ExitPointStr;
        public string StartingPostionStr;
        public string MovesStr;


        public SimpleFileSceneReaderMock() { }
        public string[] LoadData()
        {
            List<string> items = new List<string>();

            items.Add(BoardSizeStr);
            items.Add(MinesStr);
            items.Add(ExitPointStr);
            items.Add(StartingPostionStr);
            items.Add(MovesStr);

            return items.ToArray();

        }
    }
}
