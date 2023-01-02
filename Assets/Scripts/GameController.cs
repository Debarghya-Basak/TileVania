using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLife = 3;
    private void Awake() {
        if(FindObjectsOfType<GameController>().Length > 1)
            Destroy(gameObject);
        else    
            DontDestroyOnLoad(gameObject);
    }

    public void controlPlayerLife(){
        if(playerLife > 1)
            StartCoroutine(reduceLife());
        else    
            StartCoroutine(resetGame());
    }

    public void nextLevel(){
        StartCoroutine(changeToNextLevel());
    }

    IEnumerator changeToNextLevel(){
        yield return new WaitForSecondsRealtime(2);
        int len = SceneManager.GetAllScenes().Length;
        if(SceneManager.GetActiveScene().buildIndex < len-1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
    }

    IEnumerator reduceLife(){
        playerLife--;
        Debug.Log("Player Life : " + playerLife);
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator resetGame(){
        playerLife = 3;
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(0);
    }
}
