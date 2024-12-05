using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>();
    private Grid grid;

    
    private void Start()
    {
        grid=FindObjectOfType<Grid>();
        GameObject firstRoom=Instantiate(rooms[Rand(rooms.Count)]);
        /*firstRoom.transform.SetParent(grid.transform);
        Door[] doors = firstRoom.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            Debug.Log(door.direction);
            //GenerateRoom(door);
        }*/
        AssignDoor(firstRoom);

    }
    public void AssignDoor(GameObject room)
    {
        room.transform.SetParent(grid.transform);
        Door[] doors = room.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            if(!door.used)
            {
                Debug.Log(door.direction);
                GenerateRoom(door);
            }
            
        }
    }
    public void GenerateRoom(Door door)
    {
        
        GameObject room = Instantiate(rooms[Rand(rooms.Count)]);

        Door[] doors = room.GetComponentsInChildren<Door>();
        SetRoom(door,doors[Rand(doors.Length)],room);

    }
    /*public void SetRoom(Door old, Door newD)
    {
        old.used = true;
        newD.used = true;
        //hacer que se conecten mediante las direcciones 0=derecha, izquierda=2, 1=arriba, 3=abajo cogiendo sus transform position i moviendo todo el prefab
    }*/
    /*public void SetRoom(Door old, Door newD, GameObject room)
    {
        old.used = true;
        newD.used = true;

        // Obtener las posiciones de las puertas actuales en 2D
        Vector2 oldDoorPosition = old.transform.position;
        Vector2 newDoorPosition = newD.transform.position;

        // Calcular la diferencia de posición necesaria para alinear las puertas
        Vector2 offset = Vector2.zero;
        switch (old.direction)
        {
            case 0: // Derecha
                offset = oldDoorPosition - newDoorPosition + new Vector2(1, 0);
                break;
            case 1: // Arriba
                offset = oldDoorPosition - newDoorPosition + new Vector2(0, 1);
                break;
            case 2: // Izquierda
                offset = oldDoorPosition - newDoorPosition + new Vector2(-1, 0);
                break;
            case 3: // Abajo
                offset = oldDoorPosition - newDoorPosition + new Vector2(0, -1);
                break;
        }

        // Mover toda la nueva sala para conectar las puertas
        Transform roomTransform = newD.transform.parent;
        roomTransform.position += (Vector3)offset; // Convertimos Vector2 a Vector3
        AssignDoor(room);
    }*/
    public void SetRoom(Door old, Door newD, GameObject room)
    {
        old.used = true; // Marcar la puerta actual como usada
        newD.used = true; // Marcar la nueva puerta como usada

        // Calcular la rotación adecuada
        int targetDirection = (old.direction + 2) % 4; // Dirección opuesta
        int rotationSteps = (targetDirection - newD.direction + 4) % 4; // Diferencia de direcciones
        float rotationAngle = rotationSteps * 90f; // Rotar en pasos de 90 grados

        // Aplicar la rotación a la nueva sala
        room.transform.Rotate(0, 0, rotationAngle);
        if (Mathf.Abs(rotationAngle) > 180f)
        {
            Vector3 currentScale = room.transform.localScale;
            currentScale.y *= -1; // Invertir el eje X para reflejar el sprite
            room.transform.localScale = currentScale;
        }

        // Obtener las posiciones de las puertas actuales en 2D
        Vector2 oldDoorPosition = old.transform.position;
        Vector2 newDoorPosition = newD.transform.position;

        // Calcular la diferencia de posición necesaria para alinear las puertas
        Vector2 offset = oldDoorPosition - newDoorPosition;

        // Mover la nueva sala para alinear las puertas
        Transform roomTransform = newD.transform.parent;
        roomTransform.position += (Vector3)offset;

        // Asignar puertas de la nueva sala para continuar el mapa
        AssignDoor(room);
    }
    public int Rand(int num)
    {
        return Random.Range(0, num);

    }
    
}
