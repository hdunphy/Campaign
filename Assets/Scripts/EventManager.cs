using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(this);
    }

    public event Action<Unit> SelectUnit;
    public event Action ResetHighlightedTiles;
    public event Action<Unit> SetHightlightedTiles;

    public void OnSelectUnitTrigger(Unit selectedUnit)
    {
        SelectUnit?.Invoke(selectedUnit);
    }

    public void OnResetHighlightedTileTrigger()
    {
        ResetHighlightedTiles?.Invoke();
    }

    public void OnSetHighlightedTileTrigger(Unit unit)
    {
        SetHightlightedTiles?.Invoke(unit);
    }
}
