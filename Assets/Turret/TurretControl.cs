using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretControl : MonoBehaviour
{
    private TurretBehaviour turretBehaviour;
    void Awake()
    {
        turretBehaviour = GetComponent<TurretBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            
            if (turretBehaviour.health > 0)
            {
                turretBehaviour.currentState = TurretBehaviour.TurretState.Attack;
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            
            if (turretBehaviour.health > 0)
            {
                turretBehaviour.currentState = TurretBehaviour.TurretState.Idle;
            }
        }
        
    }

     
}
