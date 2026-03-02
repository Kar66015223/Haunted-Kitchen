using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private Canvas canvas;


    private void Awake()
    {
        canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        cam = Camera.main;
        canvas.worldCamera = cam;
    }

    private void LateUpdate()
    {
        if (cam == null) return;

        transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
}
