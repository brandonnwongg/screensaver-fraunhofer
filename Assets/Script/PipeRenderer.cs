using UnityEngine;

/// <summary>
/// Presentation layer for pipes.
/// Given grid cells, it spawns and positions the appropriate prefabs
/// </summary>
public class PipeRenderer
{
    private readonly PipeSettings settings; // input data
    private readonly Transform parent;
    private readonly Vector3 half;// centers object

    public PipeRenderer(PipeSettings settings, Transform parent)
    {
        this.settings    = settings;
        this.parent      = parent;
        half = new Vector3(
            (settings.gridSize.x - 1) / 2f,
            (settings.gridSize.y - 1) / 2f,
            (settings.gridSize.z - 1) / 2f
        );
    }

    // Pick a random color for this pipe
    public Material MakePipeMaterial()
    {
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        mat.color = new Color(Random.value, Random.value, Random.value);
        return mat;
    }

    /// <summary>
    /// Spawns a sphere prefab at the given grid cell.
    /// Used for pipe start and end segments
    /// </summary>
    public void SpawnSphere(Vector3Int cell, Material material)
    {
        if (settings.spherePrefab == null) return;

        var go = Object.Instantiate(settings.spherePrefab, GridToWorld(cell), Quaternion.identity, parent);
        go.transform.localScale = Vector3.one * settings.pipeThickness;
        go.GetComponent<Renderer>().material = material;
    }

    /// <summary>
    /// Spawns a cylinder prefab between two grid cells.
    /// Scaled and rotated to connect the cells correctly.
    /// </summary>
    public void SpawnCylinder(Vector3Int fromCell, Vector3Int direction, Material material)
    {
        if (settings.cylinderPrefab == null) return;

        Vector3 from = GridToWorld(fromCell);
        Vector3 to   = GridToWorld(fromCell + direction);

        Vector3 mid = (from + to) / 2f;
        Vector3 dir = (to - from).normalized;

        var cyl = Object.Instantiate(settings.cylinderPrefab, mid, Quaternion.identity, parent);
        cyl.transform.up = dir;
        cyl.transform.localScale = new Vector3(
            settings.pipeThickness,
            (to - from).magnitude / 2f,
            settings.pipeThickness
        );
        cyl.GetComponent<Renderer>().material = material;
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