using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
    private canvasScript canvasScriptRef;

    #region Attributes
    public int _width, _height;
    public VisualCell visualCellPrefab;
    public Cell[,] cells;
    private Vector2 _randomCellPos;
    private VisualCell visualCellInst;
    private List<CellAndRelativePosition> neighbors;
    #endregion
    #region Methods

    

    //public InputField MazeWidth;
    //static public int _width
    //public InputField MazeHeight;
    //static public int _height;

    void Start()
    {
        canvasScriptRef = GameObject.Find("Canvas").GetComponent<canvasScript>();
       

        _width = StaticWidth._width1;
        _height = StaticHeight._height1;

        //Debug.Log("                  " + _width);
       // Debug.Log("                  " + _height);

        cells = new Cell[_width, _height];
        Init();
    }
    void Init()
    {
        for (int i = 0; i < _width; i++)
        {
            for (int j = 0; j < _height; j++)
            {
                cells[i, j] = new Cell(false, false, false, false, false);
                cells[i, j].xPos = i;
                cells[i, j].zPos = j;
            }
        }
        RandomCell();
        InitVisualCell();
    }
    void RandomCell()
    {
        _randomCellPos = new Vector2((int)UnityEngine.Random.Range(0, _width), (int)UnityEngine.Random.Range(0, _height));
        GenerateMaze((int)_randomCellPos.x, (int)_randomCellPos.y);
    }
    void GenerateMaze(int x, int y)
    {
        Cell currentCell = currentCell = cells[x, y];
        neighbors = new List<CellAndRelativePosition>();
        if (currentCell._visited == true) return;
        currentCell._visited = true;
        if (x + 1 < _width && cells[x + 1, y]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x + 1, y], CellAndRelativePosition.Direction.East));
        }
        if (y + 1 < _height && cells[x, y + 1]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x, y + 1], CellAndRelativePosition.Direction.South));
        }
        if (x - 1 >= 0 && cells[x - 1, y]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x - 1, y], CellAndRelativePosition.Direction.West));
        }
        if (y - 1 >= 0 && cells[x, y - 1]._visited == false)
        {
            neighbors.Add(new CellAndRelativePosition(cells[x, y - 1], CellAndRelativePosition.Direction.North));
        }
        if (neighbors.Count == 0) return;
        neighbors.Shuffle();

        foreach(CellAndRelativePosition selectedcell in neighbors)
        {
            if(selectedcell.direction == CellAndRelativePosition.Direction.East)
            {
                if (selectedcell.cell._visited) continue;
                currentCell._Est = true;
                selectedcell.cell._West = true;
                GenerateMaze(x + 1, y);
            }
            else if (selectedcell.direction == CellAndRelativePosition.Direction.South)
            {
                if (selectedcell.cell._visited) continue;
                currentCell._South = true;
                selectedcell.cell._North = true;
                GenerateMaze(x, y + 1);
            }
            else if (selectedcell.direction == CellAndRelativePosition.Direction.West)
            {
                if (selectedcell.cell._visited) continue;
                currentCell._West = true;
                selectedcell.cell._Est = true;
                GenerateMaze(x - 1, y);
            }
            else if (selectedcell.direction == CellAndRelativePosition.Direction.North)
            {
                if (selectedcell.cell._visited) continue;
                currentCell._North = true;
                selectedcell.cell._South = true;
                GenerateMaze(x, y - 1);
            }
        }
    }
    void InitVisualCell()
    {
        foreach (Cell cell in cells)
        {
            visualCellInst = Instantiate(visualCellPrefab, new Vector3(cell.xPos * 3, 0, _height * 3f - cell.zPos * 3), Quaternion.identity) as VisualCell;
            visualCellInst.transform.parent = transform;
            visualCellInst._North.gameObject.SetActive(!cell._North);
            visualCellInst._South.gameObject.SetActive(!cell._South);
            visualCellInst._Est.gameObject.SetActive(!cell._Est);
            visualCellInst._West.gameObject.SetActive(!cell._West);
            visualCellInst.transform.name = cell.xPos.ToString() + "_" + cell.zPos.ToString();
        }
    }
    #endregion
}
