using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MovementTile : Tile
{
    public int MovementCost;

#if UNITY_EDITOR
    // The following is a helper that adds a menu item to create a Movement Asset
    [MenuItem("Assets/Create/MovementTile")]
    public static void CreateRoadTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Movement Tile", "New Movement Tile", "Asset", "Save Movement Tile", "Assets");
        if (path == "")
            return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MovementTile>(), path);
    }
#endif
}
