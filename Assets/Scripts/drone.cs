using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

enum ConnectionStatus { Disconnected, Started, Connected, Failed };

public class drone : MonoBehaviour
{
    const int MAX_COMMUNICATION_DISTANCE = 10;
    const int COMMUNICATION_DURATION_SECONDS = 2;
    public bool connected = false;
    private Renderer rend;
    [SerializeField]
    public Transform destination;
    public NavMeshAgent navAgent;

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

    void MessageNeighbors()
    {
        var hitColliders = Physics.OverlapSphere(transform.position, MAX_COMMUNICATION_DISTANCE);
        foreach (Collider c in hitColliders)
        {
            var maybeDrone = c.GetComponent<drone>();
            if (maybeDrone == null || maybeDrone == this)
            {
                continue;
            }
            Debug.Log("Sending message to drone");
            maybeDrone.SendMessage("Ping", (sender: this, message: "connect"));
        }

    }

    // Update is called once per frame
    void Update()
    {
        MessageNeighbors();

        if (connected)
        {
            SetColor(Color.green);
        }
    }

    // Contacted by Drone that wants to communicate
    public async void Ping((drone sender, string message) payload)
    {
        Debug.Log($"received ping from: {payload.sender} with message: {payload.message}");
        await Task.Delay(TimeSpan.FromSeconds(COMMUNICATION_DURATION_SECONDS));
        Debug.Log("connection duration completed");

        // See if drones are still within connection distance after waiting for connection to process
        var distance = Vector3.Distance(transform.position, payload.sender.transform.position);
        if (distance <= MAX_COMMUNICATION_DISTANCE)
        {
            Debug.Log("Drones connected within necessary duration");
            connected = true;
        }
        else
        {
            Debug.Log("Drones were too far apart to connect.");
        }
    }

    public void Delete()
    {
        Destroy(pause_script.selectedObject);
    }

}
