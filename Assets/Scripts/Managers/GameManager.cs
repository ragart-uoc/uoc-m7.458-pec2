using UnityEngine;
using UnityEngine.SceneManagement;

namespace PEC2.Managers
{
    /// <summary>
    /// Class <c>GameManager</c> represents the game manager.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <value>Property <c>Instance</c> represents the singleton instance of the class.</value>
        private static GameManager _instance;
        
        /// <value>Property <c>uiManager</c> represents the UI manager of the game.</value>
        public UIManager uiManager;
        
        /// <value>Property <c>isPaused</c> represents if the game is paused.</value>
        private bool _isPaused;

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            uiManager.ClearMessageText();
        }
        
        /// <summary>
        /// Method <c>TogglePause</c> is used to pause the game.
        /// </summary>
        public void TogglePause()
        {
            _isPaused = !_isPaused;
            // Pause or resume time and audio
            Time.timeScale = _isPaused ? 0 : 1;
            AudioListener.pause = _isPaused;
            // Show or hide the pause menu
            uiManager.TogglePauseMenu();
        }

        /// <summary>
        /// Method <c>TooglePause</c> is used to pause the game.
        /// </summary>
        public void GoToMainMenu()
        {
            Destroy(gameObject);
            SceneManager.LoadScene("MainMenu");
        }

        /// <summary>
        /// Method <c>QuitGame</c> quits the game.
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}
