using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>MovingPlatform</c> represents a moving platform in the game.
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        /// <value>Property <c>PlatformTypes</c> represents the types of platforms.</value>
        public enum PlatformTypes
        {
            Horizontal,
            Vertical
        }
        
        /// <value>Property <c>platformType</c> represents the type of the platform.</value>
        public PlatformTypes platformType;
        
        /// <value>Property <c>Directions</c> represents the possible directions of the platform.</value>
        public enum Directions
        {
            RightOrUp,
            LeftOrDown
        }
        
        /// <value>Property <c>direction</c> represents the direction of the platform.</value>
        public Directions direction;
        
        /// <value>Property <c>speed</c> represents the speed of the platform.</value>
        public float speed = 3.0f;
        
        /// <value>Property <c>distance</c> represents the distance of the platform.</value>
        public float distance = 5.0f;
        
        /// <value>Property <c>startPosition</c> represents the start position of the platform.</value>
        private Vector3 _startPosition;
        
        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            // Get the start position
            _startPosition = transform.position;
        }

        /// <summary>
        /// Method <c>FixedUpdate</c> is called every fixed framerate frame.
        /// </summary>
        private void FixedUpdate()
        {
            var platformDirection = (direction == Directions.RightOrUp) ? 1 : -1;   
            var position = transform.position;
            // Check the type of the platform
            position = platformType switch
            {
                PlatformTypes.Horizontal =>
                    // Move the platform horizontally
                    new Vector3(
                        _startPosition.x + platformDirection * distance * Mathf.PingPong(Time.time * speed, 1.0f),
                        position.y,
                        position.z),
                PlatformTypes.Vertical =>
                    // Move the platform vertically
                    new Vector3(
                        position.x,
                        _startPosition.y + platformDirection * distance * Mathf.PingPong(Time.time * speed, 1.0f),
                        position.z),
                _ => throw new ArgumentOutOfRangeException()
            };
            transform.position = position;
            Physics.SyncTransforms();
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        private void OnTriggerEnter(Collider col)
        {
            // Check if the player has collided with the platform
            if (!col.transform.CompareTag("Player"))
                return;
            
            // Set the player as a child of the platform
            col.transform.SetParent(transform);
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this collision.</param>
        private void OnTriggerExit(Collider col)
        {
            // Check if the player has collided with the platform
            if (!col.transform.CompareTag("Player"))
                return;
            
            // Remove the player as a child of the platform
            col.transform.SetParent(null);
        }
    }
}
