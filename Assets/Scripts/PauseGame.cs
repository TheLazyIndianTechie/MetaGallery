using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseGame : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {

    }

    // Update is called once per frame
    private void OnEnable()
    {

        // player.gameObject.GetComponent<StarterAssets.PlayerMovementSinglePlayer>().enabled = (false);
        Time.timeScale = 0;
    }


    public void OnDisable()
    {
        // player.gameObject.GetComponent<StarterAssets.PlayerMovementSinglePlayer>().enabled = (true);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel() //Restarts the level
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
