using UnityEngine;

/// <summary>
/// Entry point for the pipes screensaver.
/// Creates the 3D grid, spawns a PipeGenerator instance,
/// and injects the necessary settings and dependencies.
/// </summary>
public class PipeManager : MonoBehaviour
{
    [SerializeField] private PipeSettings settings;       // configuration for pipe growth and prefabs
    [SerializeField] private PipeGenerator generatorPrefab; // prefab containing the pipe generation logic

    private SpawnGrid grid;                 // logical grid tracking occupied cells
    private PipeGenerator generatorInstance; // runtime instance of the generator

    private void Start()
    {
        // Initialize the logical 3D grid that tracks occupancy
        grid = new SpawnGrid(settings.gridSize);

        // Create a PipeGenerator in the scene (as a child of this object)
        generatorInstance = Instantiate(generatorPrefab, Vector3.zero, Quaternion.identity, transform);

        // Inject configuration and grid so the generator can start producing pipes
        PipeRenderer visuals = new PipeRenderer(settings, generatorInstance.transform);
        generatorInstance.Initialize(settings, grid, visuals);
    }
}