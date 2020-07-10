using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum HighlightTileType { Attack, Move }

public class HighlightTile : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color AttackColor;
    public Color MoveColor;

    public void SetColor(HighlightTileType type)
    {
        switch (type)
        {
            case HighlightTileType.Attack:
                spriteRenderer.color = AttackColor;
                break;
            case HighlightTileType.Move:
                spriteRenderer.color = MoveColor;
                break;
        }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }

    private void OnMouseDown()
    {

        //PlayerController selectedPlayer = Manager.Instance.GetPlayer();
        //Vector3Int tilePos = highlightTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //TileBase highlightedTile = highlightTileMap.GetTile(tilePos);

        //PlayerController playerOnTile = FindObjectsOfType<PlayerController>().FirstOrDefault(x => x.GetTilePosition() == tilePos);
        Debug.Log("HightlightTile called");
        //if(selectedPlayer != null)
        //{
        //    if (selectedPlayer != null && highlightedTile != null)
        //    {
        //        selectedPlayer.Move(tilePos);
        //    }
        //    else if (highlightedTile == null)
        //    {
        //        Manager.Instance.SetSelectedPlayer(null);
        //    }
        //}
    }
}
