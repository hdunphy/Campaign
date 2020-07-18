using System.Collections.Generic;
using UnityEngine;

public interface IHighlightTileAction
{
    void OnMouseDownAction(Unit selectedUnit, Vector3Int highlightPosition, List<HighlightTile> highlightTiles);
}
