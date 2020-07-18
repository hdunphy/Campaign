using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar
{
    private readonly Dictionary<Vector3Int, Vector3Int?> cameFrom;
    private readonly Dictionary<Vector3Int, int> costSoFar;
    private readonly PriorityQueue<Vector3Int> Frontier;
    private readonly Tilemap map;
    private readonly List<Vector3Int> directions = new List<Vector3Int>
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };

    public AStar(Tilemap map)
    {
        Frontier = new PriorityQueue<Vector3Int>();
        cameFrom = new Dictionary<Vector3Int, Vector3Int?>();
        costSoFar = new Dictionary<Vector3Int, int>();
        this.map = map;
    }

    /// <summary>
    /// Gets Path from start node to end node on the current map.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="goal"></param>
    /// <returns>
    /// List of nodes to travel to get from start to end node
    /// </returns>
    public List<Vector3Int> GetPath(Vector3Int start, Vector3Int goal, List<Vector3Int> unitPositions)
    {
        costSoFar.Clear();
        cameFrom.Clear();
        Frontier.Clear();
        FindPath(start, goal, unitPositions);

        List<Vector3Int> path = new List<Vector3Int>();
        Vector3Int nextMove = goal;
        while(cameFrom[nextMove] != null)
        {
            path.Add(nextMove);
            nextMove = (Vector3Int)cameFrom[nextMove];
        }

        path.Reverse();
        return path;
    }

    /// <summary>
    /// Finds path from start to goal on a given map.
    /// </summary>
    /// <param name="start"></param>
    /// Starting node to search from
    /// <param name="goal"></param>
    /// End node for final state (should always be reachable)
    private void FindPath(Vector3Int start, Vector3Int goal, List<Vector3Int> unitPositions)
    {
        cameFrom.Add(start, null);
        costSoFar.Add(start, 0);
        Frontier.Put(start, 0);

        while (!Frontier.IsEmpty())
        {
            var current = Frontier.Get();
            if (current == goal)
                break;

            foreach (Vector3Int possibleMove in GetPossibleMoves(current))
            {
                int newCost = GetNewCost(current, possibleMove, unitPositions);
                if (!costSoFar.ContainsKey(possibleMove) || newCost < costSoFar[possibleMove])
                {
                    AddValueIfMissing(costSoFar, possibleMove, newCost);
                    costSoFar[possibleMove] = newCost;
                    int priority = newCost + PathFinding.ManhattanDistance(possibleMove, goal);
                    Frontier.Put(possibleMove, priority);
                    AddValueIfMissing(cameFrom, possibleMove, current);
                }
            }
        }
    }

    private int GetNewCost(Vector3Int current, Vector3Int possibleMove, List<Vector3Int> unitPositions)
    {
        int newCost = costSoFar[current] + map.GetTile<MovementTile>(possibleMove).MovementCost;
        if (unitPositions.Contains(possibleMove))
            newCost += Unit.MOVEMENT_COST;
        return newCost;
    }

    //Utilities function to add a value to dictionary. 
    //If key exists then update the value 
    //else add the key value pair
    private void AddValueIfMissing<K, V>(Dictionary<K, V> dictionary, K key, V value)
    {
        if (dictionary.ContainsKey(key))
            dictionary[key] = value;
        else
            dictionary.Add(key, value);
    }

    /// <summary>
    /// Get all possible moves from the current vector 3
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private List<Vector3Int> GetPossibleMoves(Vector3Int current)
    {
        List<Vector3Int> possibleMoves = new List<Vector3Int>();

        foreach (Vector3Int dir in directions)
        {
            Vector3Int newDirection = current + dir;
            if (map.GetTile<MovementTile>(newDirection) != null)
                possibleMoves.Add(newDirection);
        }
        return possibleMoves;
    }

    //Not used but gets the distance of the line between the two points
    private double Heuristic(Vector3Int currentPosition, Vector3Int goal)
    {
        return Math.Sqrt(
            Math.Pow(currentPosition.x - goal.x, 2) +
            Math.Pow(currentPosition.y - goal.y, 2));
    }
}
