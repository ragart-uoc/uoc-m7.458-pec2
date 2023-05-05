using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>EnemyChildListeners</c> represents the child listeners of the enemy.
    /// </summary>
    public class EnemyChildListeners : MonoBehaviour
    {
        /// <summary>
        /// Method <c>OnFootstep</c> is called when the enemy makes a footstep.
        /// </summary>
        /// <param name="animationEvent">The animation event.</param>
        private void OnFootstep(AnimationEvent animationEvent)
        {
            transform.parent.GetComponent<Enemy>().OnFootstep(animationEvent);
        }
    }
}
