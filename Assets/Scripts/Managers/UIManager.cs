using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PEC2.Managers
{
    /// <summary>
    /// Method <c>UIManager</c> manages the UI of the game.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <value>Property <c>_instance</c> represents the singleton instance of the class.</value>
        private static UIManager _instance;
        
        /// <value>Property <c>healthBar</c> represents the health bar of the player.</value>
        public Image healthBar;
        
        /// <value>Property <c>shieldBar</c> represents the shield bar of the player.</value>
        public Image shieldBar;
        
        /// <value>Property <c>keycardBlue</c> represents the blue keycard.</value>
        public Image keycardBlue;
        
        /// <value>Property <c>keycardGreen</c> represents the green keycard.</value>
        public Image keycardGreen;
        
        /// <value>Property <c>keycardRed</c> represents the red keycard.</value>
        public Image keycardRed;

        /// <value>Property <c>weaponSprite</c> represents the avatar of the current weapon.</value>
        public Image weaponSprite;

        /// <value>Property <c>clipAmmoText</c> represents the text of the remaining ammo in the clip.</value>
        public TextMeshProUGUI clipAmmoText;

        /// <value>Property <c>totalAmmoText</c> represents the text of the total remaining ammo.</value>
        public TextMeshProUGUI totalAmmoText;
        
        /// <value>Property <c>messageText</c> represents the text of the message.</value>
        public TextMeshProUGUI messageText;
        
        /// <value>Property <c>crosshair</c> represents the crosshair of the current weapon.</value>
        public Image crosshairSrite;
        
        /// <value>Property <c>timerText</c> represents the text of the timer.</value>
        public TextMeshProUGUI timerText;
        
        /// <value>Property <c>pauseMenu</c> represents the pause menu.</value>
        public GameObject pauseMenu;

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
        /// Method <c>UpdatePlayerUI</c> updates the player UI.
        /// </summary>
        /// <param name="health">The health of the player.</param>
        /// <param name="shield">The shield of the player.</param>
        public void UpdatePlayerUI(float health, float shield)
        {
            healthBar.fillAmount = health / 100.0f;
            shieldBar.fillAmount = shield / 100.0f;
        }
        
        /// <summary>
        /// Method <c>UpdateKeycardUI</c> updates the keycard UI.
        /// </summary>
        /// <param name="blue">The blue keycard obtained status.</param>
        /// <param name="green">The green keycard obtained status.</param> 
        /// <param name="red">The red keycard obtained status.</param>
        public void UpdateKeycardUI(bool blue, bool green, bool red)
        {
            keycardBlue.color = new Color(keycardBlue.color.r, keycardBlue.color.g, keycardBlue.color.b, blue ? 1.0f : keycardBlue.color.a);
            keycardGreen.color = new Color(keycardGreen.color.r, keycardGreen.color.g, keycardGreen.color.b, green ? 1.0f : keycardGreen.color.a);
            keycardRed.color = new Color(keycardRed.color.r, keycardRed.color.g, keycardRed.color.b, red ? 1.0f : keycardRed.color.a);
        }
        
        /// <summary>
        /// Method <c>UpdateWeaponUI</c> updates the weapon UI.
        /// </summary>
        public void UpdateWeaponUI(int clipAmmo, int totalAmmo)
        {
            clipAmmoText.text = clipAmmo.ToString();
            totalAmmoText.text = totalAmmo.ToString();
        }
        
        /// <summary>
        /// Method <c>UpdateWeaponSprite</c> updates the weapon sprite.
        /// </summary>
        /// <param name="sprite">The sprite of the current weapon.</param>
        public void UpdateWeaponSprite(Sprite sprite)
        {
            this.weaponSprite.sprite = sprite;
        }
        
        /// <summary>
        /// Method <c>UpdateMessageText</c> updates the message text.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        public void UpdateMessageText(string message)
        {
            messageText.text = message;
        }
        
        /// <summary>
        /// Method <c>UpdateMessageText</c> updates the message text.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <param name="duration">The duration of the message.</param>
        public void UpdateMessageText(string message, float duration)
        {
            messageText.text = message;
            Invoke(nameof(ClearMessageText), duration);
        }
        
        /// <summary>
        /// Method <c>ClearMessageText</c> clears the message text.
        /// </summary>
        public void ClearMessageText()
        {
            messageText.text = String.Empty;
        }
        
        /// <summary>
        /// Method <c>UpdateCrosshairSprite</c> updates the crosshair sprite.
        /// </summary>
        /// <param name="crosshair">The sprite of the current crosshair.</param>
        public void UpdateCrosshairSprite(Image crosshair)
        {
            this.crosshairSrite.sprite = crosshair.sprite;
        }

        /// <summary>
        /// Method <c>UpdateTimerText</c> updates the timer text.
        /// </summary>
        /// <param name="time">The time to be displayed.</param>
        /// <param name="active">Wether the timer is active or not.</param>
        public void UpdateTimerText(float time, bool active)
        {
            timerText.gameObject.SetActive(active);
            var minutes = Mathf.FloorToInt(time / 60.0f);
            var seconds = Mathf.FloorToInt(time % 60.0f);
            var milliseconds = Mathf.FloorToInt((time * 100.0f) % 100.0f);
            timerText.text = $"{minutes:00}:{seconds:00}:{milliseconds:0000}";
        }

        /// <summary>
        /// Method <c>TogglePauseMenu</c> toggles the pause menu.
        /// </summary>
        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
