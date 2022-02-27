using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        var color = Color.yellow;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
