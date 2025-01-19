using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyBehavior;

public class TurretBehaviour : MonoBehaviour
{
    public enum TurretState { Idle, Attack, Dead }
    public TurretState currentState = TurretState.Idle;
    public float health = 200f;
    public float speed = 3f;
    public float cooldown = 1f;
    public float fadeDuration = 2f;
    private bool canShoot = true;
    private Transform player;
    public GameObject bullet;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (currentState != TurretState.Dead)
        {
            switch (currentState)
            {
                case TurretState.Idle:
                    IdleState();
                    break;
                case TurretState.Attack:
                    AttackState();
                    break;
                case TurretState.Dead:
                    DeadState();
                    break;
            }
        }
        else
        {
            DeadState();
        }
    }
    public void IdleState()
    {
        
    }

    public void AttackState()
    {
        if (canShoot)  // Si no puede disparar, no hace nada
        {
            StartCoroutine(Shoot());
        }
        
    }

    public void DeadState()
    {
        
        StartCoroutine(FadeOutAndDestroy());
    }

    IEnumerator Shoot()
    {
        canShoot = false;  // Activa el cooldown
        Vector3 direction = player.position - transform.position;
        Vector3 spawn= transform.position;
        spawn.z=-2f;
        GameObject newBullet = Instantiate(bullet, spawn, Quaternion.identity);
        newBullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;

        yield return new WaitForSeconds(cooldown);  // Espera el tiempo de cooldown
        canShoot = true;  // Desactiva el cooldown

    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {

            currentState = TurretState.Dead;
        }
        Debug.Log("Enemy health: " + health);
    }
    IEnumerator FadeOutAndDestroy()
    {
        float elapsedTime = 0f;
        Color originalColor = spriteRenderer.color;
        Color targetColor = originalColor;
        targetColor.a = 0f; 

        while (elapsedTime < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(originalColor, targetColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;  
        }

        spriteRenderer.color = targetColor; 
        
        Destroy(gameObject);  
    }

    public void OnDestroy()
    {
        EventController.TriggerEnemyDefeat();
    }
}
