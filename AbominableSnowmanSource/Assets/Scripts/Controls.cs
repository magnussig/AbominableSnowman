using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Controls : MonoBehaviour {

    public void BackToMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
