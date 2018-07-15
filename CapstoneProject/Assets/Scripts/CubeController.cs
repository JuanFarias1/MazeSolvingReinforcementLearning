 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.UI;
using UnityEngine.SceneManagement;

 public class CubeController : MonoBehaviour
{
    private GameObject variableForPrefab;
    public GameObject mainCube;
    public TextWriteTest tableMessenger;
    private int numITERATIONS = 2;
    public Text startText;
    public Text completeText;
    public Camera playerCam;
    public Camera aiCam;
    private bool aiEnabled = true;             // set by menu
    public bool playerEnabled = false;       // set by menu
    public GameObject cube;
    public GameObject playerCube;
    public Slider energyBarr;
    private energyScript energyScriptRef;
    private static numIterationsScript numIterationsScriptRef; 
    public bool movementSwitch = false;
    public bool[] aiMoveArray = new bool[4] { false, false, false, false };
    char[] charactersArray = new char[4] { 'a', 'd', 'w', 's'};
    private List<Vector3> positions;
    private List<string> directionsTaken;
    private List<float> distToGoal;
    private List<bool> canMoveLeft;
    private List<bool> canMoveRight;
    private List<bool> canMoveForward;
    private List<bool> canMoveBack;
    public float ALPHA = 0.5f;
    public float GAMMA = 0.5f;
    public List<char> probabilityBank;
    private List<int> rewardGiven; 
    private DataCollect dataObj;
    private QTable lookupTable;
    public Vector3 toPosition = new Vector3(0, 0, 0);
    private bool[] findStates = { false, false, false, false };
    public GameObject target;           // this should be set dynamically as a function of the maze size
    private int counter = 0;
    // set arrays of each state 
    private static bool[] s1 = { true, false, false, false };
    private static bool[] s2 = { false, true, false, false};
    private static bool[] s3 = { false ,false, true, false};
    private static bool[] s4 = { false, false ,false, true};
    private static bool[] s5 = { true, false, true, false};
    private static bool[] s6 = { true, true, false, false};
    private static bool[] s7 = { true, false, false, true};
    private static bool[] s8 = { false, true, true, false};
    private static bool[] s9 = { false, false, true, true};
    private static bool[] s10 = { false, true, false, true};
    private static bool[] s11 = { true, true, true, false};
    private static bool[] s12 = { true, false, true, true};
    private static bool[] s13 = { false, true, true, true};
    private static bool[] s14 = { true, true, false, true};
    private static bool[] s15 = { true, true, true, true};
    private bool[][] states = new bool[][] { s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12, s13, s14, s15 };
    public int R;
    public int lastReward;
    public int counterOne;
    char fChooseActionReturn;
    int fStateReturn = 0;

    void Awake() 
    {

    }

     void Start()
    {
        energyScriptRef = energyBarr.GetComponent<energyScript>();
        lookupTable = GetComponent<QTable>();
        lookupTable.qTableInit();
        tableMessenger = GetComponent<TextWriteTest>();
        // keep reference to numIterations alive when scene reload
        variableForPrefab = Resources.Load("prefabs/MainCube", typeof(GameObject)) as GameObject;
        numIterationsScriptRef = variableForPrefab.GetComponent<numIterationsScript>();
	// run the read function to update Qtable from local text files
        read();
        numIterationsScriptRef.numIterations += 1;    // increment numIterations
        completeText.text = "";
        StartCoroutine(runStartText());

        // if aiEnabled = false hide the aiCube and render in the playerCube
        if (aiEnabled == false) {
            cube.SetActive(false);
            CubeController scriptRef = GetComponent<CubeController>();
            scriptRef.enabled = false;  // disable the mlScript so computations dont use resources in background
        }
        if (playerEnabled == false){
            //playerCube.SetActive(false);
            GameObject.Destroy(playerCube);
            //CubeController scriptRefTwo = GetComponent<playerController>();
            //scriptRefTwo.enabled = false;
        }
        else {
            aiCam.enabled = false;
        }
        dataObj = GetComponent<DataCollect>();
        positions = dataObj.getPositions();
        directionsTaken = dataObj.getdDirectionTaken();
        distToGoal = dataObj.getDistToGoal();
        canMoveLeft = dataObj.getCanMoveLeft();
        canMoveRight = dataObj.getCanMoveRight();
        canMoveBack = dataObj.getCanMoveBack();
        canMoveForward = dataObj.getCanMoveForward();
        rewardGiven = dataObj.getRewardGiven();
    }

    void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 back = transform.TransformDirection(Vector3.back);
        Vector3 left = transform.TransformDirection(Vector3.left);
        Vector3 right = transform.TransformDirection(Vector3.right);

