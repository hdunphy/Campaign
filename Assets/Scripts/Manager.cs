using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerColor { Red, Blue }

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    private Unit selectedUnit;
    private PlayerColor CurrentPlayerTurn;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CurrentPlayerTurn = PlayerColor.Blue;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }
    }

    private void EndTurn()
    {
        CurrentPlayerTurn = CurrentPlayerTurn == PlayerColor.Blue ? PlayerColor.Red : PlayerColor.Blue;
        foreach(Unit unit in FindObjectsOfType<Unit>())
        {
            unit.EndOfTurn();
        }
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (selectedUnit != null)
            selectedUnit.SetSelected(false);

        selectedUnit = unit;

        if (selectedUnit != null)
            selectedUnit.SetSelected(true);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    public PlayerColor GetCurrentPlayer()
    {
        return CurrentPlayerTurn;
    }
}
