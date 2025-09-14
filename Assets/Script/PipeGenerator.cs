using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PipeSettings gives data to PipeGenerator and CellGrid3D handles logic of occupied cells
public class PipeGenerator : MonoBehaviour
{
    private PipeSettings settings;
    private CellGrid3D grid;
    private Vector3 half;

    public void Initialize(PipeSettings settings, CellGrid3D grid)
    {
        this.settings = settings;
        this.grid = grid;

        half = new Vector3(
            (settings.gridSize.x - 1) / 2f,
            (settings.gridSize.y - 1) / 2f,
            (settings.gridSize.z - 1) / 2f
        );

        StartCoroutine(RunPipes());
    }

    private IEnumerator RunPipes()
    {
        for (int i = 0; i < settings.maxPipes; i++)
        {
            if (!grid.PickFirstFree(out Vector3Int startCell))
                yield break; // grid full

            yield return StartCoroutine(GrowPipe(startCell));
        }
    }

    private IEnumerator GrowPipe(Vector3Int startCell)
    {
        Vector3Int currentCell = startCell;
        grid.Occupy(currentCell);

        SpawnSphere(currentCell);

        int segments = 0;
        Vector3Int prevDirection = Vector3Int.zero;

        while (segments < settings.maxSegmentsPerPipe)
        {
            List<Vector3Int> neighbouringCell = new List<Vector3Int>(grid.AvailableNeighbouringCell(currentCell));
            neighbouringCell.RemoveAll(n => n - currentCell == -prevDirection);

            if (neighbouringCell.Count == 0)
            {
                SpawnSphere(currentCell);
                yield break;
            }

            Vector3Int nextCell = neighbouringCell[Random.Range(0, neighbouringCell.Count)];
            Vector3Int direction = nextCell - currentCell;

            SpawnCylinder(currentCell, direction);

            grid.Occupy(nextCell);
            currentCell = nextCell;
            prevDirection = direction;

            segments++;
            yield return new WaitForSeconds(settings.growthDelay);
        }

        SpawnSphere(currentCell);
    }

    private void SpawnSphere(Vector3Int cell)
    {
        if (settings.spherePrefab == null) return;
        Vector3 pos = GridToWorld(cell);
        Instantiate(settings.spherePrefab, pos, Quaternion.identity, transform);
    }

    private void SpawnCylinder(Vector3Int fromCell, Vector3Int direction)
    {
        if (settings.cylinderPrefab == null) return;

        Vector3 from = GridToWorld(fromCell);
        Vector3 to = GridToWorld(fromCell + direction);

        Vector3 mid = (from + to) / 2f;
        Vector3 dir = (to - from).normalized;

        GameObject cyl = Instantiate(settings.cylinderPrefab, mid, Quaternion.identity, transform);
        cyl.transform.up = dir;
        cyl.transform.localScale = new Vector3(
            cyl.transform.localScale.x,
            (to - from).magnitude / 2f,
            cyl.transform.localScale.z
        );
    }

    private Vector3 GridToWorld(Vector3Int cell)
    {
        return new Vector3(
            (cell.x - half.x) * settings.spacing,
            (cell.y - half.y) * settings.spacing,
            (cell.z - half.z) * settings.spacing
        );
    }
}
