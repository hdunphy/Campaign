using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{
    public HighlightTile HighlightTile;
    public int MoveDistance;
    public int AttackRange;
    public float MoveSpeed;
    public Vector3 PositionOffset;
    public int Attack;
    public int Defense;
    public int Health;
}
