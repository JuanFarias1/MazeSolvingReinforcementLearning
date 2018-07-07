using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticHeight : MonoBehaviour
{
    public InputField height;
    static public int _height1 = 5;

    public void Update()
    {
        int.TryParse(height.text, out _height1);
    }
    
}
