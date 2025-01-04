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
        // Calcular la posici�n deseada del fondo basada en la posici�n del personaje
        Vector3 targetPosition = target.position + offset;

        // Suavizar el movimiento hacia la posici�n deseada
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }
}