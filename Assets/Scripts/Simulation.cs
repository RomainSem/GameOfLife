using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulation : MonoBehaviour
{
    #region Exposed

    [SerializeField] bool _isPlaying;
    [Range(0f, 5f)]
    [SerializeField] float _simulationStepDelay;
    
    #endregion

    #region Unity Lyfecycle

    private void Awake()
    {
        _cellGrid = GetComponent<CellsGrid>();
    }

    void Start()
    {

    }

    void Update()
    {
        // On sort de l'update si _isPlaying est false
        if (_isPlaying && Time.timeSinceLevelLoad > _nextSimulationStepTime)
        {
            _cellGrid.SimulationStep();
            _nextSimulationStepTime = Time.timeSinceLevelLoad + _simulationStepDelay;
        }

    }


    #endregion

    #region Methods

    public void SimulationStep()
    {
        _cellGrid.SimulationStep();
    }

    public void PauseSimulation()
    {
        _isPlaying = false;
    }

    public void ResumeSimulation()
    {
        _isPlaying = true;
    }

    public void ChangeSimulationSpeed(float delay)
    {
        _simulationStepDelay = Mathf.Max(0, delay);
    }

    public void ResetSimulation()
    {
        _cellGrid.InitGrid();
    }


    #endregion

    #region Private & Protected

    private CellsGrid _cellGrid;
    private float _nextSimulationStepTime;

    #endregion
}
