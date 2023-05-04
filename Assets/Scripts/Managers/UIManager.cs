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
        /// <value>Property <c>healthBar</c> represents the health bar of the player.</value>
        public Image healthBar;
        
        /// <value>Property <c>shieldBar</c> represents the shield bar of the player.</value>
        public Image shieldBar;
        
        /// <value>Property <c>crosshair</c> represents the crosshair of the current weapon.</value>
        public Image crosshairSrite;

        /// <value>Property <c>weaponSprite</c> represents the avatar of the current weapon.</value>
        public Image weaponSprite;

        /// <value>Property <c>clipAmmoText</c> represents the text of the remaining ammo in the clip.</value>
        public TextMeshProUGUI clipAmmoText;

        /// <value>Property <c>totalAmmoText</c> represents the text of the total remaining ammo.</value>
        public TextMeshProUGUI totalAmmoText;
        
        /// <value>Property <c>messageText</c> represents the text of the message.</value>
        public TextMeshProUGUI messageText;
        
        /// <value>Property <c>pauseMenu</c> represents the pause menu.</value>
        public GameObject pauseMenu;
        
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
        /// Method <c>UpdateWeaponUI</c> updates the weapon UI.
        /// </summary>
        public void UpdateWeaponUI(int clipAmmo, int totalAmmo)
        {
            clipAmmoText.text = clipAmmo.ToString();
            totalAmmoText.text = totalAmmo.ToString();
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
        /// Method <c>TogglePauseMenu</c> toggles the pause menu.
        /// </summary>
        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }
}
