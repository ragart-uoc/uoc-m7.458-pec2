using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>Bullet</c> represents the bullet in the game.
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        /// <value>Property <c>duration</c> represents the duration of the bullet.</value>
        public float duration = 5.0f;

        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            Destroy(gameObject, duration);
        }
    }
}
