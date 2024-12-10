using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public List<GameObject> rooms = new List<GameObject>(); // Lista de habitaciones
    public int maxRooms = 30; // N�mero m�ximo de habitaciones a generar
    private int currentRoomCount = 0; // Contador de habitaciones generadas
    private Grid grid;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();

    private void Start()
    {
        grid = FindObjectOfType<Grid>();
        GameObject firstRoom = Instantiate(rooms[Rand(rooms.Count)]);
        firstRoom.transform.SetParent(grid.transform); // Asegurarse de que la primera habitaci�n sea hija del grid
        currentRoomCount++;
        occupiedPositions.Add(firstRoom.transform.position);
        AssignDoor(firstRoom); // Asignar puertas de la primera habitaci�n
    }

    // Asigna las puertas de la sala y genera nuevas habitaciones
    public void AssignDoor(GameObject room)
    {
        if (currentRoomCount >= maxRooms) return; // Si se alcanz� el l�mite, no generar m�s habitaciones

        room.transform.SetParent(grid.transform); // Asegurarse de que la nueva habitaci�n sea hija del grid
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

        while (!found && attempts < 20) // M�ximo 10 intentos para colocar la habitaci�n
        {
            room = Instantiate(rooms[Rand(rooms.Count)]); // Instanciar una nueva habitaci�n
            int oppositeDoor = GetOppositeDirection(door.direction);
            Door[] doors = room.GetComponentsInChildren<Door>(); // Obtener puertas de la nueva habitaci�n

            foreach (Door d in doors)
            {
                if (d.direction == oppositeDoor)
                {
                    // Calcular la posici�n potencial de la nueva habitaci�n
                    Vector2 oldDoorPosition = door.transform.position;
                    Vector2 newDoorPosition = d.transform.position;
                    Vector2 offset = oldDoorPosition - newDoorPosition;
                    Vector3 potentialPosition = room.transform.position + (Vector3)offset;

                    // Verificar si la posici�n es v�lida
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

    // Verifica si la posici�n de una nueva habitaci�n es v�lida usando colisiones
    private bool IsPositionValid(GameObject room, Vector3 position)
    {
        // Mueve la habitaci�n temporalmente a la posici�n propuesta
        room.transform.position = position;
        if (occupiedPositions.Contains(position))
        {
            return false; // La posici�n est� ocupada
        }
        // Obtener el collider de la habitaci�n
        Collider2D roomCollider = room.GetComponent<Collider2D>();

        if (roomCollider != null)
        {
            // Comprobar si el collider colisiona con otros
            Collider2D[] overlaps = Physics2D.OverlapBoxAll(roomCollider.bounds.center, roomCollider.bounds.size, 0f);
            foreach (Collider2D overlap in overlaps)
            {
                if (overlap.gameObject != room) // Ignorar la habitaci�n actual
                {
                    return false; // La posici�n est� ocupada
                }
            }
        }

        // Si no hay colisiones, la posici�n es v�lida
        return true;
    }

    // Conecta las dos puertas (la antigua y la nueva) y ajusta la posici�n de la habitaci�n
    public void SetRoom(Door old, Door newD, GameObject room)
    {
        old.used = true;  // Marcar la puerta antigua como usada
        newD.used = true; // Marcar la nueva puerta como usada

        // Llamar a AssignDoor para seguir generando habitaciones
        AssignDoor(room);
    }

    // M�todo para obtener la direcci�n opuesta de la puerta
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

    // M�todo aleatorio para seleccionar un �ndice de la lista
    public int Rand(int num)
    {
        return Random.Range(0, num); // Seleccionar un n�mero aleatorio entre 0 y num-1
    }
}
