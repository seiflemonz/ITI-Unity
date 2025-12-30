using UnityEngine;

[ExecuteAlways]
public class Fractal : MonoBehaviour
{
    [Header("Fractal Settings")]
    [Range(1, 6)]
    public int iterations = 3;

    [Header("Shape Options")]
    public bool useCube = true;
    public bool useSphere = true;

    [Header("Prefabs")]
    public GameObject cubePrefab;
    public GameObject spherePrefab;

    private int lastIterations = 0;

    void Update()
    {
        if (iterations != lastIterations)
        {
            lastIterations = iterations;
            GenerateFractal();
        }
    }

    void GenerateFractal()
    {
        ClearChildren(transform);
        CreateFractal(transform.position, Vector3.one, iterations, transform);
    }

    void ClearChildren(Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(parent.GetChild(i).gameObject);
        }
    }

    void CreateFractal(Vector3 position, Vector3 scale, int depth, Transform parent)
    {
        if (depth <= 0) return;

        // Choose shape based on booleans
        GameObject obj = null;

        if (useCube && useSphere)
        {
            // Both enabled, pick randomly
            if ((iterations-depth) % 2 == 0)
            {
                obj = CreateCube(position);
            }
            else
            {
                obj = CreateSphere(position);
            }
        }
        else if (useCube)
        {
            obj = CreateCube(position);
        }
        else if (useSphere)
        {
            obj = CreateSphere(position);
        }
        else
        {
            // None enabled, fallback to cube
            obj = CreateCube(position);
        }

        obj.transform.localScale = scale;
        obj.transform.SetParent(parent, true);

        // Randomize color
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = rend.sharedMaterial != null
                ? new Material(rend.sharedMaterial)
                : new Material(Shader.Find("Standard"));
            mat.color = Random.ColorHSV();
            rend.material = mat;
        }

        // Recursive positions
        float newScale = scale.x * 0.5f;
        Vector3[] offsets = {
            new Vector3(newScale, 0, 0),
            new Vector3(-newScale, 0, 0),
            new Vector3(0, newScale, 0),
            new Vector3(0, -newScale, 0),
            new Vector3(0, 0, newScale),
            new Vector3(0, 0, -newScale)
        };

        foreach (var offset in offsets)
        {
            CreateFractal(position + offset, Vector3.one * newScale, depth - 1, obj.transform);
        }
    }

    GameObject CreateCube(Vector3 position)
    {
        if (cubePrefab != null)
            return Instantiate(cubePrefab, position, Quaternion.identity);
        else
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            return cube;
        }
    }

    GameObject CreateSphere(Vector3 position)
    {
        if (spherePrefab != null)
            return Instantiate(spherePrefab, position, Quaternion.identity);
        else
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = position;
            return sphere;
        }
    }
}
