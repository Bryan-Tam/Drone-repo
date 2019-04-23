using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drone : MonoBehaviour
{
    List <Vector3> history = new List<Vector3>();
    List<Vector3> waypoints = new List<Vector3>();
    Vector3 dest;
    public float speed;
    Vector3 pt;
    float height;

    // Start is called before the first frame update
    void Start()
    {
        //pt = new Vector3(0,0,0);
        //SetWaypoint(pt);
    }

    // Update is called once per frame
    void Update()
    {
        // try to keep drone above the ground the same amount
        RaycastHit hit;
        if (waypoints.Count != 0)
        {
            transform.position = Vector3.Lerp(transform.position, waypoints[0], speed * Time.deltaTime);
        }
        Physics.Raycast(transform.position, -transform.up, out hit, height);
    }

    public void Delete()
    {
        Destroy(pause_script.selectedObject);
    }

    public void SetWaypoint(Vector3 point)
    {
        waypoints.Add(point);
        foreach(Vector3 item in waypoints)
        {
            Debug.Log(item);
        }
    }

    public void Fifo_remove()
    {
        history.Add(waypoints[0]);
        waypoints.Remove(waypoints[0]);
        //Debug.Log("Rmove");
        for (int i = 1; i < waypoints.Count; i++)
        {
            waypoints[i - 1] = waypoints[i];
        }
    }

    public void RouteBtn()
    {
        //Debug.Log("Ready to route");
        pause_script.isRoute = true;
    }
}
