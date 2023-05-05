using UnityEngine;
using PEC2.DoorBehaviours;
using PEC2.Managers;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>DoorAutomatic</c> represents an automatic door.
    /// </summary>
    public class Door : MonoBehaviour
    {
        /// <value>Property <c>DoorTypes</c> represents the types of doors.</value>
        public enum DoorTypes
        {
            Automatic,
            Locked,
            Timed
        }
        
        /// <value>Property <c>doorType</c> represents the type of the door.</value>
        public DoorTypes doorType;
        
        /// <value>Property <c>keycardColor</c> represents the color of the keycard.</value>
        public KeycardProperties.Colors keycardColor;

        /// <value>Property <c>timerButtonPressed</c> represents if the timer button is pressed.</value>
        public bool timerButtonPressed;

        /// <value>Property <c>maxTime</c> represents the max time that the door is open.</value>
        public float maxTime = 5f;
        
        /// <value>Property <c>leftDoor</c> represents the left door.</value>
        public Transform leftDoor;
        
        /// <value>Property <c>rightDoor</c> represents the right door.</value>
        public Transform rightDoor;
        
        /// <value>Property <c>leftClosedLocation</c> represents the location of the left door when is closed.</value>
        public Transform leftClosedLocation;
        
        /// <value>Property <c>rightClosedLocation</c> represents the location of the right door when is closed.</value>
        public Transform rightClosedLocation;
        
        /// <value>Property <c>leftOpenLocation</c> represents the location of the left door when is open.</value>
        public Transform leftOpenLocation;
        
        /// <value>Property <c>rightOpenLocation</c> represents the location of the right door when is open.</value>
        public Transform rightOpenLocation;

        /// <value>Property <c>_leftDoorRenderer</c> represents the renderer of the left door.</value>
        private Renderer _leftDoorRenderer;
        
        /// <value>Property <c>_rightDoorRenderer</c> represents the renderer of the right door.</value>
        private Renderer _rightDoorRenderer;
        
        /// <value>Property <c>_originalLeftDoorColor</c> represents the original color of the left door.</value>
        private Color _originalLeftDoorColor;
        
        /// <value>Property <c>_originalRightDoorColor</c> represents the original color of the right door.</value>
        private Color _originalRightDoorColor;

        /// <value>Property <c>doorSpeed</c> represents how fast the door opens and closes.</value>
        public float doorSpeed = 1.0f;

        /// <value>Property <c>isOpening</c> represents if the door is opening.</value>
        [HideInInspector]
        public bool isOpening;
        
        /// <value>Property <c>isClosing</c> represents if the door is closing.</value>
        [HideInInspector]
        public bool isClosing;

        /// <value>Property <c>doorBehaviour</c> represents the behaviour of the door.</value>
        private IDoorBehaviour _doorBehaviour;
        
        /// <value>Property <c>_distance</c> represents the distance between the door and the location.</value>
        private Vector3 _distance;
        
        /// <value>Property <c>uiManager</c> represents the UI manager.</value>
        [HideInInspector]
        public UIManager uiManager;

        /// <value>Property <c>BaseColor</c> is the ID of the base color.</value>
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        /// <summary>
        /// Method <c>Start</c> is called when the script instance is being loaded.
        /// </summary>
        private void Start()
        {
            // Get the UI manager
            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            
            // Get the door renderer
            _leftDoorRenderer = leftDoor.GetComponent<Renderer>();
            _rightDoorRenderer = rightDoor.GetComponent<Renderer>();
            
            // Get the original door color
            _originalLeftDoorColor = _leftDoorRenderer.material.color;
            _originalRightDoorColor = _rightDoorRenderer.material.color;

            // Get the door behaviour
            _doorBehaviour = GetDoorBehaviour(doorType);
            OnSelectDoorBehaviour();
        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            // Update the door behaviour
            _doorBehaviour.UpdateBehaviour();
            
            // If the door is opening
            if (isOpening)
            {
                _distance = leftDoor.position - leftOpenLocation.position;

                if (_distance.magnitude < 0.001f)
                {
                    isOpening = false;
                    leftDoor.localPosition = leftOpenLocation.localPosition;
                    rightDoor.localPosition = rightOpenLocation.localPosition;
                }
                else
                {
                    leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition,
                        leftOpenLocation.localPosition,
                        Time.deltaTime * doorSpeed);
                    rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition,
                        rightOpenLocation.localPosition,
                        Time.deltaTime * doorSpeed);
                }
            }
            // If the door is closing
            else if (isClosing)
            {
                _distance = leftDoor.position - leftClosedLocation.position;

                if (_distance.magnitude < 0.001f)
                {
                    isClosing = false;
                    leftDoor.localPosition = leftClosedLocation.localPosition;
                    rightDoor.localPosition = rightClosedLocation.localPosition;
                }
                else
                {
                    leftDoor.localPosition = Vector3.Lerp(leftDoor.localPosition,
                        leftClosedLocation.localPosition,
                        Time.deltaTime * doorSpeed);
                    rightDoor.localPosition = Vector3.Lerp(rightDoor.localPosition,
                        rightClosedLocation.localPosition,
                        Time.deltaTime * doorSpeed);
                }
            }
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the Collider other enters the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this event.</param>
        private void OnTriggerEnter(Collider col)
        {
            _doorBehaviour.OnTriggerEnter(col);
        }

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called once per frame for every Collider other that is touching the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this event.</param>
        private void OnTriggerStay(Collider col)
        {
            _doorBehaviour.OnTriggerStay(col);
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the Collider other has stopped touching the trigger.
        /// </summary>
        /// <param name="col">The other Collider involved in this event.</param>
        private void OnTriggerExit(Collider col)
        {
            _doorBehaviour.OnTriggerExit(col);
        }

        /// <summary>
        /// Method <c>GetDoorBehaviour</c> gets the door behaviour.
        /// </summary>
        /// <param name="type">The type of the door.</param>
        private IDoorBehaviour GetDoorBehaviour(DoorTypes type)
        {
            IDoorBehaviour behaviour = type switch
            {
                DoorTypes.Automatic => new AutomaticBehaviour(this),
                DoorTypes.Locked => new LockedBehaviour(this),
                DoorTypes.Timed => new TimedBehaviour(this),
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };
            return behaviour;
        }

        private void OnSelectDoorBehaviour()
        {
            switch (doorType)
            {
                case DoorTypes.Automatic:
                    _leftDoorRenderer.material.SetColor(BaseColor, _originalLeftDoorColor);
                    _rightDoorRenderer.material.SetColor(BaseColor, _originalRightDoorColor);
                    break;
                case DoorTypes.Locked:
                    var doorColor = keycardColor switch
                    {
                        KeycardProperties.Colors.Blue => Color.blue,
                        KeycardProperties.Colors.Green => Color.green,
                        KeycardProperties.Colors.Red => Color.red,
                        _ => throw new System.ArgumentOutOfRangeException(nameof(keycardColor), keycardColor, null)
                    };
                    _leftDoorRenderer.material.SetColor(BaseColor, doorColor);
                    _rightDoorRenderer.material.SetColor(BaseColor, doorColor);
                    break;
                case DoorTypes.Timed:
                    _leftDoorRenderer.material.SetColor(BaseColor, _originalLeftDoorColor);
                    _rightDoorRenderer.material.SetColor(BaseColor, _originalRightDoorColor);
                    break;
            }
        }
    }
}
