using EscapeMines.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EscapeMines
{
    public class SimpleFileSceneBuilder : IBoardBuilder
    {
        private const int BOARD_SIZE_LINE = 0;
        private const int MINES_LINE = 1;
        private const int EXIT_POINT_LINE = 2;
        private const int START_POINT_LINE = 3;
        private const int MOVES_LINE = 4;

        private Scene _scene = new Scene();
        private readonly string[] _sourceLines;

        public SimpleFileSceneBuilder(ISimpleFileSceneReader readerService)
        {
            this.Reset();
            _sourceLines = readerService.LoadData();
            this.BuildBoard();
        }

        public void Reset()
        {
            this._scene = new Scene();
        }

        public void BuildBoard()
        {

            BuildBoardSize();
            BuildPlayerState();
            BuildExitPoint();
            BuildMines();
            BuildMoves();

            PostBuildChecks();


        }

        private void PostBuildChecks()
        {

            var mines = this._scene.MineField.Tiles.Where(t => t.IsMine());
            var start = this._scene.MineField.Tiles.Where(t => t.IsStart());
            var end = this._scene.MineField.Tiles.Where(t => t.IsEnd());


            // No start specified
            if (start.Count() == 0)
            {
                throw new ApplicationException("No start point specified");
            }

            // More than one start point specified:
            if (start.Count() > 1)
            {
                throw new ApplicationException("More than one start point specified");
            }

            // No end specified
            if (end.Count() == 0)
            {
                throw new ApplicationException("No end point specified");
            }

            // More than one end point specified:
            if (end.Count() > 1)
            {
                throw new ApplicationException("More than one end point specified");
            }


            // Out of range mines
            if (mines.ToList().Exists(m => m.X >= _scene.MineField.Rows || m.Y >= _scene.MineField.Columns))
            {
                throw new ApplicationException("Out of range mines found");
            }

            // Out of range exit point
            if (end.ToList().Exists(e => e.X >= _scene.MineField.Rows || e.Y >= _scene.MineField.Columns))
            {
                throw new ApplicationException("Out of range exit point");
            }

            // Out of range starting point
            if (start.ToList().Exists(s => s.X >= _scene.MineField.Rows || s.Y >= _scene.MineField.Columns))
            {
                throw new ApplicationException("Out of range starting point");
            }

            // Duplicates mines
            var minesDuplicates = mines
                                    .GroupBy(t => new { t.X, t.Y })
                                    .Where(g => g.Count() > 1);
            if(minesDuplicates.Count() > 0)
            {
                throw new ApplicationException("Duplicated mines");
            }

            // Starting point is a mine
            if(start.Where(s => mines.ToList().Exists(m => m.X == s.X && m.Y == s.Y)).Count() > 0)
            {
                throw new ApplicationException("Starting point cannot be a mine");
            }

            // Exit point is a mine
            if(end.Where(e => mines.ToList().Exists(m => m.X == e.X && m.Y == e.Y)).Count() > 0)
            {
                throw new ApplicationException("Exit point cannot be a mine");
            }

            // End point is a start point
            if (end.Where(e => start.ToList().Exists(s => s.X == e.X && s.Y == e.Y)).Count() > 0)
            {
                throw new ApplicationException("Exit point cannot coincide with start point");
            }

        }

        public void BuildBoardSize()
        {

            string[] items;
            string line = _sourceLines[BOARD_SIZE_LINE];
            string errorMessage = $"Invalid board size data (line {BOARD_SIZE_LINE}): {line}";

            if (String.IsNullOrEmpty(line))
                throw new ApplicationException($"Empty board size data (line {BOARD_SIZE_LINE})");

            items = line.Split(' ');

            if(items.Length < 2)
                throw new ApplicationException(errorMessage);

            if(int.TryParse(items[0], out int cols) &&  int.TryParse(items[1], out int rows))
            {
                this._scene.MineField.Columns = cols;
                this._scene.MineField.Rows = rows;
            } else
            {
                throw new ApplicationException(errorMessage);
            }

        }

        public void BuildPlayerState()
        {
            string[] items;
            string line = _sourceLines[START_POINT_LINE];
            string errorMessage = $"Invalid player state data (line {START_POINT_LINE}): {line}";

            if (String.IsNullOrEmpty(line))
                throw new ApplicationException($"Empty player state data (line {START_POINT_LINE})");

            items = line.Split(' ');

            if (items.Length < 3)
                throw new ApplicationException(errorMessage);

            if (int.TryParse(items[1], out int x) && int.TryParse(items[0], out int y))
            {
                try
                {
                    var direction = (Direction)Enum.Parse(typeof(Direction), items[2]);
                    this._scene.PlayerState.Direction = direction;
                    this._scene.PlayerState.Location = new Tile(x, y, TileType.Start);
                    this._scene.PlayerState.Status = SafetyState.StillInDanger;
                    this._scene.MineField.Tiles.Add(this._scene.PlayerState.Location);
                }
                catch (Exception)
                {
                    throw new ApplicationException(errorMessage);
                }
            }
            else
            {
                throw new ApplicationException(errorMessage);
            }
        }

        public void BuildExitPoint()
        {
            string[] items;
            string line = _sourceLines[EXIT_POINT_LINE];
            string errorMessage = $"Invalid exit point data (line {EXIT_POINT_LINE}): {line}";

            if (String.IsNullOrEmpty(line))
                throw new ApplicationException($"Empty exit point data (line {EXIT_POINT_LINE})");

            items = line.Split(' ');

            if (items.Length < 2)
                throw new ApplicationException(errorMessage);

            if (int.TryParse(items[1], out int x) && int.TryParse(items[0], out int y))
            {
                this._scene.MineField.Tiles.Add(new Tile(x, y, TileType.End));
            }
            else
            {
                throw new ApplicationException(errorMessage);
            }
        }

        public void BuildMines()
        {
            string line = _sourceLines[MINES_LINE];
            string errorMessage = $"Invalid mines data (line {MINES_LINE}): {line}";

            if (String.IsNullOrEmpty(line))
                return; // No mines have been added.

            line.Split(' ').ToList().ForEach(mine => {
                var minePos = mine.Split(",");
                if (minePos.Length < 2)
                    throw new ApplicationException(errorMessage);

                if (int.TryParse(minePos[1], out int x) && int.TryParse(minePos[0], out int y))
                {
                    this._scene.MineField.Tiles.Add(new Tile(x, y, TileType.Mine));
                }
                else
                {
                    throw new ApplicationException(errorMessage);
                }
            });

        }

        public void BuildMoves()
        {
            // Add moves only in case they have been specified (it's optional by design):
            if(_sourceLines.Length > MOVES_LINE && ! string.IsNullOrWhiteSpace(_sourceLines[MOVES_LINE]))
            {
                var moveLines = _sourceLines.Skip(MOVES_LINE);
                int index = 0;
                moveLines.ToList().ForEach(line => {
                    var moves = new List<MoveType>();
                    string errorMessage = $"Invalid moves data (line {MOVES_LINE}): {line}";
                    line.Split(' ').ToList().ForEach(item => {
                        try
                        {
                            var move = (MoveType)Enum.Parse(typeof(MoveType), item);
                            //this._scene.Moves.Add(move);
                            moves.Add(move);
                        }
                        catch (Exception)
                        {
                            throw new ApplicationException(errorMessage);
                        }
                    });

                    this._scene.MovesSeries.Add(index, moves);
                    index++;
                });

            }

        }

        public static Scene Build(ISimpleFileSceneReader readerService)
        {
            var sceneBuilder = new SimpleFileSceneBuilder(readerService);
            return sceneBuilder._scene;
        }

    }
}
