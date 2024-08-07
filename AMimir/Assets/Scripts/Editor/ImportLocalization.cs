using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Busta.AppCore.Localization;
using Busta.Extensions;
using UnityEditor;
using UnityEngine;

namespace Busta.Editor
{
    public static class ImportLocalization
    {
        private static readonly string LocalizationPath = Path.Join("Assets", "Localization");
        private static Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        [MenuItem("Vanessa/Import Localization")]
        public static void Import()
        {
            // Show open file dialog to select csv file
            var path = EditorUtility.OpenFilePanel("Open localization CSV", "", "csv");
            if (path.IsNullOrEmpty())
            {
                return;
            }

            var lines = File.ReadAllLines(path);

            // Split comma separated values file by comma
            var data = lines.Select(
                line => CSVParser.Split(line).Select(
                    s => s.Trim('"')).ToArray()
                ).ToArray();
            // Languages are row 0 (Header)
            var languages = data[0];
            var languageNames = data[1];

            // For each language, skipping col 0 because those are the keys only
            for (var languageIndex = 1; languageIndex < languages.Length; languageIndex++)
            {
                var key = languages[languageIndex];
                var assetPath = Path.Join(LocalizationPath, $"{key}.asset");

                var languageAsset = AssetDatabase.LoadAssetAtPath<LocalizationData>(assetPath);
                var fileDoesNotExist = !languageAsset;
                if (fileDoesNotExist)
                {
                    languageAsset = ScriptableObject.CreateInstance<LocalizationData>();
                }

                languageAsset.Translations = new List<LocalizationData.LocalizationEntry>();
                
                languageAsset.Key = key;
                languageAsset.Name = languageNames[languageIndex];
                
                // For each row, get element 0 (key) and element from it's language (languageIndex)
                // Row 1 is the language name, which also won't get added to entries
                for (var dataIndex = 2; dataIndex < data.Length; dataIndex++)
                {
                    languageAsset.Translations.Add(new LocalizationData.LocalizationEntry
                    {
                        Key = data[dataIndex][0],
                        Value =  data[dataIndex][languageIndex]
                    });
                }

                if (fileDoesNotExist)
                {
                    // Create the actual asset file in the specified folder
                    AssetDatabase.CreateAsset(languageAsset, assetPath);
                }
            }

            // Save all the created assets and refresh
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}