using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    GameObject player;
    
    void Awake()
    {
        
        
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        player.transform.position = new Vector3(0, 0, 0);
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
    
    public void ReloadScene()
    {
        player.GetComponent<PlayerMovement>().loop += 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
