using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerPrefDelete : EditorWindow{

    [MenuItem("Tools/Player Prefs Remover")]

    public static void DeletePlayerPrefs() {
        PlayerPrefs.DeleteAll();
        Debug.Log("Player data removed.");
    }
}
