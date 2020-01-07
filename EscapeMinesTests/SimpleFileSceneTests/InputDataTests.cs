using EscapeMines;
using EscapeMinesTests.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EscapeMinesTests
{
    [TestClass]
    public class InputDataTests
    {
        private SimpleFileSceneReaderMock GetBasicMockReader()
        {
            return new SimpleFileSceneReaderMock()
            {
                BoardSizeStr = "6 7",
                StartingPostionStr = "0 0 E",
                ExitPointStr = "2 2",
                MinesStr = "1,1",
                MovesStr = "R M L M M"
            };
        }

        [TestMethod]
        public void When_BoardSizeIncorrect_Expect_AnExceptionIsThrown()
        {
            var mockReader = GetBasicMockReader();

            mockReader.BoardSizeStr = String.Empty;
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty board size data (line 0)");

            mockReader.BoardSizeStr = null;
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty board size data (line 0)");

            mockReader.BoardSizeStr = "FOO";
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, $"Invalid board size data (line 0): {mockReader.BoardSizeStr}");

        }

        [TestMethod]
        public void When_StartingPositionIsNotDefined_Expect_AnExceptionIsThrown()
        {
            var mockReader = GetBasicMockReader();

            mockReader.StartingPostionStr = String.Empty;
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty player state data (line 3)");

            mockReader.StartingPostionStr = null;
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty player state data (line 3)");

            mockReader.StartingPostionStr = "FOO";
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, $"Invalid player state data (line 3): {mockReader.StartingPostionStr}");

        }

        [TestMethod]
        public void When_ExitPointIsNotDefined_Expect_AnExceptionIsThrown()
        {
            var mockReader = GetBasicMockReader();
            mockReader.ExitPointStr = String.Empty;

            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty exit point data (line 2)");

            mockReader.ExitPointStr = null;
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Empty exit point data (line 2)");

            mockReader.ExitPointStr = "FOO";
            ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, $"Invalid exit point data (line 2): {mockReader.ExitPointStr}");

        }

        [TestMethod]
        public void When_BoardHasNoMines_Expect_NoMinesAreAdded()
        {
            var mockReader = GetBasicMockReader();

            mockReader.MinesStr = String.Empty;
            var result = SimpleFileSceneBuilder.Build(mockReader);
            Assert.IsFalse(result.MineField.Tiles.Exists(tile => tile.IsMine()));
        }

        [TestMethod]
        public void When_BoardHasIncorrectMines_Expect_AnExceptionIsThrown()
        {
            var mockReader = GetBasicMockReader();

            mockReader.MinesStr = "FOO";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, $"Invalid mines data (line 1): {mockReader.MinesStr}");

        }

        [TestMethod]
        public void When_BoardHasNoMoves_Expect_NoMovesAreAdded()
        {
            var mockReader = GetBasicMockReader();

            mockReader.MovesStr = String.Empty;
            var result = SimpleFileSceneBuilder.Build(mockReader);
            Assert.IsTrue( result.Moves.Count==0 );
        }

        [TestMethod]
        public void When_BoardIs7x3_Expect_7x3Board()
        {
            var mockReader = GetBasicMockReader();

            mockReader.BoardSizeStr = "7 3";
            var result = SimpleFileSceneBuilder.Build(mockReader);
            Assert.AreEqual(result.MineField.Columns, 7);
            Assert.AreEqual(result.MineField.Rows, 3);
        }

        [TestMethod]
        public void When_StartingPointIs5_3_W_Expect_StartingPoint_5_3_W()
        {
            var mockReader = GetBasicMockReader();

            mockReader.StartingPostionStr = "5 3 W";
            var result = SimpleFileSceneBuilder.Build(mockReader);
            Assert.AreEqual(result.PlayerState.Location.Y, 5);
            Assert.AreEqual(result.PlayerState.Location.X, 3);
            Assert.AreEqual(result.PlayerState.Direction, EscapeMines.Models.Direction.W);
        }

        [TestMethod]
        public void When_StartingPointIsAMine_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.StartingPostionStr = "5 3 W";
            mockReader.MinesStr = "1,1 5,3";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Starting point cannot be a mine");
        }

        [TestMethod]
        public void When_ExitPointIsAMine_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.ExitPointStr = "2 2";
            mockReader.MinesStr = "1,1 2,2";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Exit point cannot be a mine");
        }

        [TestMethod]
        public void When_StartingPointIsExitPoint_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.ExitPointStr = "2 2";
            mockReader.StartingPostionStr = "2 2 E";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Exit point cannot coincide with start point");
        }

        [TestMethod]
        public void When_DuplicatedMines_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.MinesStr = "1,1 2,1 2,1";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Duplicated mines");
        }

        [TestMethod]
        public void When_OuOfRangeMines_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.MinesStr = "3,13 2,1";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Out of range mines found");
        }

        [TestMethod]
        public void When_OuOfRangeStartingPoint_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.StartingPostionStr = "12 1 W";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Out of range starting point");
        }

        [TestMethod]
        public void When_OuOfRangeExitPoint_Expect_Exception()
        {
            var mockReader = GetBasicMockReader();

            mockReader.ExitPointStr = "12 1";
            var ex = Assert.ThrowsException<ApplicationException>(() => SimpleFileSceneBuilder.Build(mockReader));
            Assert.AreEqual(ex.Message, "Out of range exit point");
        }


    }
}
