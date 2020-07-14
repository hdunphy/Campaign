using UnityEngine;

public class HighlightAttackAction : IHighlightTileAction
{
    public void OnMouseDownAction(Unit selectedUnit, Vector3Int highlightPosition)
    {
        selectedUnit.Move(ClosestWalkableTile(highlightPosition));
    }

    private Vector3Int ClosestWalkableTile(Vector3Int position)
    {
        return position - Vector3Int.down;
    }
}
