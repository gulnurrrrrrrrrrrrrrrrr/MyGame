using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFit : MonoBehaviour
{
    public float targetWidth = 20f;   // сколько единиц мира должно быть видно по ширине
    public float minHeight = 10f;     // сколько минимум по высоте

    Camera cam;

    void Awake() { cam = GetComponent<Camera>(); }

    void LateUpdate()
    {
        float sizeByWidth = targetWidth / (2f * cam.aspect);
        cam.orthographicSize = Mathf.Max(minHeight / 2f, sizeByWidth);
    }
}