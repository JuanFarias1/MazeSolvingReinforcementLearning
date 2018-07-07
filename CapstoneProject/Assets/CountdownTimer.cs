using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public Text timerS;
    float time = 5f;
    public string loadedlevel;



    // Use this for initialization
    void Start()
    {
        timerS = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        timerS.text = time.ToString("f0");
        if (time <= 1)
        {
            SceneManager.LoadScene(01);
        }
    }



}