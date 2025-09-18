using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the growth of pipes within a 3D grid.
/// Uses PipeSettings for growth timing and prefab references,
/// and SpawnGrid to ensure no overlapping pipes and find available neighbouring cells.
/// </summary>
public class PipeGenerator : MonoBehaviour
{
    private PipeSettings settings;   // input data
    private SpawnGrid grid;         // tracks which cells are occupied
    private Vector3 half;            // centers object

    /// <summary>
    /// Initializes the generator with configuration and grid logic.
    /// Called by PipeManager when spawning a new generator instance.
    /// </summary>
    public void Initialize(PipeSettings settings, SpawnGrid grid)
    {
        this.settings = settings;
        this.grid = grid;

        // Calculate offset so (0,0,0) aligns with grid center
        half = new Vector3(
            (settings.gridSize.x - 1) / 2f,
            (settings.gridSize.y - 1) / 2f,
            (settings.gridSize.z - 1) / 2f
        );

        StartCoroutine(RunPipes());
    }

    /// <summary>
    /// Entry coroutine: spawns pipes until the grid is full.
    /// </summary>
    private IEnumerator RunPipes()
    {
        while (grid.PickFirstFree(out Vector3Int startCell))
        {
            yield return StartCoroutine(GrowPipe(startCell));
        }
    }

    /// <summary>
    /// Grows a single pipe step by step until blocked.
    /// </summary>
    private IEnumerator GrowPipe(Vector3Int startCell)
    {
         // Pick a random color for this pipe
        Material pipeMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        pipeMaterial.color = new Color(Random.value, Random.value, Random.value);

        Vector3Int currentCell = startCell;
        grid.Occupy(currentCell);

        SpawnSphere(currentCell, pipeMaterial);

        Vector3Int prevDirection = Vector3Int.zero;

        while (true) // keep growing until no valid moves
        {
            // Collect all neighboring free cells
            var neighbours = new List<Vector3Int>(grid.AvailableNeighbouringCell(currentCell));

            // Don’t allow a direct backtrack
            neighbours.RemoveAll(n => n - currentCell == -prevDirection);

            if (neighbours.Count == 0)
            {
                // Dead end
                SpawnSphere(currentCell, pipeMaterial);
                yield break;
            }

            // Pick a random free neighbor and grow into it
            Vector3Int nextCell = neighbours[Random.Range(0, neighbours.Count)];
            Vector3Int direction = nextCell - currentCell;

            SpawnCylinder(currentCell, direction, pipeMaterial);

            grid.Occupy(nextCell);
            currentCell = nextCell;
            prevDirection = direction;

            // Delay growth for visual effect
            yield return new WaitForSeconds(settings.growthDelay);
        }
    }

    /// <summary>
    /// Spawns a sphere prefab at the given grid cell.
    /// Used for pipe start and end segments
    /// </summary>
    private void SpawnSphere(Vector3Int cell, Material material)
    {
        if (settings.spherePrefab == null) return;
        Vector3 pos = GridToWorld(cell);

        GameObject go = Instantiate(settings.spherePrefab, pos, Quaternion.identity, transform);

        var renderer = go.GetComponent<Renderer>();
        if (renderer != null) renderer.material = material;
    }

    /// <summary>
    /// Spawns a cylinder prefab between two grid cells.
    /// Scaled and rotated to connect the cells correctly.
    /// </summary>
    private void SpawnCylinder(Vector3Int fromCell, Vector3Int direction, Material material)
    {
        if (settings.cylinderPrefab == null) return;

        Vector3 from = GridToWorld(fromCell);
        Vector3 to = GridToWorld(fromCell + direction);

        Vector3 mid = (from + to) / 2f;   // center point
        Vector3 dir = (to - from).normalized; // direction vector

        GameObject cyl = Instantiate(settings.cylinderPrefab, mid, Quaternion.identity, transform);
        cyl.transform.up = dir; // orient along pipe direction
        cyl.transform.localScale = new Vector3(
            cyl.transform.localScale.x,
            (to - from).magnitude / 2f, // scale to match distance
            cyl.transform.localScale.z
        );
        
        var renderer = cyl.GetComponent<Renderer>();
        if (renderer != null) renderer.material = material;
    }

    /// <summary>
    /// Converts grid coordinates into world coordinates,
    /// </summary>
    private Vector3 GridToWorld(Vector3Int cell)
    {
        return new Vector3(
            (cell.x - half.x) * settings.spacing,
            (cell.y - half.y) * settings.spacing,
            (cell.z - half.z) * settings.spacing
        );
    }
}