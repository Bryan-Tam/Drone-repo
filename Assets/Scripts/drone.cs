using System.Collections.Generic;
using UnityEngine;

enum ConnectionStatus { Disconnected, Started, Connected, Failed };

public class drone : MonoBehaviour
{
    List<Vector3> history = new List<Vector3>();
    List<Vector3> waypoints = new List<Vector3>();
    public float speed;
    const int MAX_COMMUNICATION_DISTANCE = 10;
    const int COMMUNICATION_FRAME_COUNT = 20;
    public bool connected = false;

    // connection status from this Drone to all other neighbor Drones
    Dictionary<drone, ConnectionStatus> neighbors = new Dictionary<drone, ConnectionStatus>();

    // Start is called before the first frame update
    void Start()
    {
        SetWaypoint(new Vector3(0, 0, 0));
    }

    private int FrameCount = 0;

    void MessageNeighbors()
    {
        // only run sometimes
        if (FrameCount == COMMUNICATION_FRAME_COUNT)
        {
            FrameCount = 0;
            var hitColliders = Physics.OverlapSphere(transform.position, MAX_COMMUNICATION_DISTANCE * 1000);
            foreach (Collider c in hitColliders)
            {
                var maybeDrone = c.GetComponent<drone>();
                if (maybeDrone != null)
                {
                    Debug.Log($"collision {c}");

                    maybeDrone.SendMessage("Ping", this);
                }

            }
        }
        FrameCount++;
    }

    // advance connection status from sender to next state
    void NeighborNextState(Dictionary<drone, ConnectionStatus> droneStatuses, drone remoteDrone)
    {
        // FIXME(chdsbd): We never set neighbors
        if (neighbors.ContainsKey(remoteDrone))
        {
            var currentStatus = neighbors[remoteDrone];
            switch (currentStatus)
            {
                case ConnectionStatus.Disconnected:
                    neighbors[remoteDrone] = ConnectionStatus.Started;
                    break;
                case ConnectionStatus.Started:
                    neighbors[remoteDrone] = ConnectionStatus.Connected;
                    break;

            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        MessageNeighbors();
        if (waypoints.Count > 0)
        {
            transform.position = Vector3.Lerp(transform.position, waypoints[0], speed * Time.deltaTime);

        }
        //Debug.Log(transform.position + " | " + waypoints[0]);
        //Debug.Log(waypoints.Count);


        foreach (var v in neighbors.Values)
        {
            if (v == ConnectionStatus.Connected)
            {
                connected = true;
                break;
            }
        }

        // TODO(chdsbd): Update mesh or whatever necesssary to change color of drone based on connection status.
        if (connected)
        {
            Debug.Log("CONNECTED");
            // FIXME(chdsbd): This doesn't actually change the color. We probably want to simplify things and remove the complex Drone mesh.
            var material = this.GetComponent<MeshRenderer>();
            material.material.color = Color.green;
        }
    }

    // Contacted by Drone that wants to communicate
    public void Ping(drone sender)
    {
        Debug.Log($"received ping from: {sender}");
        NeighborNextState(neighbors, sender);
    }

    public void Delete()
    {
        Destroy(pause_script.selectedObject);
    }

    public void SetWaypoint(Vector3 point)
    {
        waypoints.Add(point);
        foreach (Vector3 item in waypoints)
        {
            Debug.Log(item);
        }
    }

    public void Fifo_remove()
    {
        history.Add(waypoints[0]);
        waypoints.Remove(waypoints[0]);
        Debug.Log("Rmove");
        for (int i = 1; i < waypoints.Count; i++)
        {
            waypoints[i - 1] = waypoints[i];
        }
    }

    public void RouteBtn()
    {
        Debug.Log("Ready to route");
        pause_script.isRoute = true;
    }
}
