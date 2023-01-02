using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int playerLife = 3;
    int tempCount = 0;
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
        if(tempCount == 0){
            StartCoroutine(changeToNextLevel());
            tempCount++;
        }
            
    }

    IEnumerator changeToNextLevel(){
        yield return new WaitForSecondsRealtime(2);
        int len = SceneManager.sceneCount;
        if(SceneManager.GetActiveScene().buildIndex < len)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(0);
        tempCount = 0;
        Debug.Log(tempCount);
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
