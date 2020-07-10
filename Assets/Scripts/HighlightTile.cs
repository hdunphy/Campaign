using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightTile : MonoBehaviour
{
    public Tilemap highlightTileMap;

    private void OnMouseDown()
    {
        PlayerController selectedPlayer = Manager.Instance.GetPlayer();
        Vector3Int tilePos = highlightTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        TileBase highlightedTile = highlightTileMap.GetTile(tilePos);

        PlayerController playerOnTile = FindObjectsOfType<PlayerController>().FirstOrDefault(x => x.GetTilePosition() == tilePos);
        if(selectedPlayer == null)
        {
            playerOnTile.SelectUnit();
        }
        else
        {
            if (playerOnTile != null && selectedPlayer != playerOnTile)
                playerOnTile.SelectUnit();
            //need to add a check for enemy players to attack
            else if (selectedPlayer != null && highlightedTile != null)
            {
                selectedPlayer.Move(tilePos);
            }
            else if (highlightedTile == null)
            {
                Manager.Instance.SetSelectedPlayer(null);
            }
        }
    }
}
