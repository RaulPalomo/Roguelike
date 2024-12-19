using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen2 : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(); // Lista de habitaciones
    public int maxRooms = 30; // Número máximo de habitaciones a generar
    private int currentRoomCount = 0; // Contador de habitaciones generadas
    private Grid grid;
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

    private Queue<(GameObject, Door)> roomsQueue = new Queue<(GameObject, Door)>(); // Cola para manejar habitaciones pendientes

    private void Start()
    {
        grid = FindObjectOfType<Grid>();

        // Generar la primera habitación en (0, 0, 0)
        GameObject firstRoom = Instantiate(rooms[Rand(rooms.Count)]);
        firstRoom.transform.SetParent(grid.transform);
        firstRoom.transform.position = grid.CellToWorld(Vector3Int.zero);

        currentRoomCount++;
        occupiedPositions.Add(Vector3Int.zero);

        // Agregar las puertas de la primera habitación a la cola
        EnqueueDoors(firstRoom);

        // Iniciar la generación continua de habitaciones
        StartCoroutine(GenerateRoomsCoroutine());
    }

    // Genera habitaciones continuamente usando una cola
    private IEnumerator GenerateRoomsCoroutine()
    {
        while (currentRoomCount < maxRooms && roomsQueue.Count > 0)
        {
            (GameObject currentRoom, Door door) = roomsQueue.Dequeue();

            if (door.used) continue; // Saltar si la puerta ya fue usada

            bool found = false;
            int attempts = 0;

            while (!found && attempts < 20)
            {
                GameObject newRoom = Instantiate(rooms[Rand(rooms.Count)]);
                newRoom.transform.SetParent(grid.transform);
                int oppositeDirection = GetOppositeDirection(door.direction);
                Door[] newDoors = newRoom.GetComponentsInChildren<Door>();

                foreach (Door newDoor in newDoors)
                {
                    if (newDoor.direction == oppositeDirection)
                    {
                        // Calcular offset en coordenadas globales
                        Vector3 offset = door.transform.position - newDoor.transform.position;
                        Vector3 potentialPosition = currentRoom.transform.position + offset;

                        // Alinea la posición al grid
                        Vector3Int cellPosition = grid.WorldToCell(potentialPosition);
                        potentialPosition = grid.CellToWorld(cellPosition);

                        if (DoesRoomFit(newRoom, cellPosition))
                        {
                            newRoom.transform.position = potentialPosition;
                            SetRoom(door, newDoor, newRoom);
                            currentRoomCount++;
                            occupiedPositions.Add(cellPosition);
                            

                            // Agregar nuevas puertas a la cola
                            EnqueueDoors(newRoom);
                            found = true;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    Destroy(newRoom);
                    attempts++;
                    yield return null; // Esperar un frame antes de reintentar
                }
            }

            if (!found)
            {
                Debug.LogWarning("No se pudo colocar una habitación después de varios intentos.");
            }
        }
    }

    // Agrega todas las puertas no usadas de una habitación a la cola
    private void EnqueueDoors(GameObject room)
    {
        Door[] doors = room.GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            if (!door.used)
            {
                roomsQueue.Enqueue((room, door));
            }
        }
    }

    private bool DoesRoomFit(GameObject room, Vector3Int position)
    {
        if (occupiedPositions.Contains(position)) return false;

        Collider2D roomCollider = room.GetComponent<Collider2D>();
        if (roomCollider != null)
        {
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f);
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.gameObject.CompareTag("Room") && overlap.gameObject != room)
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void SetRoom(Door oldDoor, Door newDoor, GameObject room)
    {
        oldDoor.used = true;
        newDoor.used = true;
    }

    private int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case 0: return 2; // Norte -> Sur
            case 1: return 3; // Este -> Oeste
            case 2: return 0; // Sur -> Norte
            case 3: return 1; // Oeste -> Este
            default: return -1;
        }
    }

    public int Rand(int num)
    {
        return Random.Range(0, num);
    }
}

