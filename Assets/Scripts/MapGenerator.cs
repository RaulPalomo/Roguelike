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

        // Genera la primera habitación
        GameObject firstRoom = Instantiate(rooms[Rand()]);
        firstRoom.transform.SetParent(grid.transform);

        // Obtén todas las puertas de la primera habitación
        Door[] doors = firstRoom.GetComponentsInChildren<Door>();

        // Para cada puerta en la primera habitación, genera una nueva habitación
        foreach (Door door in doors)
        {
            GenerateRoom(door);
        }
    }

    // Genera una nueva habitación conectada a la puerta dada
    public void GenerateRoom(Door door)
    {
        // Obtén la posición de la puerta
        Vector2 doorPosition = door.transform.position;

        // Elige una habitación aleatoria de la lista
        GameObject newRoomPrefab = rooms[Rand()];

        // Instancia una nueva habitación
        GameObject newRoom = Instantiate(newRoomPrefab);

        // Ajusta la posición de la nueva habitación para conectarla a la puerta
        Vector2 newRoomPosition = GetRoomPositionFromDoor(door);

        // Establece la posición de la nueva habitación
        newRoom.transform.position = newRoomPosition;

        // Configura las puertas de la nueva habitación
        Door[] newRoomDoors = newRoom.GetComponentsInChildren<Door>();
        foreach (Door newDoor in newRoomDoors)
        {
            // Asegúrate de que las nuevas puertas no se conecten a sí mismas
            if (Vector2.Distance(newDoor.transform.position, doorPosition) > 0.1f)
            {
                GenerateRoom(newDoor); // Genera nuevas habitaciones desde las puertas restantes
            }
        }
    }

    // Calcula la posición de la nueva habitación a partir de una puerta existente
    private Vector2 GetRoomPositionFromDoor(Door door)
    {
        // Calcula el desplazamiento basado en la dirección de la puerta
        Vector2 offset = door.GetOffset(); // Implementa en el script Door un método que devuelva la dirección
        return (Vector2)door.transform.position + offset;
    }

    // Devuelve un índice aleatorio para seleccionar una habitación
    public int Rand()
    {
        return Random.Range(0, rooms.Count);
    }
     */
}
