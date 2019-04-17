using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class drone_info_display : MonoBehaviour
{
    Text child_text;
    string parentName;
    Vector3 location;

    // Start is called before the first frame update
    void Start()
    {
        parentName = transform.parent.name;
        
        
        child_text = GetComponentInChildren<Text>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        location = transform.parent.localPosition;
        string output = parentName + "\n" + location;
        child_text.text = output;
    }
}
