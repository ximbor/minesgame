using EscapeMines;
using EscapeMinesTests.MockServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EscapeMinesTests.SimpleFileSceneTests
{
    [TestClass]
    public class GameTests
    {

        private SimpleFileSceneReaderMock GetGameMock()
        {
            return new SimpleFileSceneReaderMock()
            {
                BoardSizeStr = "7 6", /*Cols, Rows*/
                StartingPostionStr = "0 0 N",
                ExitPointStr = "6 5",
                MinesStr = "0,3 0,5 1,0 1,1 2,0 2,2 2,3 2,4 4,1 4,2 4,3 4,5 5,5 6,2",
                MovesStr = null
            };
        }

        [TestMethod]
        public void When_FullLineVerticalPathsNoMines_Expect_IsSuccessful()
        {
            var mockReader = new SimpleFileSceneReaderMock()
            {
                BoardSizeStr = "6 4",
                StartingPostionStr = null,
                ExitPointStr = null,
                MinesStr = "",
                MovesStr = "M M M M M M M"
            };

            for (int y = 0; y < 4; y++)
            {
                mockReader.StartingPostionStr = $"{y} 0 S";
                mockReader.ExitPointStr = $"{y} 3";

                foreach( var match in SimpleFileSceneBuilder.Build(mockReader).Play())
                {
                    Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.Success);
                }
            }

        }

        [TestMethod]
        public void When_FullLineHorizontalPathsNoMines_Expect_IsSuccessful()
        {
            var mockReader = new SimpleFileSceneReaderMock()
            {
                BoardSizeStr = "6 4",
                StartingPostionStr = null,
                ExitPointStr = null,
                MinesStr = "",
                MovesStr = "L M M M M M M"
            };

            for (int x = 0; x < 4; x++)
            {
                mockReader.StartingPostionStr = $"0 {x} S";
                mockReader.ExitPointStr = $"5 {x}";

                foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
                {
                    Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.Success);
                }
            }

        }

        [TestMethod]
        public void When_IsPathCorrect_Expect_IsSuccessful()
        {
            var mockReader = GetGameMock();

            // Successful path:
            mockReader.MovesStr = "R R M M L M R M M M L M M L M R M M M R M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.Success);
            }
        }

        [TestMethod]
        public void When_HitsMineIn_0_3_Expect_IsMineHit()
        {
            // Finds a mine in (0, 3):
            var mockReader = GetGameMock();
            mockReader.MovesStr = "R R M M M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.MineHit);
                Assert.AreEqual(match.Location.X, 3);
                Assert.AreEqual(match.Location.Y, 0);
            }

        }


        [TestMethod]
        public void When_HitsMineIn_1_0_Expect_IsMineHit()
        {
            // Finds a mine in (1,0):
            var mockReader = GetGameMock();
            mockReader.MovesStr = "R M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.MineHit);
                Assert.AreEqual(match.Location.X, 0);
                Assert.AreEqual(match.Location.Y, 1);
            }

        }

        [TestMethod]
        public void When_HitsMineIn_1_1_Expect_IsMineHit()
        {
            // Finds a mine in (1,1):
            var mockReader = GetGameMock();
            mockReader.MovesStr = "R R M L M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.MineHit);
                Assert.AreEqual(match.Location.X, 1);
                Assert.AreEqual(match.Location.Y, 1);
            }
        }


        [TestMethod]
        public void When_HitsMineIn_0_5_Expect_IsMineHit()
        {
            // Finds a mine in (0,5):
            var mockReader = GetGameMock();
            mockReader.MovesStr = "R R M M L M R M M R M L M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.MineHit);
                Assert.AreEqual(match.Location.X, 5);
                Assert.AreEqual(match.Location.Y, 0);
            }

        }

        [TestMethod]
        public void When_HitsMineIn_4_1_Expect_IsMineHit()
        {
            // Finds a mine in (4,1):
            var mockReader = GetGameMock();
            mockReader.MovesStr = "R R M M L M R M M M L M M L M M M M R M";

            foreach (var match in SimpleFileSceneBuilder.Build(mockReader).Play())
            {
                Assert.AreEqual(match.Status, EscapeMines.Models.SafetyState.MineHit);
                Assert.AreEqual(match.Location.X, 1);
                Assert.AreEqual(match.Location.Y, 4);
            }
        }



    }
}
