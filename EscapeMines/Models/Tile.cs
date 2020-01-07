using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines.Models
{
    public class Tile
    {
        public Tile(int X, int Y, TileType TypeOfTile)
        {
            this.X = X;
            this.Y = Y;
            this.Kind = TypeOfTile;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public TileType Kind { get; set; }

        public bool IsMine()
        {
            return this.Kind.HasFlag(TileType.Mine);
        }

        public bool IsEnd()
        {
            return this.Kind.HasFlag(TileType.End);
        }

        public bool IsStart()
        {
            return this.Kind.HasFlag(TileType.Start);
        }
    }
}
