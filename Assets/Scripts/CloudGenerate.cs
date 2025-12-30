using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloudGenerate : MonoBehaviour
{
    [SerializeField]
    private Sprite cloud;


    public List<GameObject> clouds;

    [SerializeField]
    private float moveSpeed=10.0f;
    private float frequency = 9.0f;

    public float minX;
    public float minY;
    public float maxY;

    public float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > frequency)
        {
            timer = 0;
            GameObject newCloud = new GameObject("New Cloud");
            newCloud.transform.position = new Vector3(minX, Random.Range(minY, maxY), 0);
            var sr = newCloud.AddComponent<SpriteRenderer>();
            sr.sprite = cloud;
            sr.renderingLayerMask = 1 << 14;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = 1;
            newCloud.transform.localScale = Vector3.one*3.0f;
            clouds.Add(newCloud);
        }

        for (int i = clouds.Count - 1; i >= 0; i--)
        {
            var cloudElement = clouds[i];
            cloudElement.transform.position += Vector3.right * moveSpeed * Time.deltaTime;

            // Destroy clouds out of bounds
            if (Mathf.Abs(cloudElement.transform.position.x) > minX)
            {
                Destroy(cloudElement);
                clouds.RemoveAt(i);
            }
        }
    }
}
