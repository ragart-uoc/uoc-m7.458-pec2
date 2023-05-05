using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using PEC2.EnemyStates;

namespace PEC2.Entities
{
    /// <summary>
    /// Class <c>EnemyAI</c> is the main class for the enemy AI.
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Enemy States
        
            /// <value>Property <c>patrolState</c> represents the patrol state of the enemy.</value>
            [HideInInspector]
            public PatrolState patrolState;

            /// <value>Property <c>alertState</c> represents the alert state of the enemy.</value>
            [HideInInspector]
            public AlertState alertState;

            /// <value>Property <c>attackState</c> represents the attack state of the enemy.</value>
            [HideInInspector]
            public AttackState attackState;

            /// <value>Property <c>dyingState</c> represents the dying state of the enemy.</value>
            [HideInInspector]
            public DyingState dyingState;

            /// <value>Property <c>currentState</c> represents the current state of the enemy.</value>
            [HideInInspector]
            public IEnemyState currentState;
        
        #endregion
        
        #region Enemy Settings

            /// <value>Property <c>life</c> represents the life of the enemy.</value>
            public float life = 100f;

            /// <value>Property <c>fireRate</c> represents how fast the enemy shoots.</value>
            public float fireRate = 1.0f;

            /// <value>Property <c>damage</c> represents how much damage the enemy does.</value>
            public float damageForce = 10.0f;

            /// <value>Property <c>rotationTime</c> represents how much time the enemy takes to rotate.</value>
            public float rotationTime = 3.0f;
            
            /// <value>Property <c>viewDistance</c> represents the distance the enemy can see.</value>
            public float viewDistance = 10.0f;
            
            /// <value>Property <c>isSniper</c> represents if the enemy is a sniper.</value>
            public bool isSniper;
            
            /// <value>Property <c>laserLine</c> represents the laser line of the enemy.</value>
            public LineRenderer laserLine;

            /// <value>Property <c>shootHeight</c> represents the height of the enemy when shooting.</value>
            public float shootHeight = 1.5f;
            
        #endregion

        #region Component References

            /// <value>Property <c>navMeshAgent</c> represents the NavMeshAgent of the enemy.</value>
            [HideInInspector]
            public NavMeshAgent navMeshAgent;
            
            /// <value>Property <c>bulletPrefab</c> represents the bullet prefab of the enemy.</value>
            public GameObject bulletPrefab;
            
            /// <value>Property <c>bulletSpawnPoint</c> represents the bullet spawn point of the enemy.</value>
            public Transform bulletSpawnPoint;

            /// <value>Property <c>waypointParent</c> represents the parent of the waypoints.</value>
            public Transform waypointParent;

            /// <value>Property <c>wayPoints</c> represents the waypoints of the enemy when patrolling.</value>
            [HideInInspector]
            public Transform[] wayPoints;

            /// <value>Property <c>mandatoryDrop</c> represents the mandatory drop of the enemy.</value>
            public GameObject mandatoryDrop;
            
            /// <value>Property <c>optionalDrops</c> represents the optional drops of the enemy.</value>
            public GameObject[] optionalDrops;

            /// <value>Property <c>audioSource</c> represents the audio source of the enemy.</value>
            public AudioSource audioSource;
            
            /// <value>Property <c>materials</c> represents the materials of the enemy.</value>
            public Material[] materials;

            /// <value>Property <c>animator</c> represents the animator of the enemy.</value>
            public Animator animator;

            /// <value>Property <c>FootstepAudioClips</c> represents the footstep audio clips of the enemy.</value>
            public AudioClip[] footstepAudioClips;

            /// <value>Property <c>FootstepAudioVolume</c> represents the footstep audio volume of the enemy.</value>
            [Range(0, 1)] public float footstepAudioVolume = 0.5f;

            #endregion
        
        #region Read-Only Properties

            /// <value>Property <c>AnimatorSpeed</c> represents the speed parameter of the animator.</value>
            private static readonly int AnimatorSpeed = Animator.StringToHash("Speed");

            /// <value>Property <c>AnimatorMotionSpeed</c> represents the speed parameter of the animator.</value>
            private static readonly int AnimatorMotionSpeed = Animator.StringToHash("MotionSpeed");

            /// <value>Property <c>AnimatorDead</c> represents the dead parameter of the animator.</value>
            private static readonly int AnimatorDead = Animator.StringToHash("Dead");
            
            /// <value>Property <c>AnimatorHit</c> represents the hit parameter of the animator.</value>
            private static readonly int AnimatorHit = Animator.StringToHash("Hit");

            /// <value>Property <c>BaseColor</c> represents the base color parameter of a shader.</value>
            private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
            
        #endregion

