using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class distanceScript : MonoBehaviour
{

    private Text distanceText;
    private GameObject target;
    public GameObject cube;
    private float distance;

    private CubeController cbContrRef;


    // Use this for initialization
    void Start()
    {
        Debug.Log("wha da fuh0");
        target = GameObject.Find("target");

        Debug.Log("wha da fuh1");
        cbContrRef = cube.GetComponent<CubeController>();

        if (cbContrRef.playerEnabled == true) {
            Debug.Log("wha da fuh2");
            cube = GameObject.Find("player");
        }
        else {
            Debug.Log("wha da fuh3");
            cube = GameObject.Find("aiCube");
        }

        Debug.Log("wha da fuh4");
        distanceText = GetComponent<Text>() as Text;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(cube.transform.position, target.transform.position);
        distanceText.text = distance.ToString("Distance to Target: #.00");
    }

}
