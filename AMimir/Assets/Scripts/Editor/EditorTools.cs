using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorTools
    {
        [MenuItem("Vanessa/cleanPlayerPrefs")]
        public static void CleanPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}