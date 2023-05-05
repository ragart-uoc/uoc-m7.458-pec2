using System;
using UnityEngine;
using PEC2.Managers;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>Keycard</c> represents the keycard in the game.
    /// </summary>
    public class Keycard : MonoBehaviour
    {
        /// <value>Property <c>keycardColor</c> represents the color of the keycard.</value>
        public KeycardProperties.Colors keycardColor;
        
        /// <value>Property <c>_uiManager</c> represents the UI manager of the game.</value>
        private UIManager _uiManager;

        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            // Get the UI manager
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            // Turn the power up using the Y axis
            transform.Rotate(0, 100 * Time.deltaTime, 0);
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider col)
        {
            // Check if the player has collided with the keycard
            if (!col.CompareTag("Player"))
                return;
            var player = col.GetComponent<Player>();
            
            // Check the color of the keycard
            switch (keycardColor)
            {
                case KeycardProperties.Colors.Blue:
                    _uiManager.UpdateMessageText("You got the blue keycard!", 2.0f);
                    break;
                case KeycardProperties.Colors.Green:
                    _uiManager.UpdateMessageText("You got the green keycard!", 2.0f);
                    break;
                case KeycardProperties.Colors.Red:
                    _uiManager.UpdateMessageText("You got the red keycard!", 2.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            player.GetKeycard(keycardColor);

            // Destroy the keycard
            Destroy(gameObject);
        }
    }
}
