using System.Collections;
using System.Collections.Generic;
using System.IO;            // needed for StreamWriter
using UnityEngine;      
using UnityEditor;          // needed for AssetDatabase

public class TextWriteTest : MonoBehaviour {


    public TextAsset text;
    private string textFPath = "Assets/Resources/QTableTextFile.txt";
    private string[] clearStr = { "" };

    public void exportQtableToText(QTable qtable) {

        // clear textFile contents of previous iteration
        File.WriteAllLines(textFPath, clearStr);
        StreamWriter writeObj = new StreamWriter(textFPath, true);


        for (int i = 0; i < 15; i++) {
            for (int j = 0; j < 4; j++) {
                // write to textFile
                writeObj.WriteLine(qtable.qTable[i, j].ToString());

            }
        }
        writeObj.Close();


        // update textFile in asset with new values
        AssetDatabase.ImportAsset(textFPath);
        TextAsset asset = Resources.Load("QTableTextFile") as TextAsset;
        
    }

    public void importQtableText(QTable qtable) {

        // open a reading session
        StreamReader readObj = new StreamReader(textFPath);
        string currentValue;
        int CurrentNumValue;

        while ((currentValue = readObj.ReadLine()) == (null) || (currentValue = readObj.ReadLine()) == "") {
            
        }

        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                // write to textFile
                Debug.Log(currentValue);
                int.TryParse(currentValue, out CurrentNumValue);
                // update the qtable with values from textFile
                qtable.qTable[i, j] = CurrentNumValue; 
                currentValue = readObj.ReadLine();
                    
            }
        }
        // close reading session
        readObj.Close();
    }

}
