using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerColor { Blue, Red }

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
        EventManager.Instance.SelectUnit += OnSelectedUnit;
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectUnit -= OnSelectedUnit;
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
        selectedUnit = null;
        EventManager.Instance.OnResetHighlightedTileTrigger();

        int playerColorIndex = (int)CurrentPlayerTurn + 1;
        var playerColors = Enum.GetValues(typeof(PlayerColor)).Cast<PlayerColor>().ToList();
        if (playerColorIndex >= playerColors.Count())
            playerColorIndex = 0;
        CurrentPlayerTurn = playerColors[playerColorIndex];

        EventManager.Instance.OnEndTurnTrigger(CurrentPlayerTurn);
    }

    private void OnSelectedUnit(Unit unit)
    {
        if (selectedUnit == unit)
            selectedUnit = null;
        else
            selectedUnit = unit;
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
