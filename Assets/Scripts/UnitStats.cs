using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "ScriptableObjects/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{
    public UnitType UnitType;
    public int MoveDistance;
    public int AttackRange;
    public float MoveSpeed;
    public int AttackDamage;
    public int Armor;
    public int Health;
}

public enum UnitType { Swords, Archer, Spear, Knight }