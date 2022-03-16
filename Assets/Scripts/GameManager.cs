using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Stack collected = new Stack();
    public static GameManager instance;
    public GameObject homeScreen;

    public bool isPlaying = false;
    public GameObject progressBar;

    public Vector3 startPoint, currentPoint;
    public Transform finishPoint;
    private float completeRatio;
    public const float progressCoefficient = 34.1f;
    public int currentLevel=1;

    public Text currentLevelText, nextLevelText;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // currentLevelText.text = currentLevel.ToString();
        // nextLevelText.text = (currentLevel + 1).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            StartCoroutine(HomeScreenToPlay());
        }

        if (isPlaying)
        {
            if (collected.Count > 0)
            {
                var headTransform = (Transform)collected.Peek();
                currentPoint = headTransform.position;

                completeRatio = (currentPoint.z - startPoint.z) / (finishPoint.position.z - startPoint.z);
                completeRatio = Mathf.Clamp01(completeRatio);
                progressBar.GetComponent<Image>().fillAmount = completeRatio;
            }
        }
    }

    IEnumerator HomeScreenToPlay()
    {
        homeScreen.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(.5f);
        isPlaying = true;
    }

}
