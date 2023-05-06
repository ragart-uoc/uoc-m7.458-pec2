using UnityEngine;
using PEC2.Entities;

namespace PEC2.EnemyStates
{
    /// <summary>
    /// Class <c>AttackState</c> represents the attack state of the enemy.
    /// </summary>
    public class AttackState : IEnemyState
    {
        /// <value>Property <c>_enemy</c> represents the enemy.</value>
        private readonly Enemy _enemy;
        
        /// <value>Property <c>_player</c> represents the player.</value>
        private readonly Player _player;
        
        /// <value>Property <c>_timeSinceSeenPlayer</c> represents the time since the player was seen.</value>
        private float _timeSinceSeenPlayer;
        
        /// <value>Property <c>_actualTimeBetweenShots</c> represents the actual time between shots.</value>
        private float _actualTimeBetweenShots;
        
        /// <summary>
        /// Method <c>AttackState</c> is the constructor for the class.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        public AttackState(Enemy enemy)
        {
            _enemy = enemy;
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        /// <summary>
        /// Method <c>UpdateState</c> updates the state.
        /// </summary>
        public void UpdateState()
        {
            // Increase the timer between shots
            _actualTimeBetweenShots += Time.deltaTime;
            
            // Get the player component
            var playerPosition = _player.transform.position;
                
            // Calculate the look direction
            var lookDirection = playerPosition - _enemy.transform.position;
            lookDirection = new Vector3(lookDirection.x, 0.0f, lookDirection.z);
                
            // Rotate the enemy
            _enemy.transform.rotation = Quaternion.FromToRotation(
                Vector3.forward,
                lookDirection);
            
            // Emit a raycast to check if the player is in sight
            if (Physics.Raycast(
                    new Ray(
                        new Vector3(_enemy.transform.position.x,
                            _enemy.transform.position.y + 0.5f,
                            _enemy.transform.position.z),
                        lookDirection),
                    out var hit)
                && hit.collider.CompareTag("Player"))
            {
                _timeSinceSeenPlayer = 0.0f;
                
                // If the enemy is a sniper, draw the laser
                if (_enemy.isSniper)
                {
                    _enemy.laserLine.SetPosition(0, _enemy.transform.position + Vector3.up * _enemy.shootHeight);
                    _enemy.laserLine.SetPosition(1, playerPosition);
                    _enemy.laserLine.enabled = true;
                }

                // If the time between shots is greater than the fire rate, shoot
                if (_actualTimeBetweenShots >= _enemy.fireRate)
                {
                    _actualTimeBetweenShots = 0.0f;
                    _enemy.audioSource.Play();
                    _player.TakeDamage(_enemy.damageForce);
                
                    // Spawn the bullet
                    var bullet = Object.Instantiate(_enemy.bulletPrefab, _enemy.bulletSpawnPoint.position, Quaternion.identity);
                    bullet.GetComponent<Rigidbody>().AddForce(lookDirection * 20f, ForceMode.Impulse);
                }
            } else {
                _enemy.laserLine.enabled = false;
                _timeSinceSeenPlayer += Time.deltaTime;
            }
            
            // If the player was not seen, go to alert state
            if (_timeSinceSeenPlayer >= 5f)
            {
                _timeSinceSeenPlayer = 0;
                GoToAlertState();
            }
        }

        /// <summary>
        /// Method <c>Impact</c> is called when the enemy is hit.
        /// </summary>
        public void Impact() {
            GoToAttackState();
        }

        /// <summary>
        /// Method <c>GoToAlertState</c> changes the state to alert.
        /// </summary>
        public void GoToAlertState() {
            _enemy.laserLine.enabled = false;
            _enemy.currentState = _enemy.alertState;
        }

        /// <summary>
        /// Method <c>GoToAttackState</c> changes the state to attack.
        /// </summary>
        public void GoToAttackState() {}

        /// <summary>
        /// Method <c>GoToPatrolState</c> changes the state to patrol.
        /// </summary>
        public void GoToPatrolState() {}
        
        /// <summary>
        /// Method <c>GoToDyingState</c> changes the state to dying.
        /// </summary>
        public void GoToDyingState() {
            _enemy.laserLine.enabled = false;
            _enemy.currentState = _enemy.dyingState;
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the enemy enters a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerEnter(Collider col) {}

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called when the enemy stays in a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerStay(Collider col)
        {
            if (!col.CompareTag("Player"))
                return;
            
            // Look at the player
            var lookDirection = col.transform.position - _enemy.transform.position;
            _enemy.transform.rotation = Quaternion.FromToRotation(
                Vector3.forward,
                new Vector3(lookDirection.x, 0.0f, lookDirection.z));
        }
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerExit(Collider col) {
            if (!col.CompareTag("Player"))
                return;
            _enemy.laserLine.enabled = false;
            GoToAlertState();
        }
    }
}