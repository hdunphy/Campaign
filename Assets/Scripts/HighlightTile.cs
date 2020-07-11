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

    private HighlightTileType TileType;

    public void SetColor(HighlightTileType type)
    {
        TileType = type;
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

        Unit selectedUnit = Manager.Instance.GetSelectedUnit();
        //Vector3Int tilePos = highlightTileMap.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //TileBase highlightedTile = highlightTileMap.GetTile(tilePos);

        //PlayerController playerOnTile = FindObjectsOfType<PlayerController>().FirstOrDefault(x => x.GetTilePosition() == tilePos);
        Debug.Log("HightlightTile called");
        if (selectedUnit != null)
        {
            if(TileType == HighlightTileType.Move)
            {
                selectedUnit.Move(Vector3Int.FloorToInt(transform.position));
            }
        }
    }
}
