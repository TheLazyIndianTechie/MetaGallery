using Cinemachine;
using UnityEditor;
using UnityEngine;
using System.Collections;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class PlayerMovementSinglePlayer : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        //***************************************** CUSTOM CODE ***************************************************
        //Custom code by Raghav
        [HideInInspector] public StaminaCtrl staminactrl;
        [SerializeField] public GameObject health;
        [SerializeField] public GameObject playerDied;
        
        [SerializeField] public GameObject SpawnEffect;
        [HideInInspector] public Vector3 SpawnPoint;
        [HideInInspector] private Health healthctrl;

        //************************************** END CUSTOM CODE ******************************************************
        //End of Custom Code



        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float targetSpeed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        //**************************************** CUSTOM CODE ****************************************************
        //Custom code by Vinay

        public string[] CustomEmote;
        //End custom code
        //**************************************** END CUSTOM CODE ****************************************************



        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;



#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool groundCheck;

        
        private float speed;

        
        private float motionSpeed;

        
        private bool jump;

        
        private bool freeFall;


        //****************************** CUSTOM CODE **************************************************************
        private bool emote1, emote2, emote3;
        //******************************** END CUSTOM CODE ************************************************************

        

        private void cmdGroundChanged(bool newVal) { groundCheck = newVal; }

       
        private void cmdSpeedChanged(float newVal) { speed = newVal; }

        
        private void cmdMotionSpeedChanged(float newVal) { motionSpeed = newVal; }

      
        private void cmdJumpChanged(bool newVal) { jump = newVal; }

       
        private void cmdFreeFallChanged(bool newVal) { freeFall = newVal; }


        //********************************************************************************************
        //private void cmdEmote1(bool newVal) { emote1 = newVal; }

        
        //private void cmdEmote2(bool newVal) { emote2 = newVal; }

        
        //private void cmdEmote3(bool newVal) { emote3 = newVal; }
        //
        //custom animation emote functions (valimkhan)
        //public void OnEmote01()
        //{
        //    Debug.Log("Emote 01 was pressed in Input section");
        //    _animator.SetBool("Emote01", true);
        //    cmdEmote1(true);
        //    //call command Function..
        //}

        //public void OnEmote02()
        //{
        //    Debug.Log("Emote 02 was pressed in Input section");
        //    _animator.SetBool("Emote02", true);
        //    cmdEmote2(true);
        //}

        //public void OnEmote03()
        //{
        //    Debug.Log("Emote 03 was pressed in Input section");
        //    _animator.SetBool("Emote03", true);
        //    cmdEmote3(true);

        //}

        private void UpdateAnimatorParams()
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, groundCheck);
                _animator.SetFloat(_animIDSpeed, speed);
                _animator.SetFloat(_animIDMotionSpeed, motionSpeed);
                _animator.SetBool(_animIDJump, jump);
                _animator.SetBool(_animIDFreeFall, freeFall);

                //custom assignments valimkhan
                _animator.SetBool("Emote01", emote1);
                _animator.SetBool("Emote02", emote2);
                _animator.SetBool("Emote03", emote3);
            }
        }

        //
        //Networked Animation Code Finished....

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }


        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;


            //Custom code

            // spawnpoint after death set in script instead of the inspector 
            if (SpawnPoint != null)
            {
                SpawnPoint = gameObject.transform.position;
            }

        }

      

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _animator = GetComponentInChildren<Animator>();
            if (_animator != null)
            {
                _hasAnimator = true;
            }
            else
            {
                _hasAnimator = false;
            }

            //_hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            //Debug.Log(Application.dataPath);


            
            if (staminactrl == null)
            {
                staminactrl = GetComponent<StaminaCtrl>();
            }

            if (healthctrl == null)
            {
                healthctrl = health.GetComponent<Health>();
            }

           

        }
        //

        //set all emote bools to false...

        //private void SetAllEmotesToFalse()
        //{
        //    cmdEmote1(false);
        //    cmdEmote2(false);
        //    cmdEmote3(false);
        //}

        private bool lockmode = false;

        //
        private void Update()
        {
            _animator = GetComponentInChildren<Animator>();
            if (_animator != null)
            {
                _hasAnimator = true;
            }
            else
            {
                _hasAnimator = false;
            }
            //_hasAnimator = TryGetComponent(out _animator);
            UpdateAnimatorParams();





          
            //if (_speed > 0)
            //{
            //    SetAllEmotesToFalse();
            //}
            //if (Input.GetKeyDown(KeyCode.X))
            //{
            //    SetAllEmotesToFalse();
            //    cmdEmote1(true);
            //}
            //if (Input.GetKeyDown(KeyCode.C))
            //{
            //    SetAllEmotesToFalse();
            //    cmdEmote2(true);
            //}
            //if (Input.GetKeyDown(KeyCode.V))
            //{
            //    SetAllEmotesToFalse();
            //    cmdEmote3(true);
            //}
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{

            //    SetAllEmotesToFalse();
            //}
            if (Input.GetKeyDown(KeyCode.H))
            {

                if (lockmode)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }

                lockmode = !lockmode;
              
            }
            JumpAndGravity();
            GroundedCheck();
            Move();

            if(healthctrl != null && healthctrl.health <= 0)
            {
                if(_animator != null)
                {
                    _animator.SetBool("isDead", true);
                }
                playerDied.SetActive(true);
                StartCoroutine(Spawn());
            }
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        IEnumerator Spawn()
        {

            healthctrl.health = 100f;
            yield return new WaitForSeconds(3.3f);
            staminactrl.playerStamina = staminactrl.maxStamina;
            playerDied.SetActive(false);
            _animator.SetBool("isDead", false);
            Debug.Log(SpawnPoint);  
            gameObject.transform.position = SpawnPoint;
            Instantiate(SpawnEffect,SpawnPoint,gameObject.transform.rotation);
            
            

        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
                cmdGroundChanged(Grounded);


            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
           // float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
           
            if (_input.sprint && _input.move !=Vector2.zero)
            {
                if (staminactrl != null && staminactrl.playerStamina > 0 && staminactrl.playerStamina <= 20)
                {
                    targetSpeed = staminactrl.slowedRunSpeed;
                    MoveSpeed = staminactrl.slowedRunSpeed;
                    if(_animator != null)
                    {
                        _animator.SetBool("isTired", true);
                    }
                    staminactrl.Sprinting();
                }
                else
                {
                    targetSpeed = SprintSpeed;
                    if (staminactrl != null)
                    {
                        MoveSpeed = staminactrl.normalRunSpeed;
                    }

                    if(_animator != null)
                    {
                        _animator.SetBool("isTired", false);
                    }

                    if (staminactrl != null)
                    {
                        staminactrl.Sprinting();
                    }
                }
            }
            else
            {

                if(staminactrl !=null && staminactrl.playerStamina > 20)
                {
                    MoveSpeed = staminactrl.normalRunSpeed;
                    if(_animator != null)
                    {
                        _animator.SetBool("isTired", false);
                    }
                }

                targetSpeed = MoveSpeed;

                if (staminactrl != null)
                {
                    staminactrl.isSprinting = false;
                }
            }

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                cmdSpeedChanged(_animationBlend);
                cmdMotionSpeedChanged(inputMagnitude);

            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    cmdJumpChanged(false);
                    cmdFreeFallChanged(false);


                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                        cmdJumpChanged(true);

                        // deducting stamina 
                        if (staminactrl != null)
                        {
                            staminactrl.StaminaJump();
                        }
                        //setting the emotes off on the jump also..
                        //SetAllEmotesToFalse();

                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                        cmdFreeFallChanged(true);

                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void PlayFootStep()
        {
            var index = Random.Range(0, FootstepAudioClips.Length);
            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
        public void PlayLandSound()
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);

        }
    }
}