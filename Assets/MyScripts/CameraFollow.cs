using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z = target.position.z + offset.z;
            newPosition.x = target.position.x;
            transform.position = newPosition;
        }
    }
}
