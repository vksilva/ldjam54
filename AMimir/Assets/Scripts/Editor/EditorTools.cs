using UnityEditor;
using UnityEngine;

namespace Busta.Editor
{
    public static class EditorTools
    {
        [MenuItem("Vanessa/Clean Player Prefs")]
        public static void CleanPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}