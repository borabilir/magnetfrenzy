using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed = 15;

    public float minPositionBound, maxPositionBound;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPlaying)
            return;

        transform.position += Vector3.forward * moveSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPositionBound, maxPositionBound), transform.position.y, transform.position.z);
    }
}
