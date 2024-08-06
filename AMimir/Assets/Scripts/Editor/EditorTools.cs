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
        private static readonly string catsPath = Path.Join("Assets", "Prefabs", "Cats");
        private static readonly string obstaclesPath = Path.Join("Assets", "Prefabs", "Obstacles");

        [MenuItem("Vanessa/Utils/Clean Player Prefs")]
        public static void CleanPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

        [MenuItem("Vanessa/Configs/Add levels to Build Settings")]
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

            EditorBuildSettings.scenes = scenes.DistinctBy(s => s.guid).ToArray();
        }

        [MenuItem("Vanessa/Configs/Fix Cat Collider")]
        public static void FixCatCollider()
        {
            Debug.Log($"Fix cat collider {catsPath}");
            var files = Directory.GetFiles(catsPath);
            foreach (var file in files)
            {
                var cat = AssetDatabase.LoadAssetAtPath<PolygonCollider2D>(file);
                if (!cat)
                {
                    continue;
                }

                Debug.Log($"Fixing {cat.name}");
                for (var i = 0; i < cat.pathCount; i++)
                {
                    var path = cat.GetPath(i);
                    path = path.Select(v => new Vector2(Mathf.Round(v.x), Mathf.Round(v.y))).ToArray();
                    cat.SetPath(i, path);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}