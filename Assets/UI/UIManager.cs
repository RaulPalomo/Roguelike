using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public List<GameObject> hearts = new List<GameObject>();
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearts(player.GetComponent<PlayerMovement>().lives);
        UpdateCoins();
    }
    public void UpdateCoins()
    {
        coins.text = player.GetComponent<PlayerMovement>().coins.ToString();
    }
    public void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < health)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }
}
