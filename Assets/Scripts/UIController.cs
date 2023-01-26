using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace GameOfLife
{
    public class UIController : MonoBehaviour
    {
        #region Show in inspector

        [SerializeField] private Simulation _gameController;
        [SerializeField] private CellsGrid _grid;

        [Header("UI elements")]

        [SerializeField] private Slider _lineCountSlider;
        [SerializeField] private TMP_Text _lineCountText;
        [SerializeField] private Slider _columnCountSlider;
        [SerializeField] private TMP_Text _columnCountText;

        #endregion


        #region Init

        private void Start()
        {
            _lineCountSlider.value = _grid._lineCount;
            _columnCountSlider.value = _grid._columnCount;
        }


        #endregion


        #region UI Callbacks

        public void OnNextStepClicked()
        {
            _gameController.SimulationStep();
        }

        public void OnAutoPlayToggled(bool value)
        {
            if (value)
            {
                _gameController.ResumeSimulation();
            }
            else
            {
                _gameController.PauseSimulation();
            }
        }

        public void OnSpeedChanged(float value)
        {
            _gameController.ChangeSimulationSpeed(value);
        }

        public void OnLineCountChanged(float value)
        {
            _lineCountText.text = ((int)value).ToString("D2");
            _grid._lineCount = (int)value;
        }

        public void OnColumnCountChanged(float value)
        {
            _columnCountText.text = ((int)value).ToString("D2");
            _grid._columnCount = (int)value;
        }

        public void OnGenerateGrid()
        {
            _gameController.ResetSimulation();
        }

        #endregion
    }
}