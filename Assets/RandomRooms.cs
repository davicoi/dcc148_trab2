using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class RoomGenerator
{
    // Definindo as constantes para as direções
    const int FLAG_UP = 0x04;
    const int FLAG_RIGHT = 0x02;
    const int FLAG_DOWN = 0x01;
    const int FLAG_LEFT = 0x08;

    // Definindo as variáveis para armazenar o estado da sala
    private int roomsWidth = 0;
    private int[,] roomList;
    public float genCount = 0;

    UnityEngine.Vector3 lastRoomPosition = UnityEngine.Vector3.zero;

    // Função para criar a matriz de salas
    void roomsArray(int width)
    {
        roomList = new int[width, width];
        roomsWidth = width;
    }

    // Função para gerar uma direção aleatória não conectada
    int unlinkedDir(int x, int y)
    {
        List<int> list = new List<int>();

        // Verificar se as direções estão disponíveis
        if (x > 0 && roomList[y, x - 1] == 0)
            list.Add(FLAG_LEFT);
        if (x + 1 < roomsWidth && roomList[y, x + 1] == 0)
            list.Add(FLAG_RIGHT);
        if (y + 1 < roomsWidth && roomList[y + 1, x] == 0)
            list.Add(FLAG_DOWN);
        if (y > 0 && roomList[y - 1, x] == 0)
            list.Add(FLAG_UP);

        // Se nenhuma direção estiver disponível, retorna -1
        if (list.Count == 0)
            return -1;
        
        // a randomização não está satisfatória
        for (int i = 0; i < list.Count; i++) {
            int j = UnityEngine.Random.Range(i, list.Count);
            (list[i], list[j]) = (list[j], list[i]);
        }

        // Retorna uma direção aleatória
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    int opositeDir(int dir) {
        switch (dir) {
            case FLAG_UP: return FLAG_DOWN;
            case FLAG_RIGHT: return FLAG_LEFT;
            case FLAG_DOWN: return FLAG_UP;
            case FLAG_LEFT: return FLAG_RIGHT;
        }
        return 0;
    }

    // Função para imprimir a lista de salas
    public void printRoomList()
    {
        const string conv = "_^>3v567<9ABCDEF";
        string str = "";
        for (int y = 0; y < roomsWidth; y++)
        {
            for (int x = 0; x < roomsWidth; x++)
            {
                // switch (roomList[y, x])
                // {
                //     case FLAG_UP: str += "🠅"; break;
                //     case FLAG_RIGHT: str += ">"; break;
                //     case FLAG_DOWN: str += "🠇"; break;
                //     case FLAG_LEFT: str += "<"; break;
                //     default: str += "_"; break;
                // }
                str += conv[roomList[y, x]];
            }
            str += "\n";
        }
        UnityEngine.Debug.Log(str);
    }

    // Função para gerar as salas aleatórias
    public int[,] randomRooms(int width, int count, int startX = 0, int startY = 0)
    {
        roomsArray(width);
        genCount = 0;

        int dir = 0, x = startX, y = startY;
        int oldDir = 0;
        int oldX = x, oldY = y;
        while (count-- > 0)
        {
            if (x < 0 || x >= roomsWidth || y < 0 || y >= roomsWidth)
                break;

            roomList[y, x] = opositeDir(oldDir);
            // roomList[y, x] = 0;
            
            genCount++;
            lastRoomPosition.x = x;
            lastRoomPosition.z = y;

            dir = unlinkedDir(x, y);
            if (dir < 0)
                break;

            roomList[y, x] |= dir;
            oldDir = dir;
            oldX = x;
            oldY = y;

            // Mover na direção escolhida
            switch (dir)
            {
                case FLAG_UP: y--; break;
                case FLAG_RIGHT: x++; break;
                case FLAG_DOWN: y++; break;
                case FLAG_LEFT: x--; break;
            }
        }

        roomList[oldY, oldX] &= ~dir;

        return roomList;
    }

    public UnityEngine.Vector3 getLastRoomPosition() {
        return lastRoomPosition;
    }
}
