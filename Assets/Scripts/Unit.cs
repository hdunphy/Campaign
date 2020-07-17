using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Unit : MonoBehaviour
{
    public Tilemap BaseTileMap;
    public PlayerColor PlayerColor;
    public UnitStats Stats;
    public static int MOVEMENT_COST = 9999;

    private Manager _manager;
    private bool isSelected;
    private bool hasMoved;
    private int MoveDistance;
    private int AttackRange;
    private float MoveSpeed;
    private Vector3 PositionOffset;
    private HighlightTile Highlight;
    private List<Vector3Int> walkableTilePositions;
    private List<Vector3Int> enemeyInRangePositions;

    // Start is called before the first frame update
    void Start()
    {
        MoveDistance = Stats.MoveDistance;
        AttackRange = Stats.AttackRange;
        MoveSpeed = Stats.MoveSpeed;
        PositionOffset = Stats.PositionOffset;
        Highlight = Stats.HighlightTile;

        hasMoved = false;

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
        if (Manager.Instance.GetCurrentPlayer() == PlayerColor)
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
    }

    internal void SetSelected(bool isSelected)
    {
        this.isSelected = isSelected;
        if (this.isSelected)
        {
            if (!hasMoved)
            {
                PathFinding.Instance.SetTileHighlights(transform.position, MoveDistance, AttackRange, PlayerColor);
            }
        }
        else
        {
            PathFinding.Instance.ResetHighlightedTiles();
        }
    }

    public void Move(Vector3Int movePosition)
    {
        _manager.SetSelectedUnit(null);
        StartCoroutine(StartPathFindingMovement(movePosition));
    }

    private IEnumerator StartMovement(Vector3 movePosition)
    {
        while (transform.position.x != movePosition.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(movePosition.x, transform.position.y), MoveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != movePosition.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, movePosition.y), MoveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
    }

    private IEnumerator StartPathFindingMovement(Vector3 movePosition)
    {
        List<Vector3Int> path = PathFinding.Instance.FindPathBetween(transform.position, movePosition);
        foreach(Vector3 step in path)
        {
            while(transform.position != step)
            {
                transform.position = Vector2.MoveTowards(transform.position, step, MoveSpeed * Time.deltaTime);
                yield return null;
            }
        }

        hasMoved = true;
    }

    public Vector3Int GetTilePosition()
    {
        return BaseTileMap.WorldToCell(transform.position);
    }

    public void EndOfTurn()
    {
        hasMoved = false;
    }
}