        //bool aiMoveL = false;
        //bool aiMoveR = false;
        //bool aiMoveF = false;
        //bool aiMoveB = false;
        if (Physics.Raycast(transform.position, left, 2) == false)
        {
            if (toPosition != currentPosition)
            {
                // update can move left
                canMoveLeft.Add(true);
            }
        }
            else
            {
                if (toPosition != currentPosition)
                {
                    // update can move left
                    canMoveLeft.Add(false);
                }
            }
        if (Physics.Raycast(transform.position, right, 2) == false)
        {
            if (toPosition != currentPosition)
            {
                // update can move right
                canMoveRight.Add(true);
            }
        }
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move right
                canMoveRight.Add(false);
            }
        }
        if (Physics.Raycast(transform.position, fwd, 2) == false)
        {

            if (toPosition != currentPosition)
            {
                // update can move forward
                canMoveForward.Add(true);
            }
        }
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move forward
                canMoveForward.Add(false);
            }
        }
        if (Physics.Raycast(transform.position, back, 2) == false)
        {
            if (toPosition != currentPosition)
            {
                // update can move back
                canMoveBack.Add(true);
            }
        }
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move back
                canMoveBack.Add(false);
            }
        }
        if (Physics.Raycast(transform.position, left, 2) == false && movementSwitch == false)
        {
            if (Input.GetKeyDown(KeyCode.A) || aiMoveArray[0] == true)
            {
                energyScriptRef.didMove();
                movementSwitch = true;
                transform.position += 3 * Vector3.left;
                positions.Add(transform.position);
                directionsTaken.Add("Left");
                distToGoal.Add(distanceFunction());
                aiMoveArray[0] = false;
                fStateReturn = findState();
		
                counter += 1;
                updateReward(fChooseActionReturn, fStateReturn);
                endGameIfAtEnd();
            }
        }
        if (Physics.Raycast(transform.position, right, 2) == false && movementSwitch == false)
        {
            if (Input.GetKeyDown(KeyCode.D) || aiMoveArray[1] == true)
            {
                energyScriptRef.didMove();
                movementSwitch = true;
                transform.position += 3 * Vector3.right;
                positions.Add(transform.position);
                directionsTaken.Add("Right");
                distToGoal.Add(distanceFunction());
                aiMoveArray[1] = false;
                fStateReturn = findState();

                counter += 1;
                updateReward(fChooseActionReturn, fStateReturn);
                endGameIfAtEnd();
            }
        }
        if (Physics.Raycast(transform.position, fwd, 2) == false && movementSwitch == false)
        {
            if (Input.GetKeyDown(KeyCode.W) || aiMoveArray[2] == true)
            {
                energyScriptRef.didMove();
                movementSwitch = true;
                transform.position += 3 * Vector3.forward;
                positions.Add(transform.position);
                directionsTaken.Add("Forward");
                distToGoal.Add(distanceFunction());
                aiMoveArray[2] = false;
                fStateReturn = findState();

                counter += 1;
                updateReward(fChooseActionReturn, fStateReturn);
                endGameIfAtEnd();
            }
        }
        if (Physics.Raycast(transform.position, back, 2) == false && movementSwitch == false)
        {
            if (Input.GetKeyDown(KeyCode.S) || aiMoveArray[3] == true)
            {
                energyScriptRef.didMove();
                movementSwitch = true;
                transform.position += 3 * Vector3.back;
                positions.Add(transform.position);
                directionsTaken.Add("Back");
                distToGoal.Add(distanceFunction());
                aiMoveArray[3] = false;
                fStateReturn = findState();

                counter += 1;
                updateReward(fChooseActionReturn, fStateReturn);
                endGameIfAtEnd();
            }
        }
        toPosition = currentPosition;
        fChooseActionReturn = chooseAction(
            lookupTable.qTable[fStateReturn, 0], lookupTable.qTable[fStateReturn, 1], lookupTable.qTable[fStateReturn, 2],
            lookupTable.qTable[fStateReturn, 3]/*, fStateReturn*/
        );
        for (int g = 0; g < charactersArray.Length; g++)
        {
            if (fChooseActionReturn == charactersArray[g])
            {
                aiMoveArray[g] = true;
            }
        }
        movementSwitch = false;
    }

    public float distanceFunction() {
        var zdist = target.transform.position.z - transform.position.z;
        var xdist = target.transform.position.x - transform.position.x;
        var euclideanDistance = Mathf.Sqrt(zdist * zdist + xdist * xdist);

        return euclideanDistance;
    }

    public int findState() {

        int returnIndex = -1; 
        string strState1;
        string strState2;
        string strState3;
        string strState4;
        string strState5;
        string strState6;
        string strState7;
        string strState8;
        string strStateConcat;
        string strCompare;
        bool[] array;
        findStates[0] = canMoveLeft[counter];
        findStates[1] = canMoveRight[counter];
        findStates[2] = canMoveForward[counter];
        findStates[3] = canMoveBack[counter];

        strState1 = findStates[0].ToString();
        strState2 = findStates[1].ToString();
        strState3 = findStates[2].ToString();
        strState4 = findStates[3].ToString();

        strStateConcat = strState1 + strState2 + strState3 + strState4;
        for (int i = 0; i < 15; i++) {
            array = states[i];
                strState5 = array[0].ToString();
                strState6 = array[1].ToString();
                strState7 = array[2].ToString();
                strState8 = array[3].ToString();
                strCompare = strState5 + strState6 + strState7 + strState8;
           
                if (strStateConcat == strCompare) {

                returnIndex = i;

                break;
                }
        }
        return (returnIndex);      
    }

    public char chooseAction(int w, int a, int s, int d/*, int index*/)
    {
        for (int i = 0; i < w; i++)
        {
            probabilityBank.Add('w');
        }

        for (int i = 0; i < a; i++)
        {
            probabilityBank.Add('a');
        }

        for (int i = 0; i < s; i++)
        {
            probabilityBank.Add('s');
        }

        for (int i = 0; i < d; i++)
        {
            probabilityBank.Add('d');
        }

        int listCount = probabilityBank.Count;

        int chosenIndex = Random.Range(0, listCount - 1);

        return (probabilityBank[chosenIndex]);
    }

    public void updateReward(char actionTaken, int returnIndex) {

        string[] directionArray = new string[4] { "Left", "Right", "Forward", "Back" };
        int greatestValue = 0;
        float rewardFunction;
        int integerRewardFunction;
        float retDist;

        for (int j = 0; j < directionArray.Length; j++) {
            if (directionsTaken[counter - 1] == directionArray[j]) {

                lastReward = lookupTable.qTable[returnIndex, j];

                counterOne = j;
            }
        }
        for (int h = 0; h < 4; h++) {
            if (lookupTable.qTable[returnIndex, h] > greatestValue) {
                
                greatestValue = lookupTable.qTable[returnIndex, h];
            }
        }
        retDist = distanceFunction();
	
        if (retDist <= 2) {
             R = 100;
        }
        // Gamma as a function of energy
        GAMMA = energyScriptRef.energyPenalty();
        rewardFunction = lastReward + ALPHA * (R + (GAMMA * greatestValue) - lastReward);
        integerRewardFunction = (int) rewardFunction;

        // add only even numbers to Qtable for reduced search space nomralization
        if (integerRewardFunction % 2 != 0) {
            integerRewardFunction++;
        }
        // update the Q - table
        lookupTable.qTable[returnIndex, counterOne] += integerRewardFunction;

        // reduce search space 
        if (lookupTable.qTable[returnIndex, counterOne] > 50) {
            for (int i = 0; i < 4; i++) {
                if (lookupTable.qTable[returnIndex, i] != 2)
                {
                    lookupTable.qTable[returnIndex, i] = lookupTable.qTable[returnIndex, i] / 2;
                }
                if (lookupTable.qTable[returnIndex, i] % 2 != 0) {
                    lookupTable.qTable[returnIndex, i]++;
                }
            }
        }
    }
    
    IEnumerator runStartText() {
        startText.text = "Start!";
                
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;

        startText.text = "";
    }

    public void endGameIfAtEnd()
    {
        if (distToGoal[counter - 1] <= 2 && numIterationsScriptRef.numIterations == numITERATIONS)
        {
            write();

            completeText.text = "The AI has completed its simulations!";
            numIterationsScriptRef.numIterations = 0;
            StartCoroutine(delayFunction());
        }
        else if (distToGoal[counter - 1] <= 2 && numIterationsScriptRef.numIterations < numITERATIONS){
            write();

            GameObject.DestroyImmediate(cube);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

     public IEnumerator delayFunction()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1;
        SceneManager.LoadScene("WorkingMainMenu");
    }
    
	private void OnApplicationQuit()
	{
        numIterationsScriptRef.numIterations = 0;
	}

    private void read() {
        if (numIterationsScriptRef.numIterations == 0) {
            // do nothing
        }
        else {
            tableMessenger.importQtableText(lookupTable);
        }
    }

    private void write() {
        tableMessenger.exportQtableToText(lookupTable);
    }
}
