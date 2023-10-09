using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using System;

public class FirstPersonCameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera firstPersonFollowCamera;
    [SerializeField] private float firstPersonCameraSensitivity;
    [SerializeField] private float thirdPersonCameraSensitivity;
    [SerializeField] private LayerMask colliderLayerMask = new LayerMask();

    [Header("Avatar")]
    [SerializeField] private GameObject avatar;
    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private bool isGamePaused = false;

    private void OnEnable()
    {
        GalleryUIController.OnStateChange += SetGamePausedState;
    }

    private void OnDisable()
    {
        GalleryUIController.OnStateChange -= SetGamePausedState;
    }

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            HandleCameraView();
        }
    }

    private void HandleCameraView()
    {
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, colliderLayerMask))
        {
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.switchCamera)
        {
            firstPersonFollowCamera.gameObject.SetActive(true);
            avatar.SetActive(false);
            thirdPersonController.SetSensitivity(firstPersonCameraSensitivity);

            Vector3 zoomDirection = mouseWorldPosition;
            zoomDirection.y = transform.position.y;
            Vector3 avatarPosition = (zoomDirection - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, avatarPosition, Time.deltaTime * 20f);
        }
        else
        {
            firstPersonFollowCamera.gameObject.SetActive(false);
            avatar.SetActive(true);
            thirdPersonController.SetSensitivity(thirdPersonCameraSensitivity);
        }
    }

    private void SetGamePausedState(bool newState)
    {
        isGamePaused = newState;
    }
}
