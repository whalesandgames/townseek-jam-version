using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WhalesAndGames.Helpers
{
    /// <summary>
    /// Takes a screenshot of the current state of the game.
    /// </summary>
    public class ScreenshotTaker : MonoBehaviour
    {
#if UNITY_EDITOR
        /// <summary>
        /// Takes a game screenshot.
        /// </summary>
        [MenuItem("WAG/Take Screenshot/Capture Standard Screenshot &s")]
        public static void TakeStandardScreenshot()
        {
            TakeScreenshot(1);
        }

        /// <summary>
        /// Takes a high-resolution game screenshot.
        /// </summary>
        [MenuItem("WAG/Take Screenshot/Capture High-Resolution Screenshot &h")]
        public static void TakeHighResolutionScreenshot()
        {
            TakeScreenshot(3);
        }
#endif

        /// <summary>
        /// Writes the texture to disk.
        /// </summary>
        public static void TakeScreenshot(int scaleFactor)
        {
            // Prepares the screenshot directory.
            string applicationPath = new DirectoryInfo(Application.dataPath).Parent.FullName + Path.DirectorySeparatorChar;
            string screenshotPath = "Screenshots" + Path.DirectorySeparatorChar;
            string fileName = "Screenshot " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + ".png";

            // Creates a screenshot folder if it doesn't exist yet.
            if (!Directory.Exists(applicationPath + screenshotPath))
            {
                Directory.CreateDirectory(applicationPath + screenshotPath);
            }

            // Writes the screenhsot to the path.
            ScreenCapture.CaptureScreenshot(screenshotPath + fileName, scaleFactor);
        }
    }
}
