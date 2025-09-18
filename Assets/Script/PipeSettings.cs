using UnityEngine;

/// <summary>
/// Configuration asset for controlling pipe generation.
/// Stores grid size, growth timing, and prefabs used for rendering.
/// </summary>
[CreateAssetMenu(fileName = "PipeSettings", menuName = "Screensaver Settings")]
public class PipeSettings : ScriptableObject
{
    [Header("Grid Settings")]
    [Tooltip("Dimensions of the 3D grid where pipes can grow.")]
    public Vector3Int gridSize = new Vector3Int(10, 10, 10);

    [Tooltip("Distance between grid cells in world units.")]
    public float spacing = 1f;

    [Header("Pipe Growth Settings")]
    [Tooltip("Delay (in seconds) between adding each new pipe segment.")]
    public float growthDelay = 0.05f;

    [Tooltip("Thickness of the pipe segments.")]
    public float pipeThickness = 0.4f;

    [Tooltip("Number of pipes before the system resets the grid.")]
    public int maxPipes = 4;
    
    [Header("Pipe Prefabs")]
    [Tooltip("Prefab used at the start and end of each pipe segment (sphere) and change in direction during growth")]
    public GameObject spherePrefab;

    [Tooltip("Prefab used for the body of each pipe segment (cylinder).")]
    public GameObject cylinderPrefab;
}