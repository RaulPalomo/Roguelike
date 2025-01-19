using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMapGenerator : MonoBehaviour
{
    public int roomNum = 10; // Número total de salas a generar
    private Grid grid;
    public GameObject defaultRoom; // Prefab de la sala inicial (sin puertas)
    public List<GameObject> roomPrefabs; // Prefabs de las salas con puertas
    private List<GameObject> rooms=new List<GameObject>();
    private int roomCount=0;
    public List<GameObject> enemyPrefabs;
    public int minEnemiesPerRoom = 1;
    public int maxEnemiesPerRoom = 5;
    private int EnemyCount=0;
    void Start()
    {
        grid = FindObjectOfType<Grid>();
        GameObject newRoom = Instantiate(defaultRoom, grid.transform);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        roomNum += player.GetComponent<PlayerMovement>().loop;
        maxEnemiesPerRoom += player.GetComponent<PlayerMovement>().loop/3;
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
        
        int roomIndex = 0; 

        foreach (GameObject room in rooms)
        {
            roomIndex++;
            List<int> doors = new List<int>();
            Vector2 position = room.transform.position;

            
            foreach (GameObject otherRoom in rooms)
            {
               
                if (room == otherRoom) continue;

                Vector2 otherPosition = otherRoom.transform.position;

                
                if (otherPosition.x == position.x + 14 && otherPosition.y == position.y)
                {
                    if (!doors.Contains(0)) doors.Add(0);
                }
                if (otherPosition.x == position.x - 14 && otherPosition.y == position.y)
                {
                    if (!doors.Contains(2)) doors.Add(2);
                }
                if (otherPosition.y == position.y + 10 && otherPosition.x == position.x)
                {
                    if (!doors.Contains(1)) doors.Add(1); 
                }
                if (otherPosition.y == position.y - 10 && otherPosition.x == position.x)
                {
                    if (!doors.Contains(3)) doors.Add(3); 
                }
            }

            
            doors.Sort();
            

            
            bool replaced = false;
            List<GameObject> possibleRoom=new List<GameObject>();
            foreach (GameObject prefab in roomPrefabs)
            {
                
                Door[] prefabDoors = prefab.GetComponentsInChildren<Door>();
                List<int> prefabDoorDirections = prefabDoors.Select(d => d.direction).ToList();
                prefabDoorDirections.Sort();

                

                
                if (prefabDoorDirections.SequenceEqual(doors))
                {
                    possibleRoom.Add(prefab);
                    replaced = true;
                    
                }
            }

            if (!replaced)
            {
                
                SceneManager.LoadScene("Sample Scene");
            }
            else
            {
                
                GameObject prefab = possibleRoom[Random.Range(0,possibleRoom.Count)];
                
                GameObject newRoom = Instantiate(prefab, room.transform.parent);
                newRoom.transform.localPosition = room.transform.localPosition;
                room.name=newRoom.name;
                Destroy(room);
            }
        }
        SpawnEnemiesInRooms();
    }
    private void SpawnEnemiesInRooms()
    {
        foreach (GameObject room in rooms)
        {
            
            if (room != rooms[0]&& (!room.name.Contains("room3.1 1") && !room.name.Contains("room3.0.1")))
            {
                int enemyCount = Random.Range(minEnemiesPerRoom, maxEnemiesPerRoom + 1);  // Determinar la cantidad de enemigos

                for (int i = 0; i < enemyCount; i++)
                {
                    int enemyIndex = Random.Range(0, enemyPrefabs.Count);  // Elegir un enemigo aleatorio
                    Vector3 spawnPosition = GetRandomPositionInRoom(room);
                    spawnPosition.z = -1;
                    GameObject newEnemy = Instantiate(enemyPrefabs[enemyIndex], spawnPosition, Quaternion.identity);  // Spawnear enemigo dentro de la sala
                    EnemyCount++;
                }
            }
        }
        EventController eventController = FindObjectOfType<EventController>();
        eventController.enemiesToDefeat = EnemyCount;
    }
    private Vector3 GetRandomPositionInRoom(GameObject room)
    {
        BoxCollider2D roomCollider = room.GetComponent<BoxCollider2D>();
        if (roomCollider != null)
        {
            Vector3 roomCenter = room.transform.position;
            float spawnRangeX = roomCollider.size.x * 0.3f;  
            float spawnRangeY = roomCollider.size.y * 0.3f;  

            float xOffset = Random.Range(-spawnRangeX, spawnRangeX);
            float yOffset = Random.Range(-spawnRangeY, spawnRangeY);

            return roomCenter + new Vector3(xOffset, yOffset);  // Desplazamiento desde el centro
        }
        else
        {
           
            return room.transform.position;  // Si no hay colisionador, devuelve el centro
        }
    }



}
