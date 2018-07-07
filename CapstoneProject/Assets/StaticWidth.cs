using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticWidth : MonoBehaviour
{
    public InputField width;
    static public int _width1 = 5;

    public void Update()
    {
        int.TryParse(width.text, out _width1);  
    }

}
