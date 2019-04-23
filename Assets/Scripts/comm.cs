using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comm : MonoBehaviour
{
    bool first = true;
    int p = 23;
    int g = 5;
    int b = 3;
    int a = 4;
    
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "drone")
        {
            if (first)
            {
                first = false;
            }
            else if (!first) {
                // broadcast picked up
                Debug.Log(transform.parent.name + " >> " + other.gameObject.name);
                StartCoroutine(Delay(other));
                
                //Debug.Log()
            }
        }
    }

    IEnumerator Delay(Collider other)
    {
        //Debug.Log("waiting");
        yield return new WaitForSeconds(.0001f);
        Debug.Log("Authenticated certificate" + other.gameObject.name);
        yield return new WaitForSeconds(.0001f);
        Debug.Log("Diffie-Helman, send " + transform.parent.name + " public key");

    }
}   