        /// <summary>
        /// Method <c>Start</c> is called when the script is initialized.
        /// </summary>
        private void Start()
        {
            // Get the waypoints
            wayPoints = new Transform[waypointParent.childCount];
            for (var i = 0; i < wayPoints.Length; i++)
                wayPoints[i] = waypointParent.GetChild(i);

            // Initialize the states
            patrolState = new PatrolState(this);
            alertState = new AlertState(this);
            attackState = new AttackState(this);
            dyingState = new DyingState(this);

            // Set the initial state
            currentState = patrolState;

            // Get the NavMeshAgent
            navMeshAgent = GetComponent<NavMeshAgent>();
            
            // Get the materials of all children
            var renderers = GetComponentsInChildren<Renderer>();
            materials = new Material[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
                materials[i] = renderers[i].material;
            
            // If is sniper, get the line renderer
            if (isSniper)
            {
                laserLine.startWidth = 0.01f;
                laserLine.endWidth = 0.01f;
                laserLine.enabled = false;
            }

        }

        /// <summary>
        /// Method <c>Update</c> is called once per frame.
        /// </summary>
        private void Update()
        {
            // Update the current state
            currentState.UpdateState();

            // Update the animator
            animator.SetFloat(AnimatorSpeed, navMeshAgent.velocity.magnitude);
            animator.SetFloat(AnimatorMotionSpeed, navMeshAgent.velocity.magnitude > 0 ? 1 : 0);

            // Check if the enemy is dead
            if (life <= 0 && currentState != dyingState)
            {
                currentState.GoToDyingState();
                StartCoroutine(Die());
            }
        }

        /// <summary>
        /// Method <c>TakeDamage</c> is called when the enemy is hit.
        /// </summary>
        /// <param name="damage">The damage received.</param>
        public void TakeDamage(float damage)
        {
            life -= damage;
            currentState.Impact();
            animator.SetTrigger(AnimatorHit);
        }

        /// <summary>
        /// Method <c>OnTriggerEnter</c> is called when the enemy enters a trigger.
        /// </summary>
        /// /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerEnter(Collider col)
        {
            currentState.OnTriggerEnter(col);
        }

        /// <summary>
        /// Method <c>OnTriggerStay</c> is called when the enemy stays in a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerStay(Collider col)
        {
            currentState.OnTriggerStay(col);
        }

        /// <summary>
        /// Method <c>OnTriggerExit</c> is called when the enemy exits a trigger.
        /// </summary>
        /// <param name="col">The collider of the trigger.</param>
        private void OnTriggerExit(Collider col)
        {
            currentState.OnTriggerExit(col);
        }

        /// <summary>
        /// Method <c>Die</c> makes the enemy die.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Die()
        {
            // Play the dying animation
            animator.SetTrigger(AnimatorDead);

            // Wait for the animation to finish
            yield return new WaitForSeconds(2f);
            
            // Drop a random item
            DropItem();

            // Make the enemy disappear slowly, decreasing its size
            while (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
                yield return null;
            }

            // Destroy the enemy
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Method <c>DropItem</c> drops a random item.
        /// </summary>
        private void DropItem()
        {
            var position = transform.position;
            var dropPosition = new Vector3(position.x, position.y + 1.5f, position.z);
            
            // If there's a mandatory drop, drop it
            if (mandatoryDrop != null)
            {
                Instantiate(mandatoryDrop, dropPosition, Quaternion.identity);
                return;
            }
            
            // If there's optional drop, have a 90% change of dropping one of them
            if (optionalDrops.Length > 0 && UnityEngine.Random.Range(0, 4) == 0)
            {
                var drop = UnityEngine.Random.Range(0, optionalDrops.Length);
                Instantiate(optionalDrops[drop], dropPosition, Quaternion.identity);
            }
        }

        /// <summary>
        /// Method <c>Fade</c> fades the enemy.
        /// </summary>
        /// <param name="fadeTime">The fade time.</param>
        private void Fade(float fadeTime = 2f)
        {
            var coroutines = materials.Length;
            foreach (var material in materials)
            {
                StartCoroutine(FadeMaterial(material, fadeTime, () =>
                {
                    coroutines--;
                    if (coroutines <= 0)
                        Destroy(gameObject);
                }));
            }
        }
        
        
        /// <summary>
        /// Method <c>FadeMaterial</c> fades a material.
        /// </summary>
        /// <param name="material">The material to fade.</param>
        /// <param name="fadeTime">The fade time.</param>
        /// <param name="callback">The callback to call when the fade is finished.</param>
        /// <returns></returns>
        private IEnumerator FadeMaterial(Material material, float fadeTime, Action callback)
        {
            var color = material.GetColor(BaseColor);
            var startColor = color;
            var endColor = new Color(color.r, color.g, color.b, 0);
            var startTime = Time.time;
            while (Time.time < startTime + fadeTime)
            {
                color = Color.Lerp(startColor, endColor, (Time.time - startTime) / fadeTime);
                material.SetColor(BaseColor, color);
                yield return null;
            }
            callback();
        }

        /// <summary>
        /// Method <c>OnFootstep</c> is called when the enemy makes a footstep.
        /// </summary>
        /// <param name="animationEvent">The animation event.</param>
        public void OnFootstep(AnimationEvent animationEvent)
        {
            if (!(animationEvent.animatorClipInfo.weight > 0.5f) || (footstepAudioClips.Length <= 0))
                return;
            var index = UnityEngine.Random.Range(0, footstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.position, footstepAudioVolume);
        }
    }
}
