using UnityEngine;


/// <summary>
/// Visualizes a 3D grid of cells using Gizmos.
/// A cell is the smallest unit of space in a grid 
/// Only one object can occupy a cell at the time
/// Used for visual debugging
/// </summary>
public class CellGridVisualizer : MonoBehaviour
{
    [Header("Grid Dimensions")]
    public Vector3Int gridSize = new Vector3Int(20,20,20);
    public float spacing = 1.0f;

    [Header("Appearance")]
    public Color lineColor = new Color(1f, 1f, 1f, 0.25f);
    public bool drawX = true; // lines parallel to X (at every Y/Z boundary)
    public bool drawY = true; // lines parallel to Y (at every X/Z boundary)
    public bool drawZ = true; // lines parallel to Z (at every X/Y boundary)
    public int maxLines = 20000; // safety for huge grids

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color  = lineColor;

        float minX = -gridSize.x * spacing * 0.5f;
        float minY = -gridSize.y * spacing * 0.5f;
        float minZ = -gridSize.z * spacing * 0.5f;
        float maxX = -minX, maxY = -minY, maxZ = -minZ;

        int lines = 0;

        // Draws X-parallel lines
        if (drawX)
        {
            for (int y = 0; y <= gridSize.y; y++)
            {
                float yy = minY + y * spacing;
                for (int z = 0; z <= gridSize.z; z++)
                {
                    float zz = minZ + z * spacing;
                    Gizmos.DrawLine(new Vector3(minX, yy, zz), new Vector3(maxX, yy, zz));
                    if (++lines > maxLines) return;
                }
            }
        }

        // Draws Y-parallel lines
        if (drawY)
        {
            for (int x = 0; x <= gridSize.x; x++)
            {
                float xx = minX + x * spacing;
                for (int z = 0; z <= gridSize.z; z++)
                {
                    float zz = minZ + z * spacing;
                    Gizmos.DrawLine(new Vector3(xx, minY, zz), new Vector3(xx, maxY, zz));
                    if (++lines > maxLines) return;
                }
            }
        }

        // Draws Z-parallel lines
        if (drawZ)
        {
            for (int x = 0; x <= gridSize.x; x++)
            {
                float xx = minX + x * spacing;
                for (int y = 0; y <= gridSize.y; y++)
                {
                    float yy = minY + y * spacing;
                    Gizmos.DrawLine(new Vector3(xx, yy, minZ), new Vector3(xx, yy, maxZ));
                    if (++lines > maxLines) return;
                }
            }
        }
    }
}
