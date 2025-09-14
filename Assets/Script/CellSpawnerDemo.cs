using UnityEngine;
using System.Collections.Generic;

public class CellSpawnerDemo : MonoBehaviour
{
    public Vector3Int gridSize = new Vector3Int(10,10,10);
    public float spacing = 1f;

    public Color occupiedColor = new Color(1f, 0f, 0f, 0.5f);
    public float fill = 0.5f;

    public int sampleLength = 20;
    public bool avoidBacktrack = true;

    private CellGrid3D grid;
    private Vector3 half;

    void OnEnable()   { Rebuild(); }
    void OnValidate() { Rebuild(); }


    public void Rebuild()
    {
        grid = new CellGrid3D(gridSize);
        half = new Vector3((gridSize.x-1)/2f, (gridSize.y-1)/2f, (gridSize.z-1)/2f);

        // start near the center
        var currentCell = new Vector3Int(gridSize.x/2, gridSize.y/2, gridSize.z/2);
        grid.Occupy(currentCell);

        Vector3Int prevDirection = Vector3Int.zero;
        var options = new List<Vector3Int>(6);

        for (int i = 0; i < sampleLength; i++)
        {
            options.Clear();
            foreach (var nextCell in grid.AvailableNeighbouringCell(currentCell))
            {
                var direction = nextCell + currentCell;
                if (avoidBacktrack && direction == -prevDirection) continue;
                if (grid.IsAvailable(nextCell)) options.Add(nextCell);
            }
            if (options.Count == 0) break;

            var chosen = options[Random.Range(0, options.Count)];
            prevDirection = chosen - currentCell;
            currentCell = chosen;
            grid.Occupy(currentCell);
        }
    }

    void OnDrawGizmos()
    {
        if (grid == null) return;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color  = occupiedColor;

        var cube = Vector3.one * (spacing * fill);
        foreach (var c in grid.AllOccupied())
        {
            var p = new Vector3(
                (c.x - half.x) * spacing,
                (c.y - half.y) * spacing,
                (c.z - half.z) * spacing
            );
            Gizmos.DrawCube(p, cube);
        }
    }

}
