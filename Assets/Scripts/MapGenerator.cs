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
        firstRoom.transform.SetParent(grid.transform);
        Door[] doors = firstRoom.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            GenerateRoom(door);
        }

    }
    public void GenerateRoom(Door door)
    {
        Vector2 doorPosition = door.transform.position;
        GameObject room = Instantiate(rooms[Rand(rooms.Count)]);
        Door[] doors = room.GetComponentsInChildren<Door>();

    }
    public int Rand(int num)
    {
        return Random.Range(0, num);

    }
    /*
      public List<GameObject> rooms = new List<GameObject>(); // Lista de prefabs de las habitaciones
    private Grid grid; // Referencia al sistema de Grid (opcional)

    private void Start()
    {
        // Encuentra el objeto Grid en la escena
        grid = FindObjectOfType<Grid>();

        // Genera la primera habitaci�n
        GameObject firstRoom = Instantiate(rooms[Rand()]);
        firstRoom.transform.SetParent(grid.transform);

        // Obt�n todas las puertas de la primera habitaci�n
        Door[] doors = firstRoom.GetComponentsInChildren<Door>();

        // Para cada puerta en la primera habitaci�n, genera una nueva habitaci�n
        foreach (Door door in doors)
        {
            GenerateRoom(door);
        }
    }

    // Genera una nueva habitaci�n conectada a la puerta dada
    public void GenerateRoom(Door door)
    {
        // Obt�n la posici�n de la puerta
        Vector2 doorPosition = door.transform.position;

        // Elige una habitaci�n aleatoria de la lista
        GameObject newRoomPrefab = rooms[Rand()];

        // Instancia una nueva habitaci�n
        GameObject newRoom = Instantiate(newRoomPrefab);

        // Ajusta la posici�n de la nueva habitaci�n para conectarla a la puerta
        Vector2 newRoomPosition = GetRoomPositionFromDoor(door);

        // Establece la posici�n de la nueva habitaci�n
        newRoom.transform.position = newRoomPosition;

        // Configura las puertas de la nueva habitaci�n
        Door[] newRoomDoors = newRoom.GetComponentsInChildren<Door>();
        foreach (Door newDoor in newRoomDoors)
        {
            // Aseg�rate de que las nuevas puertas no se conecten a s� mismas
            if (Vector2.Distance(newDoor.transform.position, doorPosition) > 0.1f)
            {
                GenerateRoom(newDoor); // Genera nuevas habitaciones desde las puertas restantes
            }
        }
    }

    // Calcula la posici�n de la nueva habitaci�n a partir de una puerta existente
    private Vector2 GetRoomPositionFromDoor(Door door)
    {
        // Calcula el desplazamiento basado en la direcci�n de la puerta
        Vector2 offset = door.GetOffset(); // Implementa en el script Door un m�todo que devuelva la direcci�n
        return (Vector2)door.transform.position + offset;
    }

    // Devuelve un �ndice aleatorio para seleccionar una habitaci�n
    public int Rand()
    {
        return Random.Range(0, rooms.Count);
    }
     */
}
