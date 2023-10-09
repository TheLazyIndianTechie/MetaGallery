using UnityEngine;

public class MenuPlayerCameraManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPlayerCamera;

    private void LateUpdate()
    {
        menuPlayerCamera?.SetActive(Cursor.lockState == CursorLockMode.None);
    }
}
