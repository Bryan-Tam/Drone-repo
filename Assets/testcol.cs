using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcol : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("hit " + collision.name);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
    }
}
