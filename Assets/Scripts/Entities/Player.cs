using PEC2.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>Player</c> represents the player in the game.
    /// </summary>
    public class Player : MonoBehaviour
    {
        #region Player Settings
        
            /// <value>Property <c>health</c> represents the health of the player.</value>
            public float health = 100.0f;
            
            /// <value>Property <c>maxHealth</c> represents the maximum health of the player.</value>
            public float maxHealth = 100.0f;
            
            /// <value>Property <c>shield</c> represents the shield of the player.</value>
            public float shield = 100.0f;
            
            /// <value>Property <c>maxShield</c> represents the maximum shield of the player.</value>
            public float maxShield = 100.0f;
            
        #endregion
        
        #region Player Weapon Settings
        
            /// <value>Property <c>weapons</c> represents the weapons of the player.</value>
            public Weapon[] weapons;
            
            /// <value>Property <c>_currentWeaponIndex</c> represents the index of the current weapon.</value>
            private int _currentWeaponIndex;
            
            /// <value>Property <c>CurrentWeapon</c> represents the current weapon.</value>
            private Weapon CurrentWeapon => weapons[_currentWeaponIndex].GetComponent<Weapon>();

            /// <value>Property <c>numberOfDecals</c> represents the number of decals.</value>
            public int numberOfDecals = 30;
        
            /// <value>Property <c>decals</c> represents the decals.</value>
            public GameObject[] decals;
        
            /// <value>Property <c>decalIndex</c> represents the index of the decal.</value>
            public int decalIndex;
            
        #endregion
        
        #region Player Keys Settings
        
            /// <value>Property <c>blueKeyObtained</c> represents wether the blue keycard is obtained.</value>
            public bool blueKeyObtained;
            
            /// <value>Property <c>greenKeyObtained</c> represents wether the green keycard is obtained.</value>
            public bool greenKeyObtained;
            
            /// <value>Property <c>redKeyObtained</c> represents wether the red keycard is obtained.</value>
            public bool redKeyObtained;

        #endregion

        #region Component References

            /// <value>Property <c>_cameraRoot</c> represents the camera root of the player.</value>
            private Transform _cameraRoot;
        
            /// <value>Property <c>_uiManager</c> represents the UI manager of the game.</value>
            private UIManager _uiManager;
            
        #endregion

        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            _cameraRoot = gameObject.transform.Find("PlayerCameraRoot");
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            decals = new GameObject[numberOfDecals];
        }
        
        /// <summary>
        /// Method <c>TakeDamage</c> is called when the player takes damage.
        /// </summary>
        /// <param name="damage">The amount of damage taken.</param>
        public void TakeDamage(float damage)
        {
            shield -= damage;
            if (shield < 0.0f)
            {
                health += shield;
                shield = 0.0f;
            }
            _uiManager.UpdatePlayerUI(health, shield);
        }
        
        /// <summary>
        /// Method <c>Heal</c> is called when the player heals.
        /// </summary>
        /// <param name="multiplier">The multiplier of the heal.</param>
        public void RestoreHealth(float multiplier)
        {
            health += maxHealth * multiplier;
            health = Mathf.Clamp(health, 0.0f, maxHealth);
            _uiManager.UpdatePlayerUI(health, shield);
        }
        
        /// <summary>
        /// Method <c>RestoreShield</c> is called when the player restores shield.
        /// </summary>
        /// <param name="multiplier">The multiplier of the shield.</param>
        public void RestoreShield(float multiplier)
        {
            shield += maxShield * multiplier;
            shield = Mathf.Clamp(shield, 0.0f, maxShield);
            _uiManager.UpdatePlayerUI(health, shield);
        }
        
        /// <summary>
        /// Method <c>RestoreAmmo</c> is called when the player restores ammo.
        /// </summary>
        /// <param name="multiplier">The multiplier of the ammo.</param>
        public void RestoreAmmo(float multiplier)
        {
            foreach (var weapon in weapons)
            {
                if (weapon.isObtained)
                    weapon.RestoreAmmo(multiplier);
            }
        }
        
        /// <summary>
        /// Method <c>OnSwitchWeapon</c> is called when the player switches weapon.
        /// </summary>
        /// <param name="value">The value of the wheel.</param>
        private void OnSwitchWeapon(InputValue value)
        {
            // Get the value of the wheel
            var axis = value.Get<float>();
            
            SwitchWeapon(axis > 0.0f);
        }

        /// <summary>
        /// Method <c>OnFire</c> is called when the player fires.
        /// </summary>
        /// <param name="value">The value of the fire button.</param>
        private void OnFire(InputValue value)
        {
            CurrentWeapon.OnFire(value);
        }

        /// <summary>
        /// Method <c>OnReload</c> is called when the player relaods.
        /// </summary>
        /// <param name="value">The value of the reload button.</param>
        private void OnReload(InputValue value)
        {
            CurrentWeapon.OnReload(value);
        }

        /// <summary>
        /// Method <c>SwitchWeapon</c> switches the weapon of the player.
        /// </summary>
        /// <param name="increase">Whether to increase or decrease the weapon index.</param>
        private void SwitchWeapon(bool increase)
        {
            while (true)
            {
                // Switch to the next or previous weapon
                _currentWeaponIndex = (_currentWeaponIndex + (increase ? 1 : -1) + weapons.Length) % weapons.Length;

                // Switch to the next one if the current one is not obtained or has no ammo
                if (_currentWeaponIndex != 0)
                    if (!weapons[_currentWeaponIndex].isObtained || weapons[_currentWeaponIndex].clipAmmo <= 0)
                        continue;

                // Activate the game object of the current weapon and deactivate the others
                for (var i = 0; i < weapons.Length; i++)
                {
                    weapons[i].gameObject.SetActive(i == _currentWeaponIndex);
                }
                
                break;
            }
        }
        
        /// <summary>
        /// Method <c>AddRecoil</c> adds recoil to the player.
        /// </summary>
        public void AddRecoil(float recoil)
        {
            _cameraRoot.Rotate(Vector3.left, recoil);
        }
        
        /// <summary>
        /// Method <c>GetKeycard</c> gets a keycard.
        /// </summary>
        /// <param name="keycard"></param>
        public void GetKeycard(string keycard)
        {
            switch (keycard)
            {
                case "Blue":
                    blueKeyObtained = true;
                    break;
                case "Green":
                    greenKeyObtained = true;
                    break;
                case "Red":
                    redKeyObtained = true;
                    break;
            }
            _uiManager.UpdateKeycardUI(blueKeyObtained, greenKeyObtained, redKeyObtained);
        }
    }
}
