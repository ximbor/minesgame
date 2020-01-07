using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines.Models
{
    public class Board
    {
        /// <summary>
        /// Number of board's rows (N).
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// Number of board's columns (M).
        /// </summary>
        public int Columns { get; set; }

        /// <summary>
        /// Board tiles matrix.
        /// </summary>
        public List<Tile> Tiles { get; set; } = new List<Tile>();


    }
}
