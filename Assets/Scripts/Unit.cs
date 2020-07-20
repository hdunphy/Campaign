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
    private bool hasAttacked;

    private int MoveDistance;
    //private int MoveDistanceLeft;
    private int AttackRange;
    private float MoveSpeed;
    private int CurrentHealth;
    private int AttackDamage;
    private int Armor;
    private int TotalHealth;

    // Start is called before the first frame update
    void Start()
    {
        MoveDistance = Stats.MoveDistance;
        AttackRange = Stats.AttackRange;
        MoveSpeed = Stats.MoveSpeed;
        CurrentHealth = TotalHealth = Stats.Health;
        AttackDamage = Stats.AttackDamage;
        Armor = Stats.Armor;

        hasMoved = false;
        hasAttacked = false;

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
        hasMoved = true;
        //_manager.SetSelectedUnit(null);
        EventManager.Instance.OnSelectUnitTrigger(null);
        EventManager.Instance.OnResetHighlightedTileTrigger();
        StartCoroutine(StartPathFindingMovement(movePosition));
    }

    public void MoveThenAttack(Vector3Int movePosition, Vector3Int attackPosition)
    {
        hasMoved = true;
        //_manager.SetSelectedUnit(null);
        EventManager.Instance.OnSelectUnitTrigger(null);
        EventManager.Instance.OnResetHighlightedTileTrigger();
        StartCoroutine(StartMoveThenAttack(movePosition, attackPosition));
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
    }

    private IEnumerator StartMoveThenAttack(Vector3Int movePosition, Vector3Int attackPosition)
    {
        yield return StartPathFindingMovement(movePosition);
        Attack(attackPosition);
    }

    public void Attack(Vector3Int highlightPosition)
    {
        hasAttacked = true;
        Debug.Log("Attack at: " + highlightPosition.ToString());
        EventManager.Instance.OnResetHighlightedTileTrigger();

        Unit enemy = FindObjectsOfType<Unit>().FirstOrDefault(x => x.GetTilePosition() == highlightPosition);

        int enemyDamage = AttackDamage - enemy.Armor;
        int myDamage = Mathf.FloorToInt((enemy.AttackDamage - Armor) * .75f);
        int enemyDistance = PathFinding.ManhattanDistance(GetTilePosition(), highlightPosition);

        if(enemyDamage >= 1)
        {
            enemy.TakeDamage(enemyDamage);
        }
        if(myDamage >= 1 && enemy.AttackRange >= enemyDistance)
        {
            TakeDamage(myDamage);
        }
        if(enemy.CurrentHealth <= 0)
        {
            Destroy(enemy.gameObject);
            Move(enemy.GetTilePosition());
        }
        if(CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        /*

        if(enemy.health <= 0)
        {
            GetWalkableTiles();
            Destroy(enemy.gameObject);
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            gameMaster.RemoveStatsPanel(enemy);
        }
        if(health <= 0)
        {
            gameMaster.ResetTiles();
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameMaster.RemoveStatsPanel(this);
            Destroy(this.gameObject);
        }*/
    }

    public Vector3Int GetTilePosition()
    {
        return BaseTileMap.WorldToCell(transform.position);
    }

    public void EndOfTurn()
    {
        hasMoved = false;
        hasAttacked = false;
    }

    public int GetAttackRange()
    {
        return AttackRange;
    }

    public int GetMoveDistance()
    {
        return MoveDistance;
    }

    public int GetArmor()
    {
        return Armor;
    }

    public int GetAttackDamage()
    {
        return AttackDamage;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
    }
}
