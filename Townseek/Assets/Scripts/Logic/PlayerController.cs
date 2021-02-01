using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using WhalesAndGames.MapGame.Singletons;

namespace WhalesAndGames.MapGame.Logic
{
    /// <summary>
    /// Handles the controller of the player's ship on the over-world.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [BoxGroup("Movement")]
        public float movementSpeed = 5f;

        [BoxGroup("Discovery Radius")] 
        [SerializeField]
        private float discoveryRadius = 6f;
        
        [BoxGroup("Interact Radius")]
        [SerializeField]
        private float interactRadius = 2.5f;
        [BoxGroup("Interact Radius")]
        [SerializeField]
        private LayerMask discoveryMask;

        [BoxGroup("Components")]
        [SerializeField]
        private GameObject shipBody;
        [BoxGroup("Components")]
        [SerializeField]
        private GameObject shipBottom;
        [BoxGroup("Components")]
        [SerializeField]
        private GameObject shipTop;
        
        [BoxGroup("Audio")]
        [SerializeField]
        private LayerMask audioMask;
        [BoxGroup("Audio")] 
        [SerializeField]
        private StudioEventEmitter movementEmitter;

        private Animator shipAnimator;
        private Animator shipBodyAnimator;
        private Animator shipBottomAnimator;
            
        private new Rigidbody2D rigidbody;
        private InputAction moveAction;
        
        private List<IAudioEmitter> emittersInRange = new List<IAudioEmitter>();
        private List<IDiscoverable> discoveriesInRange = new List<IDiscoverable>();
        private IInteractable interactableInRange;

        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();

            shipAnimator = GetComponent<Animator>();
            shipBodyAnimator = shipBody.GetComponent<Animator>();
            shipBottomAnimator = shipBottom.GetComponent<Animator>();
            
            moveAction = GlobalManager.Instance.playerInput.actions["Move"];
            GlobalManager.Instance.playerInput.actions["Interact"].performed += OnInteract;
        }

        /// <summary>
        /// Allows the player to move the ship in the world through an animation event.
        /// </summary>
        public void AllowMovement()
        {
            GameManager.Instance.playerCanMove = true;
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        private void Update()
        {
            if (GameManager.Instance.gameState != GameState.Playing || !GameManager.Instance.playerCanMove)
            {
                rigidbody.velocity = Vector2.zero;
                return;
            }
            
            OnMove();
            
            CheckAudioEmitters();
            CheckDiscoveries();
            CheckInteractables();
        }

        /// <summary>
        /// Processes the input of the player for movement.
        /// </summary>
        private void OnMove()
        {
            var velocity = moveAction.ReadValue<Vector2>();
            rigidbody.velocity = velocity * movementSpeed;

            float isMoving = velocity != Vector2.zero ? 1 : 0;
            shipBottomAnimator.SetFloat("Velocity", isMoving);
            movementEmitter.SetParameter("shipMoving", isMoving);

            if (velocity.x > 0)
            {
                shipBodyAnimator.SetBool("Flip", true);
            }
            else if(velocity.x < 0)
            {
                shipBodyAnimator.SetBool("Flip", false);
            }
        }
        
        /// <summary>
        /// Checks if there are any audio emitters around the player.
        /// </summary>
        public void CheckAudioEmitters()
        {
            var emitterChecks = Physics2D.OverlapCircleAll(transform.position, discoveryRadius, audioMask)
                .Select(x => x.GetComponent<IAudioEmitter>()).ToList();

            var emittersNowOutOfRange = new List<IAudioEmitter>();
            foreach (var emit in emittersInRange)
            {
                if (!emitterChecks.Contains(emit))
                {
                    emittersNowOutOfRange.Add(emit);
                    emit.EmitAudio(100);
                }
            }

            foreach (var emit in emittersNowOutOfRange)
            {
                emittersInRange.Remove(emit);
            }

            foreach (var emit in emitterChecks)
            {
                if (emit == null)
                {
                    continue;
                }
                
                if (!emittersInRange.Contains(emit))
                {
                    emittersInRange.Add(emit);
                }

                var distanceCalculation = (Vector2.Distance(emit.GetPosition(), transform.position) / discoveryRadius) * 100 - 20;
                distanceCalculation = Mathf.Clamp(distanceCalculation, 0f, 100f);
                emit.EmitAudio(distanceCalculation);
            }
        }

        /// <summary>
        /// Checks if there are any discoveries around the player.
        /// </summary>
        public void CheckDiscoveries()
        {
            var discoveryChecks = Physics2D.OverlapCircleAll(transform.position, discoveryRadius, discoveryMask)
                .Select(x => x.GetComponent<IDiscoverable>()).ToList();

            var discoverablesNowOutOfRange = new List<IDiscoverable>();
            foreach (var disc in discoveriesInRange)
            {
                if (!discoveryChecks.Contains(disc))
                {
                    discoverablesNowOutOfRange.Add(disc);
                    disc.OnDiscoveryExit();
                }
            }

            foreach (var disc in discoverablesNowOutOfRange)
            {
                discoveriesInRange.Remove(disc);
            }

            foreach (var disc in discoveryChecks)
            {
                if (!discoveriesInRange.Contains(disc))
                {
                    discoveriesInRange.Add(disc);
                    disc.OnDiscoveryEnter();
                }

                var distanceCalculation = (Vector2.Distance(disc.GetPosition(), transform.position) / discoveryRadius) * 100 - 20;
                distanceCalculation = Mathf.Clamp(distanceCalculation, 0f, 100f);
            }
        }

        /// <summary>
        /// Check if there's any intractables around the player.
        /// </summary>
        public void CheckInteractables()
        {
            var interactableCheck = Physics2D.OverlapCircleAll(transform.position, interactRadius, discoveryMask)
                .Select(x => x.GetComponent<IInteractable>()).ToList().OrderBy(x => Vector2.Distance(transform.position, x.GetPosition())).
                FirstOrDefault(x => x != null);

            if (interactableCheck == null)
            {
                if (interactableInRange != null)
                {
                    interactableInRange.OnInteractableExit();
                    interactableInRange = null;
                }

                return;
            }
            
            if (interactableInRange != null && interactableInRange != interactableCheck)
            {
                interactableInRange.OnInteractableExit();
            }

            interactableInRange = interactableCheck;
            interactableInRange.OnInteractableEnter();
        }

        /// <summary>
        /// Checks if the player has pressed to interact with any buildings.
        /// </summary>
        private void OnInteract(InputAction.CallbackContext obj)
        {
            if (GameManager.Instance.gameState != GameState.Playing || !GameManager.Instance.playerCanMove)
            {
                return;
            }
            
            if (interactableInRange != null)
            {
                interactableInRange.InteractWith();
            }
        }

        /// <summary>
        /// OnDrawGizmos draws gizmos that are also pickable and always drawn.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, interactRadius);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, discoveryRadius);
        }
    }
}