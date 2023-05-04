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
        /// <value>Property <c>KeycardColors</c> represents the colors of the keycard.</value>
        public enum KeycardColors
        {
            Blue,
            Green,
            Red
        }
        
        /// <value>Property <c>keycardColor</c> represents the color of the keycard.</value>
        public KeycardColors keycardColor;
        
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
            // Turn the keycard using the Z axis
            transform.Rotate(0, 0, 100 * Time.deltaTime);
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
                case KeycardColors.Blue:
                    _uiManager.UpdateMessageText("You got the blue keycard!", 2.0f);
                    player.GetKeycard("Blue");
                    break;
                case KeycardColors.Green:
                    _uiManager.UpdateMessageText("You got the green keycard!", 2.0f);
                    player.GetKeycard("Green");
                    break;
                case KeycardColors.Red:
                    _uiManager.UpdateMessageText("You got the red keycard!", 2.0f);
                    player.GetKeycard("Red");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Destroy the keycard
            Destroy(gameObject);
        }
    }
}
