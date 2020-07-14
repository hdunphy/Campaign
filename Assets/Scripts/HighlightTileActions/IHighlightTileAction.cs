using UnityEngine;

public interface IHighlightTileAction
{
    void OnMouseDownAction(Unit selectedUnit, Vector3Int highlightPosition);
}
