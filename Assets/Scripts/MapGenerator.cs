using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    
    public void SetRoom(Door old, Door newD, GameObject room)
    {
        
        old.used = true; // Marcar la puerta actual como usada
        newD.used = true; // Marcar la nueva puerta como usada

        // **1. Calcular la rotación adecuada para alinear las puertas**
        int targetDirection = (old.direction + 2) % 4; // Dirección opuesta de la puerta actual
        int rotationSteps = (targetDirection - newD.direction + 4) % 4; // Diferencia de direcciones (0, 1, 2, 3)
        float rotationAngle = rotationSteps * 90f; // Convertir a grados (90° por paso)

        // Rotar la nueva sala
        room.transform.Rotate(0, 0, rotationAngle);


        // **2. Calcular la diferencia de posición entre las dos puertas**
        Vector2 oldDoorPosition = old.transform.position; // Posición de la puerta existente
        Vector2 newDoorPosition = newD.transform.position; // Posición de la nueva puerta

        // Obtener el desplazamiento necesario para alinear las puertas
        Vector2 offset = oldDoorPosition - newDoorPosition;

        // **3. Aplicar el desplazamiento a toda la sala (prefab)**
        Transform roomTransform = newD.transform.parent; // Asegurarse de mover todo el prefab
        roomTransform.position += (Vector3)offset; // Convertir el offset de Vector2 a Vector3
        

        // **4. Llamar a AssignDoor para seguir generando**
        AssignDoor(room);
    }
    public int Rand(int num)
    {
        return Random.Range(0, num);

    }
    
}
