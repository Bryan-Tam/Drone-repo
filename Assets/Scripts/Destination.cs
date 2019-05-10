using UnityEngine;

public class Destination : MonoBehaviour
{
    public int droneCount = 0;
    void OnTriggerEnter(Collider other)
    {
        print("trigger enter");
        var x = other.GetComponent<drone>();
        if (x != null)
        {
            droneCount++;
            Debug.Log($"Drone Counter {droneCount}");
        }
    }
}
