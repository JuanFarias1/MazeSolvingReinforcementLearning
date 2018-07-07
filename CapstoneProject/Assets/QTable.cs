using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTable : MonoBehaviour
{

    public int[,] qTable = new int[15, 4];

    // createa a 15 x 4 2D array for states x possible actions
    public void qTableInit()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                qTable[i, j] = 2; // must be mod 2;

            }
        }
    }

}
