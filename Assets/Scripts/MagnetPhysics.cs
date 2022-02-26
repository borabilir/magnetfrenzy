using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagnetPhysics : MonoBehaviour
{
    public float magnetForce = 200;

    public List<Rigidbody> caughtRigidbodies = new List<Rigidbody>();

    public int capacity = 64;
    public bool drop = false;
    public TextMeshPro health;

    private void Awake()
    {
        UpdateHealth();
    }
    void FixedUpdate()
    {
        for (int i = 0; i < caughtRigidbodies.Count; i++)
        {
            caughtRigidbodies[i].isKinematic = false;
            caughtRigidbodies[i].velocity = (transform.position - (caughtRigidbodies[i].transform.position + caughtRigidbodies[i].centerOfMass)) * magnetForce * Time.smoothDeltaTime;
            caughtRigidbodies[i].transform.parent = null;

            //print(caughtRigidbodies[i].velocity.magnitude);
            //if (caughtRigidbodies[i].velocity.magnitude < 7f)
            //{
            //    caughtRigidbodies[i].isKinematic=true;
            //    caughtRigidbodies[i].velocity = Vector3.zero;
            //    caughtRigidbodies.RemoveAt(i);
            //}
        }
        //if (drop)
        //{
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 90, 0), .1f * Time.deltaTime);
        //    GetComponent<Rigidbody>().velocity = Vector3.left * 10000f;
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        if (capacity == 0)
            return;

        if (other.transform.CompareTag("DiffusibleCube"))
        {
            for (int i = 0; i < other.transform.childCount; i++)
            {
                if (other.transform.GetChild(i).GetComponent<Rigidbody>())
                {
                    Rigidbody r = other.transform.GetChild(i).GetComponent<Rigidbody>();

                    if (!caughtRigidbodies.Contains(r))
                    {
                        //Add Rigidbody
                        caughtRigidbodies.Add(r);
                        capacity--;
                        UpdateHealth();
                    }
                }
            }
            StartCoroutine(DestroyBalls());
        }
    }

    //void OnTriggerExit(Collider other)
    //{
    //    if (other.GetComponent<Rigidbody>())
    //    {
    //        Rigidbody r = other.GetComponent<Rigidbody>();

    //        if (caughtRigidbodies.Contains(r))
    //        {
    //            //Remove Rigidbody
    //            caughtRigidbodies.Remove(r);
    //        }
    //    }
    //}

    void UpdateHealth()
    {
        health.text = capacity.ToString();
    }

    IEnumerator DestroyBalls()
    {
        drop = true;
        yield return new WaitForSeconds(5f);
        for (int i = 0; i< caughtRigidbodies.Count; i++)
        {
            Destroy(caughtRigidbodies[i].transform.gameObject);
            caughtRigidbodies[i] = null;
        }
        Destroy(gameObject);
    }



}
