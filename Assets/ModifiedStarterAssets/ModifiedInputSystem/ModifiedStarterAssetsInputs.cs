using StarterAssets;
using UnityEngine;
#if(ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED)
using UnityEngine.InputSystem;
#endif

public class ModifiedStarterAssetsInputs : StarterAssetsInputs
{
    [Header("Character Rotation Keyboard Input Values")]
    public bool rotateLeft;
    public bool rotateRight;

    [Header("Modified Input Control Settings")]
    public bool useModifiedInput = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    public void OnRotateLeft(InputValue value)
    {
        if (useModifiedInput)
        {
            RotateLeftInput(value.isPressed);
        }
    }

    public void OnRotateRight(InputValue value)
    {
        if (useModifiedInput)
        {
            RotateRightInput(value.isPressed);
        }
    }
#endif

    public void RotateLeftInput(bool newRotateLeftState)
    {
        rotateLeft = newRotateLeftState;
    }

    public void RotateRightInput(bool newRotateRightState)
    {
        rotateRight = newRotateRightState;
    }
}
