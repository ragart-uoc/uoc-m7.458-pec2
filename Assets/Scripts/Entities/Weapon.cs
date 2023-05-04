using PEC2.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>Weapon</c> represents the weapon in the game.
    /// </summary>
    public class Weapon : MonoBehaviour
    {
        #region Weapon Settings
        
            /// <value>Property <c>clipAmmo</c> represents the ammo in the clip.</value>
            [Header("Weapon Settings")]
            public int clipAmmo;

            /// <value>Property <c>maxClipAmmo</c> represents the maximum ammo in the clip.</value>
            public int maxClipAmmo;
            
            /// <value>Property <c>totalAmmo</c> represents the total ammo.</value>
            public int totalAmmo;
            
            /// <value>Property <c>maxTotalAmmo</c> represents the maximum total ammo.</value>
            public int maxTotalAmmo;
            
            /// <value>Property <c>fireRate</c> represents the fire rate.</value>
            public float fireRate;
            
            /// <value>Property <c>range</c> represents the range.</value>
            public float range;
            
            /// <value>Property <c>damage</c> represents the damage.</value>
            public float damage;
            
            /// <value>Property <c>recoil</c> represents the recoil.</value>
            public float recoil;
            
            /// <value>Property <c>reloadTime</c> represents the reload time.</value>
            public float reloadTime;
            
            /// <value>Property <c>nextFire</c> represents the next fire.</value>
            public float nextFire;
        
        #endregion
        
        #region Weapon States
        
            /// <value>Property <c>isObtained</c> represents whether the weapon is obtained.</value>
            [Header("Weapon States")]
            public bool isObtained;
            
            /// <value>Property <c>isAutomatic</c> represents whether the weapon is automatic.</value>
            public bool isAutomatic;
            
            /// <value>Property <c>isFiring</c> represents whether the weapon is firing.</value>
            public bool isFiring;
            
            /// <value>Property <c>isReloading</c> represents whether the weapon is reloading.</value>
            public bool isReloading;
            
        #endregion
        
        #region Component References

            /// <value>Property <c>sprite</c> represents the sprite of the weapon.</value>
            [Header("Component References")]
            public Sprite sprite;
            
            /// <value>Property <c>bulletPrefab</c> represents the bullet prefab of the weapon.</value>
            public GameObject bulletPrefab;
            
            /// <value>Property <c>bulletSpawn</c> represents the bullet spawn of the weapon.</value>
            public Transform bulletSpawn;
            
            /// <value>Property <c>decalPrefab</c> represents the decal prefab of the weapon.</value>
            public GameObject decalPrefab;
            
            /// <value>Property <c>fireSound</c> represents the fire sound of the weapon.</value>
            public AudioClip fireSound;
            
            /// <value>Property <c>reloadSound</c> represents the reload sound of the weapon.</value>
            public AudioClip reloadSound;

            /// <value>Property <c>_audioSource</c> represents the audio source of the weapon.</value>
            private AudioSource _audioSource;
            
            /// <value>Property <c>_player</c> represents the player.</value>
            private Player _player;

            /// <value>Property <c>_uiManager</c> represents the UI manager.</value>
            private UIManager _uiManager;

            /// <value>Property <c>_mainCamera</c> represents the main camera.</value>
            private Camera _mainCamera;

            /// <value>Property <c>_isMainCameraNotNull</c> represents whether the main camera is not null.</value>
            private bool _isMainCameraNotNull;

        #endregion

        /// <summary>
        /// Method <c>Awake</c> is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            _audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Method <c>Start</c> is called before the first frame update.
        /// </summary>
        private void Start()
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
            _mainCamera = Camera.main;
            _isMainCameraNotNull = _mainCamera != null;
        }

        /// <summary>
        /// Method <c>OnEnable</c> is called when the script instance is being loaded.
        /// </summary>
        private void OnEnable()
        {
            isFiring = false;
            isReloading = false;
            _uiManager.UpdateWeaponSprite(sprite);
            _uiManager.UpdateWeaponUI(clipAmmo, totalAmmo);
        }
        
        /// <summary>
        /// Method <c>OnDisable</c> is called when the script instance is being destroyed.
        /// </summary>
        private void OnDisable()
        {
            isFiring = false;
            isReloading = false;
        }
        
        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            if (isFiring)
                Fire();
        }

        /// <summary>
        /// Method <c>OnFire</c> is called when the player fires.
        /// </summary>
        /// <param name="value">The value of the fire button.</param>
        public void OnFire(InputValue value)
        {
            if (isAutomatic)
                isFiring = value.isPressed;
            else if (!value.isPressed)
                Fire();
        }
        
        /// <summary>
        /// Method <c>OnReload</c> is called when the player reloads.
        /// </summary>
        /// <param name="value">The value of the reload button.</param>
        public void OnReload(InputValue value)
        {
            if (value.isPressed)
                Reload();
        }
        
        /// <summary>
        /// Method <c>Fire</c> fires the weapon.
        /// </summary>
        private void Fire()
        {
            // Do not fire if reloading, no ammo or not enough time has passed
            if (isReloading || clipAmmo <= 0 || Time.time < nextFire)
                return;
            
            // Set initial target position
            var mainCameraTransform = _mainCamera.transform;
            var targetPosition = mainCameraTransform.position + mainCameraTransform.forward * range;

            // Fire using raycast
            if (_isMainCameraNotNull 
                && Physics.Raycast(_mainCamera.ViewportPointToRay(
                    new Vector3(0.5f, 0.5f, 0.0f)), out var hit))
            {
                targetPosition = hit.point;
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                {
                    Destroy(_player.decals[_player.decalIndex]);
                    _player.decals[_player.decalIndex] = Instantiate(decalPrefab, hit.point + hit.normal * 0.01f,
                        Quaternion.FromToRotation(Vector3.forward, -hit.normal));
                    _player.decalIndex = (_player.decalIndex + 1) % _player.decals.Length;
                }
                //if (hit.transform.CompareTag("Enemy"))
                    //hit.transform.gameObject.GetComponent<EnemyAI>().Hit(10.0f);
            }
            
            // Spawn the bullet
            var bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
            bullet.transform.LookAt(targetPosition);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 100.0f, ForceMode.Impulse);
            
            // Add recoil
            _player.AddRecoil(recoil);
            
            // Play fire sound
            _audioSource.PlayOneShot(fireSound);
            
            // Save the time of the next fire
            nextFire = Time.time + fireRate;
            
            // Decrease the ammo in the clip, update the UI and try to reload if empty
            clipAmmo--;
            _uiManager.UpdateWeaponUI(clipAmmo, totalAmmo);
            if (clipAmmo <= 0)
                Reload();
        }

        /// <summary>
        /// Method <c>Reload</c> reloads the weapon.
        /// </summary>
        private void Reload()
        {
            // Do not reload if already reloading, clip is full or no ammo
            if (isReloading || clipAmmo == maxClipAmmo || totalAmmo == 0)
                return;
            
            // Reload started
            isReloading = true;
            
            // Play reload sound
            _audioSource.PlayOneShot(reloadSound);
            
            // Invoke ReloadComplete method
            Invoke(nameof(ReloadComplete), reloadTime);
        }
        
        /// <summary>
        /// Method <c>ReloadComplete</c> is called when the reload is complete.
        /// </summary>
        private void ReloadComplete()
        {
            // Reload is complete
            isReloading = false;

            // Check how much ammo is needed to fill the clip and substract it from the total ammo
            var ammoNeeded = maxClipAmmo - clipAmmo;
            ammoNeeded = Mathf.Clamp(ammoNeeded, 0, totalAmmo);
            clipAmmo += ammoNeeded;
            totalAmmo -= ammoNeeded;
            
            // Update the UI
            _uiManager.UpdateWeaponUI(clipAmmo, totalAmmo);
        }

    }
}
