using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the growth of pipes within a 3D grid.
/// Uses PipeSettings for growth timing and prefab references,
/// and SpawnGrid to ensure no overlapping pipes and find available neighbouring cells.
/// PipeRenderer is used to viualize the pipes in the scene.
/// </summary>
public class PipeGenerator : MonoBehaviour
{
    private PipeSettings settings;   // input data
    private SpawnGrid grid;         // tracks which cells are occupied
    private PipeRenderer renderer;

    /// <summary>
    /// Initializes the generator with configuration and grid logic.
    /// Called by PipeManager when spawning a new generator instance.
    /// </summary>
    public void Initialize(PipeSettings settings, SpawnGrid grid, PipeRenderer renderer)
    {
        this.settings = settings;
        this.grid = grid;
        this.renderer = renderer;

        StartCoroutine(RunPipes());
    }

    /// <summary>
    /// Entry coroutine: spawns pipes until the grid is full.
    /// </summary>
    private IEnumerator RunPipes()
    {
        int pipeCount = 0;

        while (true)
        {
            if (pipeCount >= settings.maxPipes)
            {
                // Destroy all previously spawned pipe objects
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }

                // Reset logic grid
                grid = new SpawnGrid(settings.gridSize);
                pipeCount = 0;
            }

            if (!grid.PickFirstFree(out Vector3Int startCell))
                yield break; // grid full

        yield return StartCoroutine(GrowPipe(startCell));
        pipeCount++;
        }
    }

    /// <summary>
    /// Grows a single pipe step by step until blocked.
    /// </summary>
    private IEnumerator GrowPipe(Vector3Int startCell)
    {
        // Pick a random color for this pipe
        Material mat = renderer.MakePipeMaterial();

        Vector3Int currentCell = startCell;
        grid.Occupy(currentCell);

        renderer.SpawnEdge(currentCell, mat);

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
                renderer.SpawnEdge(currentCell, mat);
                yield break;
            }

            // Pick a random free neighbor and grow into it
            Vector3Int nextCell = neighbours[Random.Range(0, neighbours.Count)];
            Vector3Int direction = nextCell - currentCell;

            // Check for a change in direction
            if (prevDirection != Vector3Int.zero && direction != prevDirection)
            {
            // Spawn a sphere at the corner to smooth the bend
            renderer.SpawnEdge(currentCell, mat);
            }

            renderer.SpawnBody(currentCell, direction, mat);

            grid.Occupy(nextCell);
            currentCell = nextCell;
            prevDirection = direction;

            // Delay growth for visual effect
            yield return new WaitForSeconds(settings.growthDelay);
        }
    }
}