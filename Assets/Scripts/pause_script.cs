// Simulation Code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using System.Linq;
using System;

public class pause_script : MonoBehaviour
{
    public bool paused;
    public GameObject drone;
    public Button play_pause;
    //private Renderer rend;

    public static GameObject selectedObject;
    private Image img;
    private int range = 1000;
    private int nextDroneId = 1;
    public static bool isRoute;

    private int numOfDronesToSpawn = 10;

    // Start is called before the first frame update
    void Start()
    {
        paused = true;
        isRoute = false;
        Time.timeScale = 0;
        img = play_pause.GetComponent<Image>();
        img.color = Color.yellow;

        // Don't spawn a bunch of drones. We are currently using pre-placed game objects for our simulation.
        return;

        // spawn a number of drones inside the spawn area
        foreach (var i in Enumerable.Range(0, numOfDronesToSpawn)) {
            Debug.Log(i);

            var pos = getRandomSpawnPoint();
            CreateDrone(pos);

        }


    }

    private Vector3 getRandomSpawnPoint()
    {
        return GameObject.Find("Spawn Point").gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        // Left Clicks
        if (Input.GetMouseButtonDown(0))
        {
            // if clicking UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layermask = 1 << 10;
            layermask = layermask ^ (1 << 9);

            if (Physics.Raycast(ray, out hit, range, layermask))
            {
                // Selected ground
                
                if (hit.collider.gameObject.layer == 9)
                {
                    if (!isRoute)
                    {
                        Debug.Log("Clear");
                        ClearSelection();
                    }
                }

                // Selected a drone
                else if (hit.collider.gameObject.layer == 10)
                {
                    GameObject hitObject = hit.transform.root.gameObject;
                    //hitObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                    SelectObject(hitObject);

                }
            }

            //else
            //ClearSelection();

        }
        // Right Clicks
        if (Input.GetMouseButtonDown(1))
        {
            // if clicking UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            int layermask = 1 << 10;
            layermask = layermask ^ (1 << 9);

            if (Physics.Raycast(ray, out hit, range, layermask))
            {
                // Selected ground
                if (hit.collider.gameObject.layer == 9)
                {
                    var initialPos = GameObject.Find("Spawn Point").gameObject.transform.position;
                    CreateDrone(initialPos);
                }

                // Selected a drone
                else if (hit.collider.gameObject.layer == 10)
                {
                    //

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }
    }

    private void CreateDrone(Vector3 pos)
    {
        var instDrone = Instantiate(drone, pos, Quaternion.identity);
        instDrone.name = "drone" + nextDroneId;
        nextDroneId++;
        var destination = GameObject.Find("Destination").gameObject.transform.position;
        instDrone.GetComponent<NavMeshAgent>().SetDestination(destination);
    }

    void ClearSelection()
    {
        if(selectedObject != null)
        {
            selectedObject.GetComponentInChildren<Canvas>().enabled = false;
            selectedObject.GetComponentInChildren<Projector>().enabled = false;
            selectedObject = null;
        }
    }

    void SelectObject(GameObject obj)
    {
       if (selectedObject != null)
        {
            if (obj == selectedObject)
            {
                return;
            }
            ClearSelection();
        }

        selectedObject = obj;
        selectedObject.GetComponentInChildren<Canvas>().enabled=true;
        selectedObject.GetComponentInChildren<Projector>().enabled = true;
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            Time.timeScale = 0;
            //rend.material.SetColor("_Color", Color.yellow);
            
            img.color = Color.yellow;
        }
        if (!paused)
        {
            Time.timeScale = 1;
            //rend.material.SetColor("_Color", Color.blue);

            img.color = Color.white;
        }
    }
}

