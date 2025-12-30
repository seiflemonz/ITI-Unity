using UnityEngine;

public class SurveillanceCameraSwitcher : MonoBehaviour
{
    [Header("Surveillance Cameras")]
    public Camera[] cameras;

    [Header("Render Textures")]
    public RenderTexture[] cameraFeeds;

    [Header("Monitor Screen")]
    public Renderer monitorRenderer;

    private int currentIndex = 0;

    void Start()
    {
        ActivateCamera(0);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NextCamera();
        }
    }

    void NextCamera()
    {
        currentIndex++;
        if (currentIndex >= cameras.Length)
            currentIndex = 0;

        ActivateCamera(currentIndex);
    }

    void ActivateCamera(int index)
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        cameras[index].gameObject.SetActive(true);
        monitorRenderer.material.mainTexture = cameraFeeds[index];
    }
}
