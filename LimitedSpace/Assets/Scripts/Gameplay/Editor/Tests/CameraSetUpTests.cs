using AppCore;
using NUnit.Framework;
using UnityEngine;

namespace Gameplay.Editor.Tests
{
    public class CameraSetUpTests
    {
        [TestCase(10, 16, (11f/16f), 8f)]
        [TestCase(10, 16, (9f/16f), 160f/18f)]
        public void CalculateCameraSize_CorrectOutput(int gameAreaX, int gameAreaY, float cameraAspect, float expected)
        {
            var result = CameraSetUp.CalculateCameraSize(new Vector2Int(gameAreaX, gameAreaY), cameraAspect);
            Assert.That(Mathf.Abs(result - expected) < 0.01f, $"Expected: {expected}, actual: {result}");
        }
    }
}