using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    public float smoothSpeed;
    public static CameraController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;

        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, offset + target.position, smoothSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, transform.position.y, newPos.z);
        }
    }
}
