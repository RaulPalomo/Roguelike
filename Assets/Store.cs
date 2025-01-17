using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    GameObject player;
    public GameObject buttonStick;
    public GameObject buttonSniper;
    public GameObject buttonFlamethrower;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.GetComponent<weaponController>().isMeleeUnlocked)
        {
            buttonStick.SetActive(false);
        }
        else
        {
            buttonStick.SetActive(true);
        }
        if (player.GetComponent<weaponController>().isSniperUnlocked)
        {
            buttonSniper.SetActive(false);
        }
        else
        {
            buttonSniper.SetActive(true);
        }
        if (player.GetComponent<weaponController>().isFlamethrowerUnlocked)
        {
            buttonFlamethrower.SetActive(false);
        }
        else
        {
            buttonFlamethrower.SetActive(true);
        }
    }

    public void BuyStick()
    {
        if(player.GetComponent<PlayerMovement>().coins<10)
        {
            return;
        }
        player.GetComponent<PlayerMovement>().coins -= 10;
        player.GetComponent<weaponController>().isMeleeUnlocked = true;
    }

    public void BuySniper()
    {
        if (player.GetComponent<PlayerMovement>().coins < 20)
        {
            return;
        }
        player.GetComponent<PlayerMovement>().coins -= 20;
        player.GetComponent<weaponController>().isSniperUnlocked = true;
    }

    public void BuyFlamethrower()
    {
        if (player.GetComponent<PlayerMovement>().coins < 50)
        {
            return;
        }
        player.GetComponent<PlayerMovement>().coins -= 50;
        player.GetComponent<weaponController>().isFlamethrowerUnlocked = true;
    }
}
