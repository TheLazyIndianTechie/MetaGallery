using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayer : MonoBehaviour
{
    // public GameObject player;

    void OnEnable()
    {
        Time.timeScale = 0;
        /*if (player!=null)
	     {
            // player.gameObject.GetComponent<StarterAssets.PlayerMovementSinglePlayer>().enabled = (false);
         }*/

        Cursor.lockState = CursorLockMode.None;
    }

    void OnDisable()
    {
        Time.timeScale = 1;
        /*if(player!=null)
        {
           // player.gameObject.GetComponent<StarterAssets.PlayerMovementSinglePlayer>().enabled = (true);
        }*/

        //Cursor.lockState = CursorLockMode.Locked;
    }
}
