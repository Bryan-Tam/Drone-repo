using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class drone_info_display : MonoBehaviour
{
    Text child_text;
    string parentName;

    // Start is called before the first frame update
    void Start()
    {
        parentName = transform.parent.name;
        child_text = GetComponentInChildren<Text>();
        child_text.text = parentName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
