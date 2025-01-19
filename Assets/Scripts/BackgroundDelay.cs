using UnityEngine;

public class BackgroundDelay : MonoBehaviour
{
    public Transform target;  
    public float followSpeed = 2f; 

    private Vector3 offset; 

    void Start()
    {
        
        target = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - target.position;
    }

    void Update()
    {
        
        Vector3 targetPosition = target.position + offset;

        
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}