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
    private IHighlightTileAction tileAction;

    public void SetType(HighlightTileType type)
    {
        TileType = type;
        switch (type)
        {
            case HighlightTileType.Attack:
                spriteRenderer.color = AttackColor;
                tileAction = new HighlightAttackAction();
                break;
            case HighlightTileType.Move:
                spriteRenderer.color = MoveColor;
                tileAction = new HighlightMoveAction();
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
        if (selectedUnit != null)
        {
            List<HighlightTile> highlightTiles = FindObjectsOfType<HighlightTile>().ToList();
            tileAction.OnMouseDownAction(selectedUnit, Vector3Int.FloorToInt(transform.position), highlightTiles);
        }
    }
}
