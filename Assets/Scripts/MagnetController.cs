using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public bool isLerp = false;
    public bool lerpFromRight = false;
    public float rotationAngle;
    public Transform parentNode;
    public bool test = false;
    public GameObject diffusableCubePrefab;
    private Color ballColor;


    // Start is called before the first frame update
    void Awake()
    {
        float r = Random.Range(0.0f, 1.0f);
        float g = Random.Range(0.0f, 1.0f);
        float b = Random.Range(0.0f, 1.0f);
        ballColor = new Color(r, g, b);
        GetComponent<MeshRenderer>().material.color = ballColor;
    }

    void Start()
    {
        if (tag == "Head")
        {
            GameManager.instance.collected.Push(transform);
            GameManager.instance.startPoint = transform.position;
        }
    }

    void Update()
    {
        if (isLerp)
        {
            if (lerpFromRight)
            {
                transform.parent.position = new Vector3(parentNode.position.x, parentNode.position.y, parentNode.position.z + 1f);
                transform.parent.rotation = Quaternion.Lerp(transform.parent.rotation, Quaternion.Euler(0, rotationAngle, 0), 50f * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(parentNode.position.x + .75f, parentNode.position.y, parentNode.position.z + 1f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 50f * Time.deltaTime);
            }
            if (transform.rotation == Quaternion.Euler(0, rotationAngle, 0))
            {
                isLerp = false;
                transform.rotation = Quaternion.identity;
                AttachNewNode();
            }
        }
        if (parentNode != null && !isLerp)
        {
            transform.position = new Vector3(parentNode.position.x, parentNode.position.y, parentNode.position.z + 1f);
        }
    }

    public void AttachNewNode()
    {
        transform.position = new Vector3(parentNode.position.x, parentNode.position.y, parentNode.position.z + 1f);
        GameObject container = transform.parent.parent.gameObject;
        transform.parent = parentNode.parent;
        Destroy(container);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (transform.CompareTag("Head") && other.CompareTag("MagnetCube"))
        {
            other.transform.tag = "Head";
            transform.tag = "Untagged";
            other.GetComponent<BoxCollider>().size = Vector3.one;
            GameManager.instance.collected.Push(other.transform);

            var relativeLocationXAxis = transform.InverseTransformPoint(other.transform.position).x;

            other.transform.GetComponent<MagnetController>().parentNode = transform;

            if (relativeLocationXAxis >= 0.75)
            {
                //other.transform.position = new Vector3(transform.position.x + .75f, transform.position.y, transform.position.z + 1f);
                other.transform.GetComponent<MagnetController>().rotationAngle = -90;
                other.transform.GetComponent<MagnetController>().isLerp = true;
                AudioManager.instance.PlaySound(CollisionType.EdgeCollision);
            }
            else if (relativeLocationXAxis <= -0.75)
            {
                //other.transform.position = new Vector3(transform.position.x - .75f, transform.position.y, transform.position.z + 1f);
                other.transform.GetComponent<MagnetController>().rotationAngle = 90;
                other.transform.GetComponent<MagnetController>().lerpFromRight = true;
                other.transform.GetComponent<MagnetController>().isLerp = true;
                AudioManager.instance.PlaySound(CollisionType.EdgeCollision);

            }
            else
            {
                other.transform.GetComponent<MagnetController>().AttachNewNode();
                AudioManager.instance.PlaySound(CollisionType.DirectCollision);
            }

            CameraController.instance.target = other.transform;
        }
        else if (transform.CompareTag("Head") && other.CompareTag("MagneticField"))
        {
            if (other.transform.parent.transform.GetComponent<ObstacleController>().capacity > 0)
            {
                gameObject.SetActive(false);
                var newCubePrefab = Instantiate(diffusableCubePrefab, transform);
                for (int i = 0; i < newCubePrefab.transform.childCount; i++)
                {
                    newCubePrefab.transform.GetChild(i).transform.GetComponent<MeshRenderer>().material.color = ballColor;
                }
                newCubePrefab.transform.parent = null;
                GameManager.instance.collected.Pop();
                if (GameManager.instance.collected.Count > 0)
                {
                    var newHead = (Transform)GameManager.instance.collected.Peek();
                    CameraController.instance.target = newHead;
                    newHead.tag = "Head";
                    newHead.GetComponent<MagnetController>().parentNode = null;
                }
                else
                {
                    //GameOver
                    CameraController.instance.target = null;
                }

                Destroy(gameObject);
            }
        }
    }
}
