using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraControl : MonoBehaviour { 


    public float HorizontalSpeed = 2.0f;
    public float VerticalSpeed = 2.0f;
    public float MovementSpeed = 0.2f;

    public float yaw = 45.0f;
    public float pitch = 45.0f;

    protected float HorizontalMaxRange = 50.0f;
    protected float HorizontalMinRange = -50.0f;
    public float mHeight = 20.0f;


    private void Start()
    {
   
        this.transform.position = new Vector3(0.5f * HorizontalMinRange, 3.0f * mHeight, 0.5f * HorizontalMinRange);
        //transform.LookAt(new Vector3(0, 0, 0));
    }

    private void Update()
    {
        Vector3 CurrentLocation = this.transform.position;
        Vector3 UpdateLocation = CurrentLocation;
        if (CurrentLocation.x < HorizontalMinRange)
        {
            UpdateLocation.x = HorizontalMinRange;
        }
        if (CurrentLocation.x > HorizontalMaxRange)
        {
            UpdateLocation.x = HorizontalMaxRange;
            }
            if (CurrentLocation.z < HorizontalMinRange)
            {
                UpdateLocation.z = HorizontalMinRange;
            }
            if (CurrentLocation.z > HorizontalMaxRange)
            {
                UpdateLocation.z = HorizontalMaxRange;
            }
        if (CurrentLocation != UpdateLocation)
        {
            this.transform.position = UpdateLocation;
        }


        yaw += HorizontalSpeed * Input.GetAxis("Mouse X");
        pitch -= VerticalSpeed * Input.GetAxis("Mouse Y");

        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);

        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(Input.GetAxis("Horizontal") * MovementSpeed, 0, 0);
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(0, 0, Input.GetAxis("Vertical") * MovementSpeed);
        }


    }



    void FixedUpdate(){
      Vector3 v = new Vector3(0, 0, 0);
      this.gameObject.GetComponent<Rigidbody>().velocity = v;
    }


}
