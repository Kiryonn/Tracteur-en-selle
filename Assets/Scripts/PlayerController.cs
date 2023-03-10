using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float rotationSpeed;
    ResourceController resourceController;
    Rigidbody rb;
    float movement;
    float rotation;
    [SerializeField] float maxAngle;
    [HideInInspector]
    public bool canMove = true;
    [SerializeField] float energyRequired;

    [SerializeField] WheelCollider frontRightWheelCollider;
    [SerializeField] WheelCollider frontLeftWheelCollider;
    [SerializeField] WheelCollider backRightWheelCollider;
    [SerializeField] WheelCollider backLeftWheelCollider;

    [SerializeField] Transform frontRightWheel;
    [SerializeField] Transform frontLeftWheel;
    [SerializeField] Transform backRightWheel;
    [SerializeField] Transform backLeftWheel;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        resourceController = GetComponent<ResourceController>();
    }

    private void Update()
    {
        movement = Input.GetAxis("Vertical");
        rotation = Input.GetAxis("Horizontal");
        
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
            /*
        if (canMove && movement.magnitude >=0.1f && resourceController.SuffisantEnergy())
        {
            resourceController.UseEnergy(energyRequired);
            rb.AddForce(movement * speed * 500f * Time.fixedDeltaTime, ForceMode.Force);
            Quaternion deltaRotation = Quaternion.Euler(0, rotation, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        */
    }

    void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = movement * speed;
        frontRightWheelCollider.motorTorque = movement * speed;
        //backLeftWheelCollider.motorTorque = movement * speed;
        //backRightWheelCollider.motorTorque = movement * speed;
    }

    void HandleSteering()
    {
        float turnAngle = rotation * maxAngle;
;
        frontLeftWheelCollider.steerAngle = turnAngle;
        frontRightWheelCollider.steerAngle = turnAngle;

        UpdateWheels(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheels(frontRightWheelCollider, frontRightWheel);
        UpdateWheels(backLeftWheelCollider, backLeftWheel);
        UpdateWheels(backRightWheelCollider, backRightWheel);
    }

    void UpdateWheels(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }
    
}
