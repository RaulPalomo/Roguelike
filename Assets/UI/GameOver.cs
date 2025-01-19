using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    GameObject player;
    public GameObject GameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
    }
    private void FixedUpdate()
    {
        if (player.GetComponent<PlayerMovement>().lives <= 0 && GameOverPanel != null)
        {
            GameOverPanel.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
