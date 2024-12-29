using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NewMapGenerator : MonoBehaviour
{
    public int roomNum = 10; // Número total de salas a generar
    private Grid grid;
    public GameObject defaultRoom; // Prefab de la sala inicial (sin puertas)
    public List<GameObject> roomPrefabs; // Prefabs de las salas con puertas
    private List<GameObject> rooms=new List<GameObject>();
    private int roomCount=0;
    

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        GameObject newRoom = Instantiate(defaultRoom, grid.transform);
        rooms.Add(newRoom);
        GenerateMap(newRoom);
        
    }

    void GenerateMap(GameObject room)
    {
        
        if (roomCount < roomNum)
        {
            Vector2 position = room.transform.position;
            GameObject newRoom=Instantiate(defaultRoom,grid.transform);
            bool validPosition;
            do 
            {
                int direction = Random.Range(0, 4);
                validPosition = true;
                if (direction == 0)
                {
                    position.x = room.transform.position.x + 14;
                }
                else if (direction == 1)
                {
                    position.y = room.transform.position.y + 10;
                }
                else if (direction == 2)
                {
                    position.x = room.transform.position.x - 14;
                }
                else if (direction == 3)
                {
                    position.y = room.transform.position.y - 10;
                }
                foreach(GameObject r in rooms)
                {
                    if((Vector2)r.transform.position == position)
                    {
                        validPosition = false;                                                                                     
                    }
                }                             
            }while (!validPosition);
            newRoom.transform.localPosition = position;
            rooms.Add(newRoom);
            roomCount++;
            GenerateMap(newRoom);
        }
        else
        {
            ReplaceRooms();
        }




    }
    public void ReplaceRooms()
    {
        Debug.Log($"Starting ReplaceRooms... Total rooms: {rooms.Count}");
        int roomIndex = 0; // Índice para depuración

        foreach (GameObject room in rooms)
        {
            roomIndex++;
            List<int> doors = new List<int>();
            Vector2 position = room.transform.position;

            // Identificar las puertas (vecinos) de la habitación actual
            foreach (GameObject otherRoom in rooms)
            {
                // Evitar comparar la habitación consigo misma
                if (room == otherRoom) continue;

                Vector2 otherPosition = otherRoom.transform.position;

                // Comparar posiciones relativas para determinar puertas
                if (otherPosition.x == position.x + 14 && otherPosition.y == position.y)
                {
                    if (!doors.Contains(0)) doors.Add(0); // Derecha
                }
                if (otherPosition.x == position.x - 14 && otherPosition.y == position.y)
                {
                    if (!doors.Contains(2)) doors.Add(2); // Izquierda
                }
                if (otherPosition.y == position.y + 10 && otherPosition.x == position.x)
                {
                    if (!doors.Contains(1)) doors.Add(1); // Arriba
                }
                if (otherPosition.y == position.y - 10 && otherPosition.x == position.x)
                {
                    if (!doors.Contains(3)) doors.Add(3); // Abajo
                }
            }

            // Ordenar puertas y depurar
            doors.Sort();
            Debug.Log($"Room {roomIndex} at {position}: Doors = {string.Join(",", doors)}");

            // Intentar encontrar un prefab que coincida
            bool replaced = false;
            foreach (GameObject prefab in roomPrefabs)
            {
                // Obtener las puertas del prefab
                Door[] prefabDoors = prefab.GetComponentsInChildren<Door>();
                List<int> prefabDoorDirections = prefabDoors.Select(d => d.direction).ToList();
                prefabDoorDirections.Sort();

                Debug.Log($"Checking prefab {prefab.name}: Doors = {string.Join(",", prefabDoorDirections)}");

                // Comparar puertas
                if (prefabDoorDirections.SequenceEqual(doors))
                {
                    Debug.Log($"Match found! Replacing room {roomIndex} with prefab {prefab.name}.");
                    GameObject newRoom = Instantiate(prefab, room.transform.parent);
                    newRoom.transform.localPosition = room.transform.localPosition;

                    // Destruir la habitación original
                    Destroy(room);
                    replaced = true;
                    break;
                }
            }

            if (!replaced)
            {
                Debug.LogWarning($"No matching prefab found for room {roomIndex}!");
            }
        }
    }


}
