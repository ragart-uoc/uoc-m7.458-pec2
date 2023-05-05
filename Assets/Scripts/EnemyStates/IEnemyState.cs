using UnityEngine;

namespace PEC2.EnemyStates
{
    /// <summary>
    /// Interface <c>IEnemyState</c> is the interface for the enemy states.
    /// </summary>
    public interface IEnemyState
    {
        /// <summary>
        /// Method <c>UpdateState</c> updates the state.
        /// </summary>
        void UpdateState();
        
        /// <summary>
        /// Method <c>Impact</c> is called when the enemy is hit.
        /// </summary>
        void Impact();
        
        /// <summary>
        /// Method <c>GoToAlertState</c> changes the state to alert.
        /// </summary>
        void GoToAlertState();
        
        /// <summary>
        /// Method <c>GoToAttackState</c> changes the state to attack.
        /// </summary>
        void GoToAttackState();
        
        /// <summary>
        /// Method <c>GoToPatrolState</c> changes the state to patrol.
        /// </summary>
        void GoToPatrolState();
        
        /// <summary>
        /// Method <c>GoToDyingState</c> changes the state to dying.
        /// </summary>
        void GoToDyingState();
        
        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the enemy enters a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        void OnTriggerEnter(Collider col);
        
        /// <summary>
        /// Method <c>OnTriggerStay</c> is called when the enemy stays in a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        void OnTriggerStay(Collider col);
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        void OnTriggerExit(Collider col);
    }
}
