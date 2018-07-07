using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class energyScript : MonoBehaviour {

    public Slider energyBar;
    public GameObject cell;
    public GameObject cube;
    private CubeController cubeCtrlScriptRef;
    private int bestCase;
    private int worstCase;
    public float energyHitFactor;
    private int mazeWidth=-1;
    private int mazeHeight=-1;
    private float hitFactDenom;
    public Text loseGame;

    private GameObject varForPrefab;
    private numIterationsScript numIterationsScriptRef;

	// Use this for initialization
	void Start () {
        loseGame.text = "";

        cubeCtrlScriptRef = cube.GetComponent<CubeController>();

        varForPrefab = Resources.Load("prefabs/MainCube", typeof(GameObject)) as GameObject;
        numIterationsScriptRef = varForPrefab.GetComponent<numIterationsScript>();


        while ((mazeWidth = cell.GetComponent<MazeGenerator>()._width) == -1 || (mazeHeight = cell.GetComponent<MazeGenerator>()._height) == -1) {

        }
        mazeWidth = cell.GetComponent<MazeGenerator>()._width;
        mazeHeight = cell.GetComponent<MazeGenerator>()._height;
        Debug.Log(mazeWidth);
        Debug.Log(mazeHeight);
        // for n * n mazes (n(n+2) - 1) / 2
        hitFactDenom = energyPenaltyDivFactor(mazeWidth, mazeHeight);

        energyHitFactor = (mazeWidth * (mazeWidth + 2) - 1) / hitFactDenom; // the denomitaor here scales out the large penalty do to errors ai will make
        // different function for non square mazes

        Debug.Log(hitFactDenom);
        Debug.Log(energyHitFactor);
	}
	
	// Update is called once per frame
	void Update () {

        //didMove();

	}

    public void didMove() {


        if (energyBar.value >= 100) {
            
            loseGame.text = "Sorry, the AI is feeling slow :(";
            numIterationsScriptRef.numIterations = 0;
            StartCoroutine(cubeCtrlScriptRef.delayFunction());
        }

        //float takeAway = energyBar.value - energyHitFactor;
        energyBar.value += energyHitFactor;



    }

    private float energyPenaltyDivFactor(int w, int h) {

        int cellNo = w * h;
        float denom;

        if (cellNo > 0 & cellNo < 49)
        {
            denom = 500f;
        }
        else if (cellNo >= 49 & cellNo < 81)
        {
            denom = 800f;
        }
        else if (cellNo >= 81 & cellNo < 112)
        {
            denom = 2000f;
        }
        else if (cellNo >= 112 & cellNo < 143)
        {
            denom = 3000f;
        }
        else if (cellNo >= 143 & cellNo < 230)
        {
            denom = 3500f;
        }
        else {
            denom = 4000f;
        }

        return (denom);
    }

    public float energyPenalty() {

        float retValue = 1 - energyBar.value / energyBar.maxValue;


        return retValue;
    }

}
