using UnityEngine;
using PEC2.Entities;

namespace PEC2.DoorBehaviours
{
    /// <summary>
    /// Class <c>LockedBehaviour</c> represents a locked door.
    /// </summary>
    public class LockedBehaviour : IDoorBehaviour
    {
        /// <value>Property <c>_door</c> represents the door.</value>
        private readonly Door _door;
        
        /// <value>Property <c>isAlreadyOpen</c> represents if the door is already open.</value>
        private bool _isAlreadyOpen;

        /// <summary>
        /// Method <c>LockedBehaviour</c> is the constructor of the class.
        /// </summary>
        /// <param name="door">The door.</param>
        public LockedBehaviour(Door door)
        {
            _door = door;
        }
        
        /// <summary>
        /// Method <c>UpdateBehaviour</c> updates the behaviour of the door using the parent update method.
        /// </summary>
        public void UpdateBehaviour() {}
        
        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        public void OnTriggerEnter(Collider col) {
            if (!col.gameObject.CompareTag("Player"))
                return;
            if (!_isAlreadyOpen)
            {
                var keycardColor = _door.keycardColor switch
                {
                    KeycardProperties.Colors.Blue => "Blue",
                    KeycardProperties.Colors.Green => "Green",
                    KeycardProperties.Colors.Red => "Red",
                    _ => throw new System.ArgumentException("Invalid keycard color."),
                };
                if (!col.gameObject.GetComponent<Player>().HasKeycard(_door.keycardColor))
                {
                    _door.uiManager.UpdateMessageText(keycardColor + " keycard needed", 2f);
                    return;
                }
                else
                {
                    _door.uiManager.UpdateMessageText(keycardColor + " keycard used", 2f);
                    _isAlreadyOpen = true;
                }
            }
            _door.isOpening = true;
            _door.isClosing = false;
            
        }
        
        /// <summary>
        /// Method <c>OnTriggerStay</c> is called once per frame for every Collider other that is touching the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        public void OnTriggerStay(Collider col) {
            if (!col.gameObject.CompareTag("Player") || !_isAlreadyOpen)
                return;
            _door.isOpening = true;
            _door.isClosing = false;
        }
        
        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the Collider other has stopped touching the trigger.
        /// </summary>
        public void OnTriggerExit(Collider col) {
            if (!col.gameObject.CompareTag("Player") || !_isAlreadyOpen)
                return;
            _door.isOpening = false;
            _door.isClosing = true;
            
        }
    }
}
