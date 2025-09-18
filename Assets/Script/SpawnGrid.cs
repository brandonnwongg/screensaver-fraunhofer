using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The logic behind spawning objects in a 3D grid and cells
/// Tracks wheher a cell is occupied or free
/// Used by the pipe generator to make sure pipes never overlap.
/// </summary>
public class SpawnGrid
{
    private readonly bool[,,] occupied; //true if the cell is occupied

    /// <summary>
    /// Size of the grid (X, Y, Z dimensions).
    /// </summary>
    public readonly Vector3Int Size;

    public SpawnGrid(Vector3Int gridSize)
    {
        Size = new Vector3Int(
            Mathf.Max(1, gridSize.x),
            Mathf.Max(1, gridSize.y),
            Mathf.Max(1, gridSize.z)
        );
        occupied = new bool[Size.x, Size.y, Size.z];
    }

    /// <summary>
    /// Checks if the given cell is inside the grid boundaries.
    /// True if the cell is within the grid; otherwise, false.
    /// </summary>
    public bool InBounds(Vector3Int cell) =>
        cell.x >= 0 && cell.x < Size.x &&
        cell.y >= 0 && cell.y < Size.y &&
        cell.z >= 0 && cell.z < Size.z;

    
    /// <summary>
    /// Returns true if the cell is in the grid and not currently occupied.
    /// </summary>
    public bool IsAvailable(Vector3Int cell) =>
        InBounds(cell) && !occupied[cell.x, cell.y, cell.z];

    /// <summary>
    /// Marks the specified cell as occupied if it is within the grid bounds and theres an object in the cell.
    /// </summary>
    public void Occupy(Vector3Int cell)
    {
        if (InBounds(cell)) occupied[cell.x, cell.y, cell.z] = true;
    }

    /// <summary>
    /// Returns all neighboring cells (6 directions) that are available.
    /// </summary>
    public IEnumerable<Vector3Int> AvailableNeighbouringCell(Vector3Int cell)
    {
        var nextcell = cell + Vector3Int.right; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.left; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.up; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.down; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.forward; if (IsAvailable(nextcell)) yield return nextcell;
            nextcell = cell + Vector3Int.back; if (IsAvailable(nextcell)) yield return nextcell;
    }

    /// <summary>
    /// Returns all occupied cells in the grid.
    /// </summary>
    public IEnumerable<Vector3Int> AllOccupied()
    {
        for (int z = 0; z < Size.z; z++)
        for (int y = 0; y < Size.y; y++)
        for (int x = 0; x < Size.x; x++)
            if (occupied[x, y, z]) yield return new Vector3Int(x, y, z);
    }

    /// <summary>
    /// Finds the first unoccupied cell by scanning.
    /// Returns true if found, false if the grid is completely full.
    /// </summary>
    public bool PickFirstFree(out Vector3Int cell)
    {
        for (int z = 0; z < Size.z; z++)
        for (int y = 0; y < Size.y; y++)
        for (int x = 0; x < Size.x; x++)
            if (!occupied[x, y, z]) { cell = new Vector3Int(x, y, z); return true; }
        cell = default; 
        return false;
    }

}