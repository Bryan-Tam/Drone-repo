// Simulation Code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class pause_script : MonoBehaviour
{
    public bool paused;
    public GameObject drone;
    public Button play_pause;
    //private Renderer rend;

    public static GameObject selectedObject;
    public float timescale { get; set; }
    private Image img;
    private int range = 1000;
    private GameObject instDrone;
    private int i = 1;
    public static bool isRoute;
    public Slider mainSlider;
    public Text slidetxt;

    // Start is called before the first frame update
    void Start()
    {
        paused = true;
        isRoute = false;
        Time.timeScale = 0;
        img = play_pause.GetComponent<Image>();
        img.color = Color.yellow;
        //rend = drone.GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("_color");
        
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = timescale;
        slidetxt.text = mainSlider.value.ToString() + "x real time";
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
                    if (isRoute)
                    {
                        //get coordinates
                        Debug.Log("route");
                        Vector3 myPoint = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z);
                        selectedObject.GetComponent<drone>().SetWaypoint(myPoint);
                        isRoute = false;
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
                    // Places "drone" prefab into scene
                    //Debug.Log("Hit!");
                    //Debug.Log("hit at" + hit.point);
                    Vector3 myPoint = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z);
                    instDrone = Instantiate(drone, myPoint, Quaternion.identity);
                    instDrone.name = "drone" + i;
                    i++;
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

