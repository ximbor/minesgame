using EscapeMines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EscapeMines
{
    public class Scene
    {
        public Board MineField { get; set; } = new Board();

        public List<MoveType> Moves { get; set; } = new List<MoveType>();

        public PlayerState PlayerState { get; set; } = new PlayerState();

        public PlayerState Play()
        {
            // Startup conditions:
            var currentTile = GetPlayerTile(this.PlayerState);

            if (currentTile.IsEnd())
                this.PlayerState.Status = SafetyState.Success;

            if (currentTile.IsMine())
                this.PlayerState.Status = SafetyState.MineHit;

            // Start playing:
            return this.Moves.Aggregate(this.PlayerState, GameReducer);
        }

        public PlayerState GameReducer(PlayerState state, MoveType next)
        {
            var newState = (PlayerState)state.Clone();

            if (newState.Status == SafetyState.MineHit || newState.Status == SafetyState.Success)
                return newState;

            if(next == MoveType.M) {
                if (newState.Direction == Direction.N && newState.Location.X > 0) newState.Location.X -= 1;
                if (newState.Direction == Direction.S && newState.Location.X < MineField.Rows - 1) newState.Location.X += 1;
                if (newState.Direction == Direction.E && newState.Location.Y < MineField.Columns -1) newState.Location.Y += 1;
                if (newState.Direction == Direction.W && newState.Location.Y > 0) newState.Location.Y -= 1;

                Tile tile = GetPlayerTile(newState);
                if (tile != null)
                {
                    newState.SetTile(tile);
                }
                else
                {
                    newState.SetTile(new Tile(newState.Location.X, newState.Location.Y, TileType.Blank));
                }
                
            }
            else
            {
                newState.SetDirection(next);
            }


            return newState;
        }


        private Tile GetPlayerTile(PlayerState state)
        {
            var tile = MineField.Tiles.FirstOrDefault(t => t.X == state.Location.X && t.Y == state.Location.Y);

            return tile == null ? null: new Tile(tile.X, tile.Y, tile.Kind);
        }


    }
}
