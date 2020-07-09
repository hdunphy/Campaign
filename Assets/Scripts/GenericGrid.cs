using UnityEngine;
using UnityEditor;
using System;

public class GenericGrid <TGridType>
{
    private int width, height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridType[,] gridArray;

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs { public int x; public int y; }

    public GenericGrid(int width, int height, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gridArray = new TGridType[width, height];
    }

    

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetValue(int x, int y, TGridType value)
    {
        if(x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    public TGridType GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
            return default;
    }
}