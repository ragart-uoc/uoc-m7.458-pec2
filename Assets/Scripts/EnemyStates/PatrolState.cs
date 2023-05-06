using UnityEngine;
using PEC2.Entities;

namespace PEC2.EnemyStates
{
    /// <summary>
    /// Class <c>PatrolState</c> is the class that defines the patrol state.
    /// </summary>
    public class PatrolState : IEnemyState
    {
        /// <value>Property <c>_enemy</c> represents the enemy.</value>
        private readonly Enemy _enemy;
        
        /// <value>Property <c>m_PreviousWayPoint</c> represents the previous way point.</value>
        private int _previousWayPoint;
        
        /// <value>Property <c>m_NextWayPoint</c> represents the next way point.</value>
        private int _nextWayPoint;
        
        /// <summary>
        /// Method <c>PatrolState</c> is the constructor for the class.
        /// </summary>
        /// <param name="enemy">The enemy.</param>
        public PatrolState(Enemy enemy)
        {
            _enemy = enemy;
        }

        /// <summary>
        /// Method <c>UpdateState</c> updates the state.
        /// </summary>
        public void UpdateState()
        {
            // If no way points are defined, return.
            if (!_enemy.isNavigator)
                return;
            
            // Define the next way point.
            _enemy.navMeshAgent.destination = _enemy.wayPoints[_nextWayPoint].position;
            
            // If the path is pending, return.
            if (_enemy.navMeshAgent.pathPending)
                return;
            
            // If the enemy is close to the way point, go to the next one.
            if (_enemy.navMeshAgent.remainingDistance <= _enemy.navMeshAgent.stoppingDistance)
                _nextWayPoint = (_nextWayPoint + 1) % _enemy.wayPoints.Length;
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
        public void GoToAlertState()
        {
            if (_enemy.isNavigator)
                _enemy.navMeshAgent.isStopped = true;
            _enemy.currentState = _enemy.alertState;
        }

        /// <summary>
        /// Method <c>GoToAttackState</c> changes the state to attack.
        /// </summary>
        public void GoToAttackState()
        {
            if (_enemy.isNavigator)
                _enemy.navMeshAgent.isStopped = true;
            _enemy.currentState = _enemy.attackState;
        }
        
        /// <summary>
        /// Method <c>GoToPatrolState</c> changes the state to patrol.
        /// </summary>
        public void GoToPatrolState() {}
        
        /// <summary>
        /// Method <c>GoToDyingState</c> changes the state to dying.
        /// </summary>
        public void GoToDyingState() {
            if (_enemy.isNavigator)
                _enemy.navMeshAgent.isStopped = true;
            _enemy.currentState = _enemy.dyingState;
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the enemy enters a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player"))
                return;
            if (_enemy.alwaysLookAtPlayer)
                GoToAttackState();
            else
                GoToAlertState();
        }

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called when the enemy stays in a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerStay(Collider col)
        {
            if (!col.CompareTag("Player"))
                return;
            if (_enemy.alwaysLookAtPlayer)
                GoToAttackState();
            else
                GoToAlertState();
        }
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        public void OnTriggerExit(Collider col) {}
    }
}