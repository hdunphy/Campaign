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

    // Start is called before the first frame update
    void Start()
    {
        MoveDistance = Stats.MoveDistance;
        AttackRange = Stats.AttackRange;
        MoveSpeed = Stats.MoveSpeed;

        hasMoved = false;

        _manager = Manager.Instance;

        //Set sprite color for which team
        if (PlayerColor == PlayerColor.Blue)
            GetComponent<SpriteRenderer>().color = Color.blue;
        else if (PlayerColor == PlayerColor.Red)
            GetComponent<SpriteRenderer>().color = Color.red;

        EventManager.Instance.SelectUnit += OnSelectedUnit;
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectUnit -= OnSelectedUnit;
    }

    //public void SelectUnit()
    private void OnMouseDown()
    {
        if (_manager.GetCurrentPlayer() == PlayerColor)
        {
            Debug.Log("OnMouseDown in selected unit");
            //Trigger isSelected
            EventManager.Instance.OnSelectUnitTrigger(this);
        }
    }

    private void OnSelectedUnit(Unit selectedUnit)
    {
        if (selectedUnit == this)
        {
            EventManager.Instance.OnResetHighlightedTileTrigger();
            isSelected = !isSelected;
            if (isSelected && !hasMoved)
            {
                EventManager.Instance.OnSetHighlightedTileTrigger(this);
            }
        }
        else
        {
            isSelected = false;
        }
    }

    //public void SetSelected(bool isSelected)
    //{
    //    this.isSelected = isSelected;
    //    if (this.isSelected)
    //    {
    //        if (!hasMoved)
    //        {
    //            EventManager.Instance.OnSetHighlightedTileTrigger(this);
    //        }
    //    }
    //    else
    //    {
    //        EventManager.Instance.OnResetHighlightedTileTrigger();
    //    }
    //}

    public void Move(Vector3Int movePosition)
    {
        //_manager.SetSelectedUnit(null);
        EventManager.Instance.OnSelectUnitTrigger(null);
        EventManager.Instance.OnResetHighlightedTileTrigger();
        StartCoroutine(StartPathFindingMovement(movePosition));
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

    public void Attack(Vector3Int highlightPosition)
    {
        Debug.Log("Attack");
        //EventManager.Instance.OnResetHighlightedTileTrigger();
    }

    public Vector3Int GetTilePosition()
    {
        return BaseTileMap.WorldToCell(transform.position);
    }

    public void EndOfTurn()
    {
        hasMoved = false;
    }

    public int GetAttackRange()
    {
        return AttackRange;
    }

    public int GetMoveDistance()
    {
        return MoveDistance;
    }
}
