 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;


 public class CubeController : MonoBehaviour
{
    public bool aiEnabled = true;
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


    public GameObject target;

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


     void Start()
    {
        dataObj = GetComponent<DataCollect>();
        lookupTable = GetComponent<QTable>();

        lookupTable.qTableInit();



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
       // Debug.Log("Left: " + canMoveLeft.Count + "Right: " + canMoveRight.Count + "Forward: " + canMoveForward.Count + "Back: " + canMoveBack.Count);

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
                // update can move lefy
                canMoveLeft.Add(true);
                // Debug.Log("true was added 0");

                //  toPosition = currentPosition;
            }
        }


            else
            {

                if (toPosition != currentPosition)
                {
                    // update can move lefy
                    canMoveLeft.Add(false);
                    // Debug.Log("false was added 0");

                    // toPosition = currentPosition;
                }
            }


        if (Physics.Raycast(transform.position, right, 2) == false)
        {

            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveRight.Add(true);
                //  Debug.Log("true was added 1");

                //  toPosition = currentPosition;
            }
        }

        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveRight.Add(false);
                // Debug.Log("false was added 1");

                // toPosition = currentPosition;
            }
        }

        if (Physics.Raycast(transform.position, fwd, 2) == false)
        {

            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveForward.Add(true);
                //  Debug.Log("true was added 2");

                // toPosition = currentPosition;
            }
        }

        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveForward.Add(false);
                // Debug.Log("false was added 2");

                // toPosition = currentPosition;
            }
        }


        if (Physics.Raycast(transform.position, back, 2) == false)
        {

            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveBack.Add(true);
                // Debug.Log("true was added 3");

                // toPosition = currentPosition;
            }
        }

        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveBack.Add(false);
                // Debug.Log("false was added 3");

                //toPosition = currentPosition;
            }
        }







        if (Physics.Raycast(transform.position, left, 2) == false && movementSwitch == false)
        {
            /*
            if (toPosition != currentPosition) {
            // update can move lefy
                canMoveLeft.Add(true);
               // Debug.Log("true was added 0");

              //  toPosition = currentPosition; 
            } */

            if (Input.GetKeyDown(KeyCode.A) || aiMoveArray[0] == true)
            {
                movementSwitch = true;

                //Debug.Log("DANGER ZONE 1");

                transform.position += 3 * Vector3.left;

                positions.Add(transform.position);
                directionsTaken.Add("Left");
                distToGoal.Add(distanceFunction());
                //Debug.Log(directionsTaken[counter]);
                //Debug.Log(distToGoal[counter]);


                aiMoveArray[0] = false;

               // Debug.Log("Number of moves: " + counter);
                fStateReturn = findState();

                counter += 1;
                //Debug.Log("Number of moves: " + counter);

                updateReward(fChooseActionReturn, fStateReturn);
            }
        }

        /*else {
            
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveLeft.Add(false);
               // Debug.Log("false was added 0");

               // toPosition = currentPosition;
            }
        } */

        if (Physics.Raycast(transform.position, right, 2) == false && movementSwitch == false)
        {
            /*
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveRight.Add(true);
              //  Debug.Log("true was added 1");

              //  toPosition = currentPosition;
            } */

            if (Input.GetKeyDown(KeyCode.D) || aiMoveArray[1] == true)
            {
                movementSwitch = true;

               // Debug.Log("DANGER ZONE 2");

                transform.position += 3 * Vector3.right;

                positions.Add(transform.position);
                directionsTaken.Add("Right");
                distToGoal.Add(distanceFunction());
                //Debug.Log(directionsTaken[counter]);
                //Debug.Log(distToGoal[counter]);



                aiMoveArray[1] = false;


               // Debug.Log("Number of moves: " + counter);
                fStateReturn = findState();

                counter += 1;

                //Debug.Log("Number of moves: " + counter);
                updateReward(fChooseActionReturn, fStateReturn);
            }
        }
        /*
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveRight.Add(false);
               // Debug.Log("false was added 1");

               // toPosition = currentPosition;
            }
        } */

        if (Physics.Raycast(transform.position, fwd, 2) == false && movementSwitch == false)
        {
            /*
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveForward.Add(true);
              //  Debug.Log("true was added 2");

               // toPosition = currentPosition;
            } */

            if (Input.GetKeyDown(KeyCode.W) || aiMoveArray[2] == true)
            {
                movementSwitch = true;

                //Debug.Log("DANGER ZONE 3");

                transform.position += 3 * Vector3.forward;

                positions.Add(transform.position);
                directionsTaken.Add("Forward");
                distToGoal.Add(distanceFunction());
                //Debug.Log(directionsTaken[counter]);
               // Debug.Log(distToGoal[counter]);

                aiMoveArray[2] = false;

              //  Debug.Log("Number of moves: " + counter);

                fStateReturn = findState();

                counter += 1;
                //Debug.Log("Number of moves: " + counter);
                updateReward(fChooseActionReturn, fStateReturn);
            }
        }
        /*
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveForward.Add(false);
               // Debug.Log("false was added 2");

               // toPosition = currentPosition;
            }
        }*/

        if (Physics.Raycast(transform.position, back, 2) == false && movementSwitch == false)
        {
            /*
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveBack.Add(true);
               // Debug.Log("true was added 3");

               // toPosition = currentPosition;
            }*/

            if (Input.GetKeyDown(KeyCode.S) || aiMoveArray[3] == true)
            {
                movementSwitch = true;

               // Debug.Log("DANGER ZONE 4");

                transform.position += 3 * Vector3.back;

                positions.Add(transform.position);
                directionsTaken.Add("Back");
                distToGoal.Add(distanceFunction());
                //Debug.Log(directionsTaken[counter]);
                //Debug.Log(distToGoal[counter]);

                aiMoveArray[3] = false;

               // Debug.Log("Number of moves: " + counter);
                fStateReturn = findState();


                counter += 1;

                //Debug.Log("Number of moves: " + counter);
                updateReward(fChooseActionReturn, fStateReturn);

            }
        }
        /*
        else
        {
            if (toPosition != currentPosition)
            {
                // update can move lefy
                canMoveBack.Add(false);
               // Debug.Log("false was added 3");

                //toPosition = currentPosition;
            }
        } */



        toPosition = currentPosition;

       // Debug.Log("Debug 1");
        //fStateReturn = findState(); -----------------------------------------> working somewhat if uncomment
        //Debug.Log("Debug 2");

        fChooseActionReturn = chooseAction(
            lookupTable.qTable[fStateReturn, 0], lookupTable.qTable[fStateReturn, 1], lookupTable.qTable[fStateReturn, 2],
            lookupTable.qTable[fStateReturn, 3]/*, fStateReturn*/



        );
        //Debug.Log("Debug 3");

        //updateReward(fChooseActionReturn, fStateReturn);

        //Debug.Log("Debug 4");
        for (int g = 0; g < charactersArray.Length; g++)
        {

            if (fChooseActionReturn == charactersArray[g])
            {

                /*    for (int i = 0; i < 4; i++) {
                        aiMoveArray[i] = false;
                    }
                */
                aiMoveArray[g] = true;
            }
        }



        //findState();

        movementSwitch = false;

    }


    private float distanceFunction() {


        var zdist = target.transform.position.z - transform.position.z;
        var xdist = target.transform.position.x - transform.position.x;
        var euclideanDistance = Mathf.Sqrt(zdist * zdist + xdist * xdist);

        return euclideanDistance;
    }


    public int findState() {
        
        // int x, y, z, w;

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
        //Debug.Log("wha da hell  " + counter);
        //int count = 0; --------> replace with a for loop to assign this
        findStates[0] = canMoveLeft[counter];
        findStates[1] = canMoveRight[counter];
        findStates[2] = canMoveForward[counter];
        findStates[3] = canMoveBack[counter];
        //Debug.Log("wha da hell 2");
        strState1 = findStates[0].ToString();
        strState2 = findStates[1].ToString();
        strState3 = findStates[2].ToString();
        strState4 = findStates[3].ToString();

        strStateConcat = strState1 + strState2 + strState3 + strState4;
        //strStateConcat = findStates.ToString();
        //Debug.Log("right here" + states[0][3].ToString());
        //Debug.Log(strStateConcat);
       // Debug.Log("wha da hell 3");
        for (int i = 0; i < 15; i++) {


            array = states[i];
            //Debug.Log("right here" + array[0].ToString() + array[1].ToString() + array[2].ToString() + array[3].ToString());
            //for (int j = 0; j < 4; j++) {

                strState5 = array[0].ToString();
                strState6 = array[1].ToString();
                strState7 = array[2].ToString();
                strState8 = array[3].ToString();
                strCompare = strState5 + strState6 + strState7 + strState8;
            //Debug.Log("the current state is" + strCompare);
                if (strStateConcat == strCompare) {
               // Debug.Log("the current state is" + strCompare);


                // added -------------------------------->
                returnIndex = i;

                break;
                }
           // }

        }

        // added -------------------------------->
        return (returnIndex);      
    }

    public char chooseAction(int w, int a, int s, int d/*, int index*/)
    {
        //Debug.Log("here 1");
        
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

        //Debug.Log("here 2");

        int listCount = probabilityBank.Count;

        //Debug.Log("here 3");

        int chosenIndex = Random.Range(0, listCount - 1);
        
       // Debug.Log("here 4");

        return (probabilityBank[chosenIndex]);

    }

    public void updateReward(char actionTaken, int returnIndex) {

        string[] directionArray = new string[4] { "left", "right", "forward", "back" };

        //int fReward = 0;
        int greatestValue = 0;
        float rewardFunction;
        int integerRewardFunction;
        float retDist;
        /*
        int R;
        int lastReward;
        int counterOne;
        */

        /*
        if (distToGoal[counter] < distToGoal[counter - 1]) {

            //fReward += 
        }
        */

        for (int j = 0; j < directionArray.Length; j++) {
            //Debug.Log("blab");
            if (directionsTaken[counter - 1] == directionArray[j]) {

                lastReward = lookupTable.qTable[returnIndex, j];

                counterOne = j;
            }
            //Debug.Log("blab 2");
        }

        for (int h = 0; h < 4; h++) {
           // Debug.Log("blab 3");
            if (lookupTable.qTable[returnIndex, h] > greatestValue) {
                
                greatestValue = lookupTable.qTable[returnIndex, h];
            }
        }

        retDist = distanceFunction();
       // Debug.Log("blab 4");
        if (retDist <= 2) {

             R = 100;
        }
        //Debug.Log("blab 4.5");
        rewardFunction = lastReward + ALPHA * (R + (GAMMA * greatestValue) - lastReward);
       // Debug.Log("blab 5");
        integerRewardFunction = (int) rewardFunction;

        //Debug.Log("blab 6");
        // update the Q - table
        lookupTable.qTable[returnIndex, counterOne] += integerRewardFunction;

        //return (integerRewardFunction);
    }

}