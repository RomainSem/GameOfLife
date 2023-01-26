using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellsGrid : MonoBehaviour
{
    #region Expose

    [SerializeField] public int _lineCount;
    [SerializeField] public int _columnCount;

    [SerializeField] Cell _cellPrefab;

    [SerializeField] Cell[,] _cellGrid;

    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {

    }

    void Start()
    {
        InitGrid();
    }

    void Update()
    {

    }


    #endregion

    #region Methods

    public void InitGrid()
    {

        foreach (Cell cell1 in FindObjectsOfType<Cell>())
        {
            Destroy(cell1.gameObject);
        }
        // Initialisation tableau
        _cellGrid = new Cell[_columnCount, _lineCount];

        // Initialisation des listes
        _aliveCells= new List<Cell>();
        _candidateCells = new HashSet<Cell>();

        // Création de notre grille de cellules
        for (int line = 0; line < _lineCount; line++)
        {
            for (int column = 0; column < _columnCount; column++)
            {
                Vector2 cellPosition = new Vector2(column, line);
                Cell cell = Instantiate(_cellPrefab, cellPosition, Quaternion.identity, transform);

                _cellGrid[column, line] = cell;

                cell.name = $"Cell:({column},{line})";
                cell._position = (line, column);
                cell._grid = this;
            }
        }
        PositionCamera();
    }

    public void SimulationStep()
    {
        // Dans un premier temps on compte nos cellules
        CountAliveNeighborsToCandidate();

        // Dans un 2e temps on applique les règles de survie
        ApplyRulesForCandidates();
    }

    private void PositionCamera()
    {
        Camera camera = Camera.main;
        camera.transform.position = new Vector3(_columnCount * .5f, _lineCount * .5f, camera.transform.position.z);
        float targetHeight = _columnCount / camera.aspect;

        camera.orthographicSize = (Mathf.Max(_lineCount, targetHeight) + 4) * .5f; 
    }

    #endregion

    #region FirstStep Count Alive


    private void CountAliveNeighborsToCandidate()
    {
        // Remettre à 0 la liste
        _candidateCells.Clear();

        foreach (Cell cell in _aliveCells)
        {
            SendAliveSignalToNeighbors(cell);
            _candidateCells.Add(cell);
        }
    }

    private void SendAliveSignalToNeighbors(Cell cell)
    {
        SendAliveSignalToCell(cell._position.line +1, cell._position.column);
        SendAliveSignalToCell(cell._position.line +1, cell._position.column +1);
        SendAliveSignalToCell(cell._position.line, cell._position.column +1);
        SendAliveSignalToCell(cell._position.line -1, cell._position.column + 1);
        SendAliveSignalToCell(cell._position.line -1, cell._position.column);
        SendAliveSignalToCell(cell._position.line - 1 , cell._position.column - 1);
        SendAliveSignalToCell(cell._position.line, cell._position.column - 1);
        SendAliveSignalToCell(cell._position.line +1, cell._position.column - 1);
    }

    private void SendAliveSignalToCell(int line, int column)
    {
        if (IsValideCell(line, column))
        {
            Cell cell = _cellGrid[column, line];
            cell._aliveNeighborsCount++;
            _candidateCells.Add(cell);
        }
    }

    private bool IsValideCell(int column, int line)
    {
        return (line >= 0 && line < _lineCount && column >= 0 && column < _columnCount);
    }

    #endregion

    #region SecondStep Apply Rules

    private void ApplyRulesForCandidates()
    {
        _aliveCells.Clear();

        foreach (Cell cell in _candidateCells)
        {
            if (cell._aliveNeighborsCount == 3)
            {
                cell.Birth();
                _aliveCells.Add(cell);
            }
            else if (cell._aliveNeighborsCount == 2)
            {
                if (cell.IsAlive())
                {
                    _aliveCells.Add(cell);
                }
            }
            else
            {
                cell.Death();
            }
            cell._aliveNeighborsCount= 0;
        }
    }

    #endregion

    #region Mouse Methods

    public void AddAliveCell(Cell cell)
    {
        _aliveCells.Add(cell);
    }

    public void RemoveAliveCell(Cell cell)
    {
        _aliveCells.Remove(cell);
    }
    #endregion

    #region Private & Protected

    List<Cell> _aliveCells;
    HashSet<Cell> _candidateCells;

    #endregion
}
