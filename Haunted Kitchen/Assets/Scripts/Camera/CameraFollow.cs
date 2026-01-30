using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 8, -6);
    [SerializeField] private float followSpeed = 10f;
    [SerializeField] private bool lookAtTarget = true;

    private void Start()
    {
        target = FindAnyObjectByType<PlayerController>().gameObject.transform;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        if (lookAtTarget)
        {
            transform.LookAt(target);
        }
    }

}
