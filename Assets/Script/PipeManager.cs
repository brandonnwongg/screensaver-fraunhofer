using UnityEngine;


public class PipeManager : MonoBehaviour
{
    [SerializeField] private PipeSettings settings;
    [SerializeField] private PipeGenerator generatorPrefab;

    private CellGrid3D grid;
    private PipeGenerator generatorInstance;

    private void Start()
    {
        //initialize the grid
        grid = new CellGrid3D(settings.gridSize);

        // Spawn a generator to visualize pipes
        generatorInstance = Instantiate(generatorPrefab, Vector3.zero, Quaternion.identity, transform);

        // Inject dependencies
        generatorInstance.Initialize(settings, grid);
    }
}