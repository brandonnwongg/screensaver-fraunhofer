using UnityEngine;

[CreateAssetMenu(fileName = "PipeSettings", menuName = "Screensaver Settings")]
public class PipeSettings : ScriptableObject
{
    public Vector3Int gridSize = new Vector3Int(10, 10, 10);
    public float spacing = 1f;

    // max amount of pipes allowed in a round before it clears the grid
    public int maxPipes = 10;
    // max amount of segments allowed per pipe until a new pipe starts
    public int maxSegmentsPerPipe = 30;
    // time delay for each segment to grow
    public float growthDelay = 0.2f;

    // start and end  of pipe
    public GameObject spherePrefab;
    // body of pipe
    public GameObject cylinderPrefab;
}
