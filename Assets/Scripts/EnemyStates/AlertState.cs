using UnityEngine;
using PEC2.Entities;

namespace PEC2.EnemyStates
{
    /// <summary>
    /// CLass <c>AlertState</c> is the class that represents the alert state.
    /// </summary>
    public class AlertState : IEnemyState
    {
        /// <value>Property <c>_enemy</c> represents the enemy.</value>
        private Enemy _enemy;
        
        /// <value>Property <c>_currentRotationTime</c> represents the current rotation time.</value>
        private float _currentRotationTime;
        
        /// <summary>
        /// Method <c>AlerState</c> is the constructor for the class.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        public AlertState(Enemy enemy)
        {
            _enemy = enemy;
        }

        /// <summary>
        /// Method <c>UpdateState</c> updates the state.
        /// </summary>
        public void UpdateState()
        {
            _enemy.transform.rotation *=
                Quaternion.Euler(0.0f, 360.0f * Time.deltaTime / _enemy.rotationTime, 0.0f);

            if (_currentRotationTime >= _enemy.rotationTime)
            {
                _currentRotationTime = 0.0f;
                GoToPatrolState();
            }
            else
            {
                if (Physics.Raycast(
                        new Ray(
                            new Vector3(_enemy.transform.position.x,
                                _enemy.transform.position.y + 0.5f,
                                _enemy.transform.position.z),
                            _enemy.transform.forward * _enemy.viewDistance),
                        out var hit))
                {
                    if (hit.collider.CompareTag("Player"))
                        GoToAttackState();
                }
                _currentRotationTime += Time.deltaTime;
            }
        }

        /// <summary>
        /// Method <c>Impact</c> is called when the enemy is hit.
        /// </summary>
        public void Impact()
        {
            
            GoToAttackState();
        }

        /// <summary>
        /// Method <c>GoToAlertState</c> changes the state to alert.
        /// </summary>
        public void GoToAlertState() {}

        /// <summary>
        /// Method <c>GoToAttackState</c> changes the state to attack.
        /// </summary>
        public void GoToAttackState()
        {
            _enemy.currentState = _enemy.attackState;
        }

        /// <summary>
        /// Method <c>GoToPatrolState</c> changes the state to patrol.
        /// </summary>
        public void GoToPatrolState()
        {
            _enemy.navMeshAgent.isStopped = false;
            _enemy.currentState = _enemy.patrolState;
        }
        
        /// <summary>
        /// Method <c>GoToDyingState</c> changes the state to dying.
        /// </summary>
        public void GoToDyingState() {
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
        public void OnTriggerStay(Collider col) {}
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerExit(Collider col) {}
    }
}
