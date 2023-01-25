using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CellStateMode 
{
    ALIVE,
    DEAD
}
public class Cell : MonoBehaviour
{
    #region Expose

    [SerializeField] Color _cellColor;
    [SerializeField] Material _aliveMaterial, _deadMaterial;

    #endregion

    #region Public

    [HideInInspector]
    public int _aliveNeighborsCount;
    [HideInInspector]
    public (int line, int column) _position;
    [HideInInspector]
    public CellsGrid _grid;

    public bool IsAlive()
    {
        return (_currentState == CellStateMode.ALIVE);
    }
    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _renderer = GetComponentInChildren<MeshRenderer>();
    }

    void Start()
    {
        //_currentState = CellStateMode.DEAD;
        Death();
    }

    void Update()
    {
        
    }


    #endregion

    #region Methods
    
    public void Birth()
    {
        if (_currentState != CellStateMode.ALIVE)
        {
            _currentState= CellStateMode.ALIVE;
            UpdateGraphics();
        }
    }

    public void Death()
    {
        if (_currentState != CellStateMode.DEAD)
        {
            _currentState = CellStateMode.DEAD;
            UpdateGraphics();
        }
    }

    void UpdateGraphics()
    {
        switch (_currentState)
        {
            case CellStateMode.ALIVE:
                _renderer.material = _aliveMaterial;
                break;
            case CellStateMode.DEAD:
                _renderer.material = _deadMaterial;
                break;
            default:
                break;
        }
    }

    #endregion

    #region Mouse Method

    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0))
        {
            Birth();
            _grid.AddAliveCell(this);
        }
        else if (Input.GetMouseButton(1))
        {
            Death();
            _grid.RemoveAliveCell(this);
        }
    }

    #endregion

    #region Private & Protected

    private MeshRenderer _renderer;
    private CellStateMode _currentState;

    #endregion
}
