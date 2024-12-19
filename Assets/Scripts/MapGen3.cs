using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen3 : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(); // Lista de habitaciones
    public int maxRooms = 30; // Número máximo de habitaciones a generar
    private int currentRoomCount = 0; // Contador de habitaciones generadas
    private Grid grid;
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        grid = FindObjectOfType<Grid>();

        // Generar la primera habitación en (0, 0, 0)
        GameObject firstRoom = Instantiate(rooms[Rand(rooms.Count)]);
        firstRoom.transform.SetParent(grid.transform);
        firstRoom.transform.position = grid.CellToWorld(Vector3Int.zero);

        currentRoomCount++;
        occupiedPositions.Add(Vector3Int.zero);

        // Asignar las puertas de la primera habitación
        AssignDoor(firstRoom);
    }

    // Asigna las puertas de la sala y genera nuevas habitaciones
    public void AssignDoor(GameObject room)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanzó el límite, no generar más habitaciones

        Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener las puertas de la habitación
        foreach (Door door in doors)
        {
            if (!door.used) // Si la puerta no ha sido usada
            {
                Debug.Log("Asignando puerta con dirección: " + door.direction); // Mostrar dirección de la puerta
                GenerateRoom(door); // Generar una nueva habitación a partir de esta puerta
            }
        }
    }

    // Genera una nueva habitación a partir de una puerta dada
    public void GenerateRoom(Door door)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanzó el límite, no generar más habitaciones

        bool found = false;
        int attempts = 0; // Intentos para generar una habitación válida
        GameObject room = null;

        while (!found && attempts < 20) // Máximo 20 intentos para colocar la habitación
        {
            room = Instantiate(rooms[Rand(rooms.Count)]); // Instanciar una nueva habitación
            room.transform.SetParent(grid.transform);
            int oppositeDoor = GetOppositeDirection(door.direction);
            Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener puertas de la nueva habitación

            foreach (Door newDoor in doors)
            {
                if (newDoor.direction == oppositeDoor)
                {
                    // Calcular la posición potencial de la nueva habitación
                    Vector2 oldDoorPosition = door.transform.position;
                    Vector2 newDoorPosition = newDoor.transform.position;
                    Vector2 offset = oldDoorPosition - newDoorPosition;
                    Vector3 potentialPosition = room.transform.position + (Vector3)offset;

                    // Alinea la posición al grid
                    Vector3Int cellPosition = grid.WorldToCell(potentialPosition);
                    potentialPosition = grid.CellToWorld(cellPosition);

                    // Verificar si la posición es válida
                    if (DoesRoomFit(room, cellPosition))
                    {
                        room.transform.position = potentialPosition;
                        SetRoom(door, newDoor, room); // Conectar las puertas
                        currentRoomCount++; // Incrementar el contador de habitaciones
                        occupiedPositions.Add(cellPosition);
                        found = true;
                        break;
                    }
                }
            }

            // Si no se pudo colocar la habitación, destruirla y volver a intentar
            if (!found)
            {
                Destroy(room);
                attempts++;
            }
        }

        if (!found)
        {
            Debug.LogWarning("No se pudo colocar una habitación válida después de varios intentos.");
        }
    }

    private bool DoesRoomFit(GameObject room, Vector3Int position)
    {
        if (occupiedPositions.Contains(position)) return false; // Verificar si la posición ya está ocupada

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

    public void SetRoom(Door oldDoor, Door newDoor, GameObject room)
    {
        oldDoor.used = true;  // Marcar la puerta antigua como usada
        newDoor.used = true; // Marcar la nueva puerta como usada

        // Llamar a AssignDoor para seguir generando habitaciones
        AssignDoor(room);
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
