using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines.Models
{
    public class PlayerState : ICloneable
    {

        public Direction Direction { get; set; }

        public Tile Location { get; set; }

        public SafetyState Status { get; set; }

        public object Clone()
        {
            return new PlayerState()
            {
                Direction = this.Direction,
                Status = this.Status,
                Location = new Tile(this.Location.X, this.Location.Y, this.Location.Kind)
            };
        }

        public void SetDirection(MoveType move)
        {
            // IF NORTH
            if (this.Direction == Direction.N)
            {
                if (move == MoveType.L) this.Direction = Direction.W;
                else if (move == MoveType.R) this.Direction = Direction.E;
                return;
            }

            // IF EAST
            if (this.Direction == Direction.E) {
                if (move == MoveType.L) this.Direction = Direction.N;
                else if (move == MoveType.R) this.Direction = Direction.S;
                return;
            }
            
            // IF SOUTH
            if (this.Direction == Direction.S) {
                if (move == MoveType.L) this.Direction = Direction.E;
                else if (move == MoveType.R) this.Direction = Direction.W;
                return;
            }
            
            // IF WEST
            if (this.Direction == Direction.W) {
                if (move == MoveType.L) this.Direction = Direction.S;
                else if (move == MoveType.R) this.Direction = Direction.N;
            }
        }

        public void SetTile(Tile tile)
        {

            this.Location = new Tile(tile.X, tile.Y, tile.Kind);

            if (this.Location.IsStart())
            {
                this.Location.Kind = TileType.Start;
            }
            else if (this.Location.IsMine())
                this.Status = SafetyState.MineHit;

            else if (this.Location.IsEnd())
                this.Status = SafetyState.Success;

            else
            {
                this.Location.Kind = TileType.Blank;
            }
                       
        }
    }
}
