using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public Tilemap BaseTileMap;
    public HighlightTile Highlight;
    public int MoveDistance;
    public int AttackRange;
    public float MoveSpeed;
    public Vector3 PositionOffset;
    public PlayerColor PlayerColor;

    private Manager _manager;
    private bool isSelected;
    private List<Vector3Int> walkableTilePositions;
    private List<Vector3Int> enemeyInRangePositions;

    // Start is called before the first frame update
    void Start()
    {
        walkableTilePositions = new List<Vector3Int>();
        enemeyInRangePositions = new List<Vector3Int>();
        _manager = Manager.Instance;

        //Set sprite color for which team
        if (PlayerColor == PlayerColor.Blue)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (PlayerColor == PlayerColor.Red)
            GetComponent<SpriteRenderer>().color = Color.red;

    }

    //public void SelectUnit()
    private void OnMouseDown()
    {
        if (isSelected)
        {
            _manager.SetSelectedUnit(null);
        }
        else
        {
            _manager.SetSelectedUnit(this);
        }
    }

    internal void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        if (this.isSelected)
        {
            SetTileHighlights();
        }
        else
        {
            ResetHighlightedTiles();
        }
    }

    private void SetTileHighlights()
    {
        ResetHighlightedTiles();

        Vector3Int currentPosition = BaseTileMap.WorldToCell(transform.position);
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
                Vector3Int newPos = current.position + dir;
                MovementTile tile = BaseTileMap.GetTile<MovementTile>(newPos);
                int newCost;
                if (tile != null)
                {
                    newCost = tile.MovementCost + current.cost;
                    bool hasVisited = visited.ContainsKey(newPos);
                    if (hasVisited && (int)visited[newPos] <= newCost)
                        continue;

                    if (hasVisited)
                        visited[newPos] = newCost;
                    else
                        visited.Add(newPos, newCost);

                    Unit enemy = FindObjectsOfType<Unit>().FirstOrDefault(x => x.GetTilePosition() == newPos && x.PlayerColor != this.PlayerColor);
                    if (enemy != null && MoveDistance + AttackRange >= current.cost + 1)
                    {
                        enemeyInRangePositions.Add(newPos);
                        newCost = 99999; //Can't move through enemies
                        queue.Add(new MovementPath { cost = newCost, position = newPos });
                    }
                    else if (newCost <= MoveDistance && !walkableTilePositions.Contains(newPos))
                    {
                        walkableTilePositions.Add(newPos);
                        queue.Add(new MovementPath { cost = newCost, position = newPos });
                    }
                }
            }
            queue.RemoveAt(0);
        }

        DrawWalkableTiles();
    }

    private void DrawWalkableTiles()
    {
        IEnumerable<Vector3Int> unitPositions = FindObjectsOfType<Unit>()
            //.Where(x => x.PlayerColor == PlayerColor)
            .Select(x => x.GetTilePosition());

        foreach (Vector3Int pos in walkableTilePositions)
        {
            if (!unitPositions.Contains(pos)) {
                var highlight = Instantiate(Highlight, pos, Quaternion.identity);
                highlight.SetColor(HighlightTileType.Move);
            }
        }
        foreach (Vector3Int pos in enemeyInRangePositions)
        {
            var highlight = Instantiate(Highlight, pos, Quaternion.identity);
            highlight.SetColor(HighlightTileType.Attack);
        }
    }

    public void ResetHighlightedTiles()
    {
        foreach (HighlightTile highlight in FindObjectsOfType<HighlightTile>())
            Destroy(highlight.gameObject);
        walkableTilePositions.Clear();
        enemeyInRangePositions.Clear();
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

    public void Move(Vector3Int movePosition)
    {
        transform.position = movePosition + PositionOffset;
        _manager.SetSelectedUnit(null);
    }

    public Vector3Int GetTilePosition()
    {
        return BaseTileMap.WorldToCell(transform.position);
    }
}
