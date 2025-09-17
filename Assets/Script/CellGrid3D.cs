using System.Collections.Generic;
using UnityEngine;

//the logic to track which cells are currently occupied based on coordinates
//checking the boundaries and neighbouring grids
public class CellGrid3D
{
    private readonly bool[,,] occupied;

    public readonly Vector3Int Size;

    public CellGrid3D(Vector3Int gridSize)
    {
        Size = new Vector3Int(
            Mathf.Max(1, gridSize.x),
            Mathf.Max(1, gridSize.y),
            Mathf.Max(1, gridSize.z)
        );
        occupied = new bool[Size.x, Size.y, Size.z];
    }

    //returning true if the current cell is in the Grid
    public bool InBounds(Vector3Int cell) =>
        cell.x >= 0 && cell.x < Size.x &&
        cell.y >= 0 && cell.y < Size.y &&
        cell.z >= 0 && cell.z < Size.z;

    
    // in grid and not occupied
    public bool IsAvailable(Vector3Int cell) =>
        InBounds(cell) && !occupied[cell.x, cell.y, cell.z];

    //mark the cell as occupied
    public void Occupy(Vector3Int cell)
    {
        if (InBounds(cell)) occupied[cell.x, cell.y, cell.z] = true;
    }

    public IEnumerable<Vector3Int> AvailableNeighbouringCell(Vector3Int cell)
    {
        var nextcell = cell + Vector3Int.right; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.left; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.up; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.down; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.forward; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.back; if (IsAvailable(nextcell)) yield return nextcell;
    }

       public IEnumerable<Vector3Int> AllOccupied()
    {
        for (int z = 0; z < Size.z; z++)
        for (int y = 0; y < Size.y; y++)
        for (int x = 0; x < Size.x; x++)
            if (occupied[x, y, z]) yield return new Vector3Int(x, y, z);
    }

    //returns first free cell found through scanning
    public bool PickFirstFree(out Vector3Int cell)
    {
        for (int z = 0; z < Size.z; z++)
        for (int y = 0; y < Size.y; y++)
        for (int x = 0; x < Size.x; x++)
            if (!occupied[x, y, z]) { cell = new Vector3Int(x, y, z); return true; }
        cell = default; 
        return false;
    }

    public void Clear()
    {
        for (int z = 0; z < Size.z; z++)
        for (int y = 0; y < Size.y; y++)
        for (int x = 0; x < Size.x; x++)
            occupied[x, y, z] = false;
    }


}
