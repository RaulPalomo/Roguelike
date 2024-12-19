using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen3 : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(); // Lista de habitaciones
    public int maxRooms = 30; // N�mero m�ximo de habitaciones a generar
    private int currentRoomCount = 0; // Contador de habitaciones generadas
    private Grid grid;
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

    private void Start()
    {
        grid = FindObjectOfType<Grid>();

        // Generar la primera habitaci�n en (0, 0, 0)
        GameObject firstRoom = Instantiate(rooms[Rand(rooms.Count)]);
        firstRoom.transform.SetParent(grid.transform);
        firstRoom.transform.position = grid.CellToWorld(Vector3Int.zero);

        currentRoomCount++;
        occupiedPositions.Add(Vector3Int.zero);

        // Asignar las puertas de la primera habitaci�n
        AssignDoor(firstRoom);
    }

    // Asigna las puertas de la sala y genera nuevas habitaciones
    public void AssignDoor(GameObject room)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanz� el l�mite, no generar m�s habitaciones

        Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener las puertas de la habitaci�n
        foreach (Door door in doors)
        {
            if (!door.used) // Si la puerta no ha sido usada
            {
                Debug.Log("Asignando puerta con direcci�n: " + door.direction); // Mostrar direcci�n de la puerta
                GenerateRoom(door); // Generar una nueva habitaci�n a partir de esta puerta
            }
        }
    }

    // Genera una nueva habitaci�n a partir de una puerta dada
    public void GenerateRoom(Door door)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanz� el l�mite, no generar m�s habitaciones

        bool found = false;
        int attempts = 0; // Intentos para generar una habitaci�n v�lida
        GameObject room = null;

        while (!found && attempts < 20) // M�ximo 20 intentos para colocar la habitaci�n
        {
            room = Instantiate(rooms[Rand(rooms.Count)]); // Instanciar una nueva habitaci�n
            room.transform.SetParent(grid.transform);
            int oppositeDoor = GetOppositeDirection(door.direction);
            Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener puertas de la nueva habitaci�n

            foreach (Door newDoor in doors)
            {
                if (newDoor.direction == oppositeDoor)
                {
                    // Calcular la posici�n potencial de la nueva habitaci�n
                    Vector2 oldDoorPosition = door.transform.position;
                    Vector2 newDoorPosition = newDoor.transform.position;
                    Vector2 offset = oldDoorPosition - newDoorPosition;
                    Vector3 potentialPosition = room.transform.position + (Vector3)offset;

                    // Alinea la posici�n al grid
                    Vector3Int cellPosition = grid.WorldToCell(potentialPosition);
                    potentialPosition = grid.CellToWorld(cellPosition);

                    // Verificar si la posici�n es v�lida
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

            // Si no se pudo colocar la habitaci�n, destruirla y volver a intentar
            if (!found)
            {
                Destroy(room);
                attempts++;
            }
        }

        if (!found)
        {
            Debug.LogWarning("No se pudo colocar una habitaci�n v�lida despu�s de varios intentos.");
        }
    }

    private bool DoesRoomFit(GameObject room, Vector3Int position)
    {
        if (occupiedPositions.Contains(position)) return false; // Verificar si la posici�n ya est� ocupada

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
