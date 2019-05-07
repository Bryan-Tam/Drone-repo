using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum ConnectionStatus { Disconnected, Started, Connected, Failed };

public class drone : MonoBehaviour
{
    const int MAX_COMMUNICATION_DISTANCE = 10;
    const int COMMUNICATION_FRAME_COUNT = 20;
    public bool connected = false;
    private Renderer rend;
    [SerializeField]
    public Transform destination;
    public NavMeshAgent navAgent;

    // connection status from this Drone to all other neighbor Drones
    private Dictionary<drone, ConnectionStatus> neighbors = new Dictionary<drone, ConnectionStatus>();
    void SetColor(Color c)
    {
        
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", c);

        //Find the Specular shader and change its Color to red
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", c);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();
        // start the colors RED
        SetColor(Color.red);
        navAgent = GetComponent<NavMeshAgent>();
        Debug.Assert(navAgent != null);
        if (destination != null)
        {
            navAgent.SetDestination(destination.transform.position);
        }
    }

    private int FrameCount = 0;

    internal Dictionary<drone, ConnectionStatus> Neighbors { get => neighbors; set => neighbors = value; }

    void MessageNeighbors()
    {
        // only run sometimes
        if (FrameCount == COMMUNICATION_FRAME_COUNT)
        {
            FrameCount = 0;
            var hitColliders = Physics.OverlapSphere(transform.position, MAX_COMMUNICATION_DISTANCE);
            foreach (Collider c in hitColliders)
            {
                var maybeDrone = c.GetComponent<drone>();
                if (maybeDrone != null && maybeDrone != this)
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
        } else
        {
            neighbors.Add(remoteDrone, ConnectionStatus.Started);
        }

    }

    // Update is called once per frame
    void Update()
    {
        MessageNeighbors();


        foreach (var v in neighbors.Values)
        {
            if (v == ConnectionStatus.Connected)
            {
                connected = true;
                break;
            }
        }

        if (connected)
        {
            Debug.Log("CONNECTED");
            SetColor(Color.green);
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

}
