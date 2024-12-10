using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(); // Lista de habitaciones
    public int maxRooms = 30; // Número máximo de habitaciones a generar
    private int currentRoomCount = 0; // Contador de habitaciones generadas
    private Grid grid;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        GameObject firstRoom = Instantiate(rooms[Rand(rooms.Count)]);
        firstRoom.transform.SetParent(grid.transform); // Asegurarse de que la primera habitación sea hija del grid
        currentRoomCount++;
        occupiedPositions.Add(firstRoom.transform.position);
        AssignDoor(firstRoom); // Asignar puertas de la primera habitación
    }

    // Asigna las puertas de la sala y genera nuevas habitaciones
    public void AssignDoor(GameObject room)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanzó el límite, no generar más habitaciones

        room.transform.SetParent(grid.transform); // Asegurarse de que la nueva habitación sea hija del grid
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

        while (!found && attempts < 20) // Máximo 10 intentos para colocar la habitación
        {
            room = Instantiate(rooms[Rand(rooms.Count)]); // Instanciar una nueva habitación
            int oppositeDoor = GetOppositeDirection(door.direction);
            Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener puertas de la nueva habitación

            foreach (Door d in doors)
            {
                if (d.direction == oppositeDoor)
                {
                    // Calcular la posición potencial de la nueva habitación
                    Vector2 oldDoorPosition = door.transform.position;
                    Vector2 newDoorPosition = d.transform.position;
                    Vector2 offset = oldDoorPosition - newDoorPosition;
                    Vector3 potentialPosition = room.transform.position + (Vector3)offset;

                    // Verificar si la posición es válida
                    if (IsPositionValid(room, potentialPosition))
                    {
                        room.transform.position = potentialPosition;
                        SetRoom(door, d, room); // Conectar las puertas
                        currentRoomCount++; // Incrementar el contador de habitaciones
                        occupiedPositions.Add(potentialPosition);
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

    // Verifica si la posición de una nueva habitación es válida usando colisiones
    private bool IsPositionValid(GameObject room, Vector3 position)
    {
        // Mueve la habitación temporalmente a la posición propuesta
        room.transform.position = position;
        if (occupiedPositions.Contains(position))
        {
            return false; // La posición está ocupada
        }
        // Obtener el collider de la habitación
        Collider2D roomCollider = room.GetComponent<Collider2D>();

        if (roomCollider != null)
        {
            // Comprobar si el collider colisiona con otros
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f);
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.gameObject != room) // Ignorar la habitación actual
                {
                    return false; // La posición está ocupada
                }
            }
        }

        // Si no hay colisiones, la posición es válida
        return true;
    }

    // Conecta las dos puertas (la antigua y la nueva) y ajusta la posición de la habitación
    public void SetRoom(Door old, Door newD, GameObject room)
    {
        old.used = true;  // Marcar la puerta antigua como usada
        newD.used = true; // Marcar la nueva puerta como usada

        // Llamar a AssignDoor para seguir generando habitaciones
        AssignDoor(room);
    }

    // Método para obtener la dirección opuesta de la puerta
    public int GetOppositeDirection(int direction)
    {
        switch (direction)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 0;
            case 3: return 1;
            default: return -1;
        }
    }

    // Método aleatorio para seleccionar un índice de la lista
    public int Rand(int num)
    {
        return Random.Range(0, num); // Seleccionar un número aleatorio entre 0 y num-1
    }
}
