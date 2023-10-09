using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickDetector : MonoBehaviour
{
    public delegate void PreviewArt(Art art);
    public static event PreviewArt OnTap;
    private bool isGamePaused = false;

    private void OnEnable()
    {
        GalleryUIController.OnStateChange += SetGamePausedState;
    }

    private void OnDisable()
    {
        GalleryUIController.OnStateChange -= SetGamePausedState;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit objectHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out objectHit, 100.0f, LayerMask.GetMask("Digital Content")))
            {
                if (objectHit.transform != null && !isGamePaused)
                {
                    print(objectHit.transform.gameObject.ToString());
                    ArtDisplay currentScript = objectHit.transform.gameObject.GetComponent<ArtDisplay>();
                    if(currentScript!=null)
                    {
                        var selectedArt = currentScript.GetArt();
                        OnTap(selectedArt);
                    }
                }
            }
        }
    }

    private void SetGamePausedState(bool newState)
    {
        isGamePaused = newState;
    }
}