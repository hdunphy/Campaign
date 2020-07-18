using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightMoveAction : IHighlightTileAction
{
    public void OnMouseDownAction(Unit selectedUnit, Vector3Int highlightPosition, List<HighlightTile> highlightTiles)
    {
        selectedUnit.Move(highlightPosition);
    }
}
