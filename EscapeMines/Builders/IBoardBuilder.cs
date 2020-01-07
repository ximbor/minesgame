using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMines
{
    public interface IBoardBuilder
    {

        /// <summary>
        /// Builds the board size.
        /// </summary>
        void BuildBoardSize();

        /// <summary>
        /// Builds the exit point.
        /// </summary>
        void BuildExitPoint();

        /// <summary>
        /// Builds the player state.
        /// </summary>
        void BuildPlayerState();

        /// <summary>
        /// Builds the mines.
        /// </summary>
        void BuildMines();

        /// <summary>
        /// Builds the moves.
        /// </summary>
        void BuildMoves();

    }
}
