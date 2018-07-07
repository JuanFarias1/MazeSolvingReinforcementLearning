using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollect : MonoBehaviour
{

    List<Vector3> positions = new List<Vector3>();
    List<string> directionTaken = new List<string>();   // direction that the cube took previously

    List<float> distToGoal = new List<float>();  // distance to target

    List<bool> canMoveLeft = new List<bool>();
    List<bool> canMoveRight = new List<bool>();
    List<bool> canMoveForward = new List<bool>();
    List<bool> canMoveBack = new List<bool>();

    List<int> rewardGiven = new List<int>();    // current reward given the action taken 



    public List<Vector3> getPositions() {

        return positions;  // list of current positions

    }

    public List<string> getdDirectionTaken()
    {

        return directionTaken;  // list of current positions

    }

    public List<float> getDistToGoal()
    {

        return distToGoal;  // list of current positions

    }

    public List<bool> getCanMoveLeft()
    {

        return canMoveLeft;  // list of current positions

    }

    public List<bool> getCanMoveRight()
    {

        return canMoveRight;  // list of current positions

    }

    public List<bool> getCanMoveForward()
    {

        return canMoveForward;  // list of current positions

    }

    public List<bool> getCanMoveBack()
    {

        return canMoveBack;  // list of current positions

    }

    public List<int> getRewardGiven()
    {

        return rewardGiven;  // list of current positions

    }
}
