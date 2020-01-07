using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines.Models
{
    [Flags] public enum TileType
    {
        Blank = 0,
        Mine = 1,
        Start = 2,
        End = 4
    }
}
