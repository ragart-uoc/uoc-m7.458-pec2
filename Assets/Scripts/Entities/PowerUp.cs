using System;
using System.Collections;
using UnityEngine;
using PEC2.Managers;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>PowerUp</c> represents the power up in the game.
    /// </summary>
    public class PowerUp : MonoBehaviour
    {
        /// <value>Property <c>PowerUpTypes</c> represents the types of power ups.</value>
        public enum PowerUpTypes
        {
            Health,
            Shield,
            Ammo
        }
        
        /// <value>Property <c>powerUpType</c> represents the type of the power up.</value>
        public PowerUpTypes powerUpType;
        
        /// <value>Property <c>_uiManager</c> represents the UI manager of the game.</value>
        private UIManager _uiManager;
        
        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            // Get the UI manager
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            // Begin to fade after 5 seconds
            Invoke(nameof(Fade), 5.0f);
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
            
            // Check the type of the power up
            switch (powerUpType)
            {
                case PowerUpTypes.Health:
                    if (player.health >= player.maxHealth)
                    {
                        _uiManager.UpdateMessageText("Already at full health", 2.0f);
                        return;
                    }
                    _uiManager.UpdateMessageText("You got some health!", 2.0f);
                    player.RestoreHealth(0.25f);
                    break;
                case PowerUpTypes.Shield:
                    if (player.shield >= player.maxShield)
                    {
                        _uiManager.UpdateMessageText("Already at full shield", 2.0f);
                        return;
                    }
                    _uiManager.UpdateMessageText("You got some shield", 2.0f);
                    player.RestoreShield(0.25f);
                    break;
                case PowerUpTypes.Ammo:
                    _uiManager.UpdateMessageText("You got some ammo", 2.0f);
                    player.RestoreAmmo(0.25f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // Destroy the power up
            Destroy(gameObject);
        }

        /// <summary>
        /// Method <c>Fade</c> fades the power up.
        /// </summary>
        private void Fade()
        {
            // Fade the power up
            StartCoroutine(Blink());
            
            // Destroy the power up after 5 seconds
            Destroy(gameObject, 5.0f);
        }

        /// <summary>
        /// Method <c>Blink</c> blinks the power up.
        /// </summary>
        private IEnumerator Blink()
        {
            // Blink all child renderers
            var childRenderers = GetComponentsInChildren<Renderer>();
            while (gameObject != null)
            {
                foreach (var childRenderer in childRenderers)
                {
                    childRenderer.enabled = false;
                }
                yield return new WaitForSeconds(0.1f);
                foreach (var childRenderer in childRenderers)
                {
                    childRenderer.enabled = true;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
