using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>EnemyChildTriggers</c> represents the child triggers of the enemy.
    /// </summary>
    public class EnemyChildTriggers : MonoBehaviour
    {
        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the enemy enters a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerEnter(Collider col)
        {
            transform.parent.GetComponent<Enemy>().currentState.OnTriggerEnter(col);
        }

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called when the enemy stays in a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerStay(Collider col)
        {
            transform.parent.GetComponent<Enemy>().currentState.OnTriggerStay(col);
        }
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerExit(Collider col)
        {
            transform.parent.GetComponent<Enemy>().currentState.OnTriggerExit(col);
        }
    }
}