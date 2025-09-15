using UnityEngine;

public class PipeMaterial
{
    private static Shader litShader = Shader.Find("Universal Render Pipeline/Lit");

    public static Material CreateRandomMaterial()
    {
        var mat = new Material(litShader);
        mat.color = new Color(Random.value, Random.value, Random.value);
        return mat;
    }
}