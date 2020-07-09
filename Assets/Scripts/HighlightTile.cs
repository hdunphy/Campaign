using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightTile : MonoBehaviour
{
    public Tilemap highlightTileMap;

    private void OnMouseDown()
    {
        PlayerController player = Manager.Instance.GetPlayer();
        Vector3Int tilePos = highlightTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        TileBase highlightedTile = highlightTileMap.GetTile(tilePos);
        if (player != null && highlightedTile != null)
        {
            player.Move(tilePos);
        }
        else if(highlightedTile == null)
        {
            Manager.Instance.SetSelectedPlayer(null);
        }
    }
}
