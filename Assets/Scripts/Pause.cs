using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private Key key = Key.M;
    public GameObject PauseMenu;
    public PauseGame game;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current[key].isPressed)
        {
            PauseMenu.SetActive(true);
            game.enabled = true;

            this.gameObject.SetActive(false);
        }

    }
}
