// Simple WASD for translation, Q/E for yaw

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float rotSpeed = 2;

    public float yaw = 0f;
    public float pitch = 0f;

    private float move;
    private float rot;


    // Start is called before the first frame update
    void Start()
    {
        move = movementSpeed;
        rot = rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            movementSpeed = 2*move;
            rotSpeed = 2*rot;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            movementSpeed = move;
            rotSpeed = rot;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(transform.InverseTransformDirection(Vector3.up) * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(transform.InverseTransformDirection(- Vector3.up) * movementSpeed * Time.deltaTime);
        }

        // keys
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(transform.InverseTransformDirection(-Vector3.up) * rotSpeed * 10 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(transform.InverseTransformDirection(Vector3.up) * rotSpeed * 10 * Time.deltaTime);
        }
    }
}
