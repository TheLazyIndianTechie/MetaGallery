using StarterAssets;
using UnityEngine;
using System;

public class ModifiedThirdPersonController : ThirdPersonController
{
    [Space(10)]
    [Header("Player Rotation Using Keyboard")]
    [Tooltip("How fast the character rotates when controlled using only keyboard")]
    [Range(0.1f, 1f)]
    [SerializeField] private float keyPressSensitivity = 0.25f;

    private ModifiedStarterAssetsInputs modifiedInputs;

    private bool isInPlayMode = false;

    protected override void Start()
    {
        base.Start();

        AttachAndCheckAnimator();

        modifiedInputs = GetComponent<ModifiedStarterAssetsInputs>();
    }

    protected override void Update()
    {
        AttachAndCheckAnimator();

        if(isInPlayMode)
        {
            base.Update();
        }
    }

    protected override void LateUpdate()
    {
        if(isInPlayMode)
        {
            base.LateUpdate();
        }
     
        if (modifiedInputs.useModifiedInput)
        {
            KeyboardControlledCameraRotation();
        }
    }

    private void AttachAndCheckAnimator()
    {
        _animator = GetComponentInChildren<Animator>();

        _hasAnimator = _animator != null;
    }

    private void OnEnable()
    {
        WowEventManager.StartListening(nameof(WowEvents.OnRPMAvatarLoaded), ResumeGame);
        WowEventManager.StartListening(nameof(WowEvents.OnPlayerResumeGame), ResumeGame);
        WowEventManager.StartListening(nameof(WowEvents.OnPlayerRequestMenu), ToogleCursorLocked);
    }

    private void OnDisable()
    {
        WowEventManager.StopListening(nameof(WowEvents.OnRPMAvatarLoaded), ResumeGame);
        WowEventManager.StopListening(nameof(WowEvents.OnPlayerResumeGame), ResumeGame);
        WowEventManager.StopListening(nameof(WowEvents.OnPlayerRequestMenu), ToogleCursorLocked);
    }

    private void KeyboardControlledCameraRotation()
    {
        if (modifiedInputs.rotateLeft || modifiedInputs.rotateRight)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += (modifiedInputs.rotateLeft ? -keyPressSensitivity : keyPressSensitivity) * deltaTimeMultiplier;
        }

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);

        // Finally rotate the player to the new camera direction
        PlayerRotation();
    }

    private void PlayerRotation()
    {
        // Modify player direction when not in motion
        if (modifiedInputs.move == Vector2.zero)
        {
            // Rotate player to face the direction relative to camera position
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _cinemachineTargetYaw, ref _rotationVelocity,
                    RotationSmoothTime * 3);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    private void ToogleCursorLocked()
    {
        UpdateGameState(false);
    }

    private void ResumeGame()
    {
        UpdateGameState(true);
    }

    private void UpdateGameState(bool isInPlayMode)
    {
        this.isInPlayMode = isInPlayMode;
        modifiedInputs.cursorLocked = isInPlayMode;

        SetCursorState(modifiedInputs.cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }
}