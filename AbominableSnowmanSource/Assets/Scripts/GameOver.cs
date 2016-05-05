using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {
    
   [SerializeField] private GameObject gameOverCanvas;
   private CharacterController player;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }

   void Update()
    {
        if (player.IsDead)
        {
            gameOverCanvas.SetActive(true);
        }
        else
        {
            gameOverCanvas.SetActive(false);
        }
    }


    public void ExitToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
