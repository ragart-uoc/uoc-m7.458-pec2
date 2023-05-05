using UnityEngine;
using PEC2.Entities;

namespace PEC2.EnemyStates
{
    public class DyingState : IEnemyState
    {
        /// <value>Property <c>_enemy</c> represents the enemy.</value>
        private readonly Enemy _enemy;
        public DyingState(Enemy enemy)
        {
            _enemy = enemy;
        }

        /// <summary>
        /// Method <c>UpdateState</c> updates the state.
        /// </summary>
        public void UpdateState() {}

        /// <summary>
        /// Method <c>Impact</c> is called when the enemy is hit.
        /// </summary>
        public void Impact() {}

        /// <summary>
        /// Method <c>GoToAlertState</c> changes the state to alert.
        /// </summary>
        public void GoToAlertState() {}

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
        public void GoToDyingState() {}

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