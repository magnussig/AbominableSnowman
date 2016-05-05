using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    
    public GameObject gameOverCanvas;

  /*  void Update()
    {
        if (GameCharacter.isDead)
        {
            gameOverCanvas.SetActive(true);
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }
    }*/


    public void ExitToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
