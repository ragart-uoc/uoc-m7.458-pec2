using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Method <c>DoorSwitch</c> represents a door switch.
    /// </summary>
    public class DoorSwitch : MonoBehaviour
    {
        /// <value>Property <c>door</c> represents the door.</value>
        public Door door;
        
        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider col)
        {
            if (!col.gameObject.CompareTag("Player"))
                return;
            door.timerButtonPressed = true;
            door.uiManager.UpdateMessageText("A door opened somewhere", 2f);
        }
    }
}
