using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventController : MonoBehaviour
{
    public static event Action OnEnemiesDefeat;
    public static event Action OnEnemyDefeat;
    public GameObject storePanel;
    public int enemiesToDefeat = 0;
    public int enemiesDefeated = 0;
    void Start()
    {
        storePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnEnable()
    {
        EventController.OnEnemiesDefeat += EnemiesDefeat;
        EventController.OnEnemyDefeat += EnemyDefeat;

    }

    public void OnDisable()
    {
        EventController.OnEnemiesDefeat -= EnemiesDefeat;
        EventController.OnEnemyDefeat -= EnemyDefeat;
    }

    public void EnemiesDefeat()
    {
        
        try
        {
            storePanel.SetActive(true);
        }
        catch (Exception e)
        {
            
        }
    }

    public void EnemyDefeat()
    {
        Debug.Log("An enemy was defeated");
        GameObject player=GameObject.FindGameObjectWithTag("Player");
        if (player != null) { player.GetComponent<PlayerMovement>().coins += UnityEngine.Random.Range(0, 6); }
        
        enemiesDefeated++;
        Debug.Log("Enemies defeated: " + enemiesDefeated);
        if (enemiesDefeated == enemiesToDefeat)
        {
            OnEnemiesDefeat?.Invoke();
        }
        
    }

    public static void TriggerEnemyDefeat()
    {
        OnEnemyDefeat?.Invoke();
    }
}