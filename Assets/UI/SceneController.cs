using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{

    
    void Awake()
    {
        
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if(sceneName=="MainMenu") { Destroy(GameObject.FindGameObjectWithTag("Player")); }
        Time.timeScale = 1f;
    }
    public static void ExitGame()
    {
        Application.Quit();
    }   
}
