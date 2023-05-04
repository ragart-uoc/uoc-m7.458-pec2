using UnityEngine;

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
    }
}
