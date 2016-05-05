 using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    //public string mainMenu;

    public bool isPaused;

    public GameObject pauseMenuCanvas;
	
	// Update is called once per frame
	void Update () {
        if (isPaused)
        {
            pauseMenuCanvas.SetActive(true);
        }
        else
        {
            pauseMenuCanvas.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.P)){
            isPaused = !isPaused;
        }
	}
    public void Resume()
    {
        isPaused = false;  

    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
