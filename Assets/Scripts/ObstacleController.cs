using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float magnetForce = 200;

    public List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();

    [HideInInspector]
    public int capacity = 64;
    public int capacityPerCube = 1;
    public TextMeshPro health;
    private bool moveBalls = true;

    public Transform[] poles;

    private void Awake()
    {
        capacity = capacityPerCube * 64;
        //UpdateHealth();
    }
    void FixedUpdate()
    {
        if (!moveBalls)
            return;

        foreach (var body in caughtRigidbodies.Where(x => !x.isKinematic))
        {
            var closestPole = FindClosestPole(body.transform);
            body.velocity = (closestPole - (body.transform.position + body.centerOfMass)) * magnetForce * Time.smoothDeltaTime;
            body.transform.parent = null;

            //if ((closestPole - body.position).magnitude < 1f)
            //{
            //    body.isKinematic = true;
            //    body.detectCollisions = false;
            //    body.velocity = Vector3.zero;
            //}
        }

        //StartCoroutine(DestroyBalls());
    }

    Vector3 FindClosestPole(Transform ball)
    {
        float minDistance = 10000f;
        Transform minDistancePole = null;
        foreach (Transform pole in poles)
        {
            float distance = (pole.position - ball.position).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                minDistancePole = pole;
            }
        }

        return minDistancePole.position;
    }

    void OnTriggerEnter(Collider other)
    {
        //if (capacity == 0)
        //    return;

        if (other.transform.CompareTag("DiffusibleCube"))
        {
            other.transform.GetComponent<BoxCollider>().enabled = false;
            AudioManager.instance.PlaySound(CollisionType.ObstacleCollision);
            for (int i = 0; i < other.transform.childCount; i++)
            {
                if (other.transform.GetChild(i).GetComponent<Rigidbody>())
                {
                    Rigidbody r = other.transform.GetChild(i).GetComponent<Rigidbody>();

                    if (!caughtRigidbodies.Contains(r))
                    {
                        //Add Rigidbody
                        caughtRigidbodies.Add(r);
                        r.isKinematic = false;
                        capacity--;
                        //UpdateHealth();
                    }
                }
            }
            StartCoroutine(StopBalls());
        }
    }
    IEnumerator StopBalls()
    {
        yield return new WaitForSeconds(1f);
        foreach (var body in caughtRigidbodies.Where(x => !x.isKinematic))
        {
            body.isKinematic = true;
            body.detectCollisions = false;
            body.velocity = Vector3.zero;
        }
        moveBalls = false;
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < caughtRigidbodies.Count; i++)
        {
            Destroy(caughtRigidbodies[i].transform.gameObject);
            caughtRigidbodies[i] = null;
        }
        Destroy(gameObject);
    }

}
