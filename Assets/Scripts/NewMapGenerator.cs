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
        int roomCount =0;
        foreach (GameObject room in rooms)
        {
            roomCount++;                                
            List<int> doors = new List<int>();
            Vector2 position = room.transform.position;
            foreach (GameObject room2 in rooms)
            {
                Vector2 otherPosition = room2.transform.position;
                if(otherPosition.x == position.x + 14)
                {
                    doors.Add(0);
                }
                if (otherPosition.x == position.x - 14)
                {
                    doors.Add(2);
                }
                if (otherPosition.y == position.y + 10)
                {
                    doors.Add(1);
                }
                if (otherPosition.y == position.y - 10)
                {
                    doors.Add(3);
                }
                doors.Sort();
                foreach (int d in doors)
                {
                    Debug.Log(roomCount +" "+d);
                }
                
                foreach (GameObject rom in roomPrefabs)
                {
                    List<int> newDoors = new List<int>();

                    Door[] doorsRom = rom.GetComponentsInChildren<Door>();
                    /*foreach (Door door in doorsRom)
                    {
                        newDoors.Add(door.direction);
                        Debug.Log("a"+door.direction);
                    }*/
                    newDoors.Sort();
                    if (newDoors.SequenceEqual(doors))
                    {
                        Debug.Log("AAAAA");
                        // Instanciar el nuevo prefab en la posición de la habitación actual
                        GameObject newRoom = Instantiate(rom, room.transform.parent);

                        // Asegurarse de que esté en la misma posición que la habitación original
                        newRoom.transform.localPosition = room.transform.localPosition;

                        // Destruir la habitación original
                        Destroy(room);

                        // Salir del bucle, ya que se encontró un prefab válido
                        break;
                    }
                }
                break;
            }
            
        }
        
        
    }

}
