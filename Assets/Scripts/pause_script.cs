// Simulation Code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pause_script : MonoBehaviour
{
    public bool paused;
    public GameObject drone;
    public Button play_pause;
    private Renderer rend;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        paused = false;
        img = play_pause.GetComponent<Image>();

        //rend = drone.GetComponent<Renderer>();
        //rend.material.shader = Shader.Find("_color");
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            int layermask = 1 << 9;
            if (Physics.Raycast(ray, out hit, layermask))
            {
                if (hit.collider != null)
                {
                    // Places "drone" prefab into scene
                    Debug.Log("Hit!");
                    Debug.Log("hit at" + hit.point);
                    Vector3 myPoint = new Vector3(hit.point.x, hit.point.y + .5f, hit.point.z);
                    Instantiate(drone, myPoint, Quaternion.identity);
                }
            }
            else
                Debug.Log("miss");
        }

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

