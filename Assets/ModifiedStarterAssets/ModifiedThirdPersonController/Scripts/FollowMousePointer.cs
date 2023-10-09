using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePointer : MonoBehaviour
{
    private void LateUpdate()
    {
        Vector3 worldPosition = GetWorldPosition();
        this.transform.position = new Vector3(worldPosition.x, worldPosition.y, worldPosition.z);
    }

    private Vector3 GetWorldPosition()
    {
        float distanceFromCamera = 10f;
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = distanceFromCamera;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}