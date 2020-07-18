using System.Collections.Generic;
using UnityEngine;

public class HighlightAttackAction : IHighlightTileAction
{
    public void OnMouseDownAction(Unit selectedUnit, Vector3Int highlightPosition, List<HighlightTile> highlightTiles)
    {
        Vector3Int unitPosition = selectedUnit.GetTilePosition();
        int distanceFromEnemy = PathFinding.ManhattanDistance(unitPosition, highlightPosition);
        int unitAttackRange = selectedUnit.GetAttackRange();
        if(distanceFromEnemy > unitAttackRange)
        {
            selectedUnit.Move(ClosestTileInRange(unitAttackRange, unitPosition, highlightPosition, highlightTiles));
        }
        selectedUnit.Attack(highlightPosition);
        //After attacking if enemy is dead, move onto that square
    }

    //Move to closest walkable tile
    private Vector3Int ClosestTileInRange(int AttackRange, Vector3Int unitPosition, Vector3Int position, List<HighlightTile> highlightTiles)
    {
        int distanceFromUnit = int.MaxValue;
        Vector3Int closestTileInRange = position;

        foreach(HighlightTile highlightTile in highlightTiles)
        {
            Vector3Int tilePosition = Vector3Int.FloorToInt(highlightTile.transform.position);
            int manhattanDistance = PathFinding.ManhattanDistance(position, tilePosition);
            if (manhattanDistance <= AttackRange)
            {
                int tempDistanceFromUnit = PathFinding.ManhattanDistance(unitPosition, tilePosition);
                if(tempDistanceFromUnit < distanceFromUnit)
                {
                    distanceFromUnit = tempDistanceFromUnit;
                    closestTileInRange = tilePosition;
                }
            }
        }
        if (closestTileInRange == position)
            Debug.Log("Couldn't find closest tile");
        return closestTileInRange;
    }
}
