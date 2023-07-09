using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinding : MonoBehaviour
{
    public Tilemap BaseTileMap;
    public HighlightTile Highlight;
    public static PathFinding Instance;

    private List<Vector3Int> highlightTilePositions;
    private AStar AStar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }

    private void Start()
    {
        highlightTilePositions = new List<Vector3Int>();
        AStar = new AStar(BaseTileMap);

        EventManager.Instance.ResetHighlightedTiles += ResetHighlightedTiles;
        EventManager.Instance.SetHightlightedTiles += SetTileHighlights;
        EventManager.Instance.GetEnemiesInRange += GetEnemiesInRange;
    }

    private void OnDestroy()
    {
        EventManager.Instance.ResetHighlightedTiles -= ResetHighlightedTiles;
        EventManager.Instance.SetHightlightedTiles -= SetTileHighlights;
        EventManager.Instance.GetEnemiesInRange -= GetEnemiesInRange;
    }

    private void SetTileHighlights(Unit unit)
    {
        IEnumerable<Vector3Int> unitPositions = FindObjectsOfType<Unit>()
            .Select(x => x.GetTilePosition());

        Vector3Int currentPosition = BaseTileMap.WorldToCell(unit.transform.position);
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

                    Unit enemy = FindObjectsOfType<Unit>().FirstOrDefault(x => x.GetTilePosition() == newPosition && x.PlayerColor != unit.PlayerColor);
                    if (enemy != null && unit.GetMoveDistance() + unit.GetAttackRange() >= current.cost + 1)
                    {
                        var highlight = Instantiate(Highlight, newPosition, Quaternion.identity);
                        highlight.SetType(HighlightTileType.Attack);
                        newCost = Unit.MOVEMENT_COST; //Can't move through enemies
                        queue.Add(new MovementPath { cost = newCost, position = newPosition });
                    }
                    else if (newCost <= unit.GetMoveDistance() && !highlightTilePositions.Contains(newPosition))
                    {
                        if (!unitPositions.Contains(newPosition))
                        {
                            highlightTilePositions.Add(newPosition);
                            var highlight = Instantiate(Highlight, newPosition, Quaternion.identity);
                            highlight.SetType(HighlightTileType.Move);
                        }
                        queue.Add(new MovementPath { cost = newCost, position = newPosition });
                    }
                }
            }
            queue.RemoveAt(0);
        }
    }

    public void GetEnemiesInRange(Unit unit)
    {
        foreach(Unit enemy in FindObjectsOfType<Unit>())
        {
            int distance = ManhattanDistance(unit.GetTilePosition(), enemy.GetTilePosition());
            if(enemy.PlayerColor != unit.PlayerColor && distance <= unit.GetAttackRange())
            {
                var highlight = Instantiate(Highlight, enemy.GetTilePosition(), Quaternion.identity);
                highlight.SetType(HighlightTileType.Attack);
            }
        }
    }

    private void ResetHighlightedTiles()
    {
        foreach (HighlightTile highlight in FindObjectsOfType<HighlightTile>())
            Destroy(highlight.gameObject);
        highlightTilePositions.Clear();
    }

    public List<Vector3Int> FindPathBetween(Vector3 start, Vector3 goal, PlayerColor player)
    {
        Vector3Int startInt = BaseTileMap.WorldToCell(start);
        Vector3Int goalInt = BaseTileMap.WorldToCell(goal);
        List<Vector3Int> unitPositions = FindObjectsOfType<Unit>()
            .Where(x => x.PlayerColor != player)
            .Select(x => x.GetTilePosition()).ToList();
        return AStar.GetPath(startInt, goalInt, unitPositions);
    }

    /// <summary>
    /// Gets the manhattan distance from current position to the goal
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>distance (int)</returns>
    public static int ManhattanDistance(Vector3Int from, Vector3Int to)
    {
        return Math.Abs(from.x - to.x) +
            Math.Abs(from.y - to.y);
    }
}
