using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPath
{
    public Vector3Int position;
    public int cost;
    public MovementPath CameFrom;

    public override bool Equals(object obj)
    {
        return obj is MovementPath path &&
               position.Equals(path.position) &&
               cost == path.cost &&
               EqualityComparer<MovementPath>.Default.Equals(CameFrom, path.CameFrom);
    }

    public override int GetHashCode()
    {
        int hashCode = -1499852887;
        hashCode = hashCode * -1521134295 + position.GetHashCode();
        hashCode = hashCode * -1521134295 + cost.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<MovementPath>.Default.GetHashCode(CameFrom);
        return hashCode;
    }
}
