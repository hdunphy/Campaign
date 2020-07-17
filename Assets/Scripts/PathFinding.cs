using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public Tilemap BaseTileMap;
    public static PathFinding Instance;

    private List<Vector3Int> highlightTilePositions;
    public HighlightTile Highlight;

    private void Awake()
    {
        Instance = this;
        highlightTilePositions = new List<Vector3Int>();
    }

    public void SetTileHighlights(Vector3 position, int MoveDistance, int AttackRange, PlayerColor playerColor)
    {
        ResetHighlightedTiles();

        IEnumerable<Vector3Int> unitPositions = FindObjectsOfType<Unit>()
            .Select(x => x.GetTilePosition());

        Vector3Int currentPosition = BaseTileMap.WorldToCell(position);
        List<Vector3Int> directions = new List<Vector3Int>
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };
        Hashtable visited = new Hashtable();
        List<MovementPath> queue = new List<MovementPath> { new MovementPath { position = currentPosition, cost = 0 } };

        while (queue.Count > 0)
        {
            MovementPath current = queue[0];
            foreach (Vector3Int dir in directions)
            {
                Vector3Int newPosition = current.position + dir;
                MovementTile tile = BaseTileMap.GetTile<MovementTile>(newPosition);
                int newCost;
                if (tile != null)
                {
                    newCost = tile.MovementCost + current.cost;
                    bool hasVisited = visited.ContainsKey(newPosition);
                    if (hasVisited && (int)visited[newPosition] <= newCost)
                        continue;

                    if (hasVisited)
                        visited[newPosition] = newCost;
                    else
                        visited.Add(newPosition, newCost);

                    Unit enemy = FindObjectsOfType<Unit>().FirstOrDefault(x => x.GetTilePosition() == newPosition && x.PlayerColor != playerColor);
                    if (enemy != null && MoveDistance + AttackRange >= current.cost + 1)
                    {
                        var highlight = Instantiate(Highlight, newPosition, Quaternion.identity);
                        highlight.SetColor(HighlightTileType.Attack);
                        newCost = 99999; //Can't move through enemies
                        queue.Add(new MovementPath { cost = newCost, position = newPosition });
                    }
                    else if (newCost <= MoveDistance && !highlightTilePositions.Contains(newPosition))
                    {
                        if (!unitPositions.Contains(newPosition))
                        {
                            highlightTilePositions.Add(newPosition);
                            var highlight = Instantiate(Highlight, newPosition, Quaternion.identity);
                            highlight.SetColor(HighlightTileType.Move);
                        }
                        queue.Add(new MovementPath { cost = newCost, position = newPosition });
                    }
                }
            }
            queue.RemoveAt(0);
        }

        //DrawWalkableTiles();
    }

    //private void DrawWalkableTiles()
    //{
    //    IEnumerable<Vector3Int> unitPositions = FindObjectsOfType<Unit>()
    //        .Select(x => x.GetTilePosition());

    //    foreach (Vector3Int pos in walkableTilePositions)
    //    {
    //        if (!unitPositions.Contains(pos))
    //        {
    //            var highlight = Instantiate(Highlight, pos, Quaternion.identity);
    //            highlight.SetColor(HighlightTileType.Move);
    //        }
    //    }
    //    foreach (Vector3Int pos in enemeyInRangePositions)
    //    {
    //        var highlight = Instantiate(Highlight, pos, Quaternion.identity);
    //        highlight.SetColor(HighlightTileType.Attack);
    //    }
    //}

    public void ResetHighlightedTiles()
    {
        foreach (HighlightTile highlight in FindObjectsOfType<HighlightTile>())
            Destroy(highlight.gameObject);
        highlightTilePositions.Clear();
        //walkableTilePositions.Clear();
        //enemeyInRangePositions.Clear();
    }

    private class MovementPath
    {
        public Vector3Int position;
        public int cost;

        public override bool Equals(object obj)
        {
            return obj is MovementPath path &&
                   position.Equals(path.position);
        }

        public override int GetHashCode()
        {
            int hashCode = 810222802;
            hashCode = hashCode * -1521134295 + position.GetHashCode();
            hashCode = hashCode * -1521134295 + cost.GetHashCode();
            return hashCode;
        }
    }
}
