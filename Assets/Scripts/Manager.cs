using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerColor { Red, Blue }

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    private Unit selectedUnit;

    private void Awake()
    {
        Instance = this;
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


}
