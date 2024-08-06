using NUnit.Framework;
using UnityEngine;

namespace Busta.Editor
{
    [TestFixture]
    public class GenerateLevelSolutionTests
    {
        [Test]
        public void CheckOverlap_IsValid()
        {
            var catIndex = 2;

            // 0 0 0 0 
            // 1 2 2 0
            // 1 2 2 0
            // 1 1 0 0

            GenerateLevelSolution.CatState catState = new GenerateLevelSolution.CatState
            {
                Positions = new[]
                {
                    new Vector2Int(0, 0), // cat 1 - at 0,0
                    new Vector2Int(1, 1), // cat 2 - at 1,1
                    new Vector2Int(-1, -1), // cat 3 - not placed
                    new Vector2Int(-1, -1), // cat 4 - not placed
                }
            };

            GenerateLevelSolution.PiecePlacementData[] catsData =
            {
                new() // cat 1 data
                {
                    Size = new Vector2Int(2, 3),
                    Matrix = new[,]
                    {
                        { 1, 1 },
                        { 1, 0 },
                        { 1, 0 }
                    },
                    PossiblePositions = null
                },
                new() // cat 2 data
                {
                    Size = new Vector2Int(2, 2),
                    Matrix = new[,]
                    {
                        { 1, 1 },
                        { 1, 1 }
                    }
                }
            };
            var bedData = new GenerateLevelSolution.BedData
            {
                Size = new Vector2Int(4, 4),
                Matrix = new[,]
                {
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                }
            };


            Assert.IsTrue(GenerateLevelSolution.CheckIfValid(catIndex, catState, catsData, bedData));
        }

        [Test]
        public void CheckOverlap_IsNotValid()
        {
            // 0 0 0 0
            // 1 1 1 0
            // 0 2 # 0
            // 0 2 2 0

            //Assert.IsFalse(GenerateLevelSolution.CheckIfValid());
        }
    }
}