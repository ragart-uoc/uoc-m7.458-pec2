using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>EndLine</c> represents the end line of the game.
    /// </summary>
    public class EndLine : MonoBehaviour
    {
        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                other.GetComponent<Player>().GameOver();
        }
    }
}
