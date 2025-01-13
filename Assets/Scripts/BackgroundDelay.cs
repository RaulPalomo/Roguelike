using UnityEngine;

public class BackgroundDelay : MonoBehaviour
{
    public Transform target;  
    public float followSpeed = 2f; 

    private Vector3 offset; 

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        // Calcular la posición deseada del fondo basada en la posición del personaje
        Vector3 targetPosition = target.position + offset;

        // Suavizar el movimiento hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}