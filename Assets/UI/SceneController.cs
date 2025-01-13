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
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }
    public static void ExitGame()
    {
        Application.Quit();
    }   
}
