using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool _West, _North, _Est, _South;
    public bool _visited;

    public int xPos, zPos;

    public Cell (bool west, bool north, bool est, bool south, bool visited)
    {
        this._West = west;
        this._North = north;
        this._Est = est;
        this._South = south;
        this._visited = visited;
    }
}