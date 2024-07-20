using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using Unity.VisualScripting;

namespace Busta.Editor
{
    public static class EditorTools
    {
        private static readonly string levelsPath = Path.Join("Assets", "Scenes", "Levels");
        
        [MenuItem("Vanessa/Clean Player Prefs")]
        public static void CleanPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Vanessa/Add levels to Build Settings")]
        public static void AddLevelsToBuildSettings()
        {
            var scenes = new List<EditorBuildSettingsScene>();
            scenes.AddRange(EditorBuildSettings.scenes);

            //Find levels
            var worldDirectories = Directory.GetDirectories(levelsPath);
            foreach (var dir in worldDirectories)
            {
                var files = Directory.GetFiles(dir, "*.unity");
                foreach (var file in files)
                {
                    scenes.Add(new EditorBuildSettingsScene(file, true));
                }
            }

            EditorBuildSettings.scenes = scenes.DistinctBy(s=> s.guid).ToArray();
        }
    }
}