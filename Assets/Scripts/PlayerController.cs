using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float modifier = 1f; // float to control speed with like mud or stop the car
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

    // Automatic control values

    Vector3 destinationPoint;
    float minDistance;

    enum NavState
    {
        PlayerControl,
        Forced,
        Stopped
    }

    NavState navState;
    private void Start()
    {
        navState = NavState.PlayerControl;
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
        switch (navState)
        {
            case NavState.PlayerControl:
                if (canMove && resourceController.SuffisantEnergy())
                {
                    modifier = 1f;
                    if (movement >= 0.1f)
                    {
                        resourceController.UseEnergy(energyRequired);
                    }
                }
                else
                {
                    modifier = 0f;
                }
                HandleMotor();
                HandleSteering();
                break;
            case NavState.Forced:
                HandleAutomatic();
                break;
            case NavState.Stopped:
                break;
            default:
                break;
        }
        
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
        frontLeftWheelCollider.motorTorque = movement * speed * modifier;
        frontRightWheelCollider.motorTorque = movement * speed * modifier;
        //backLeftWheelCollider.motorTorque = movement * speed;
        //backRightWheelCollider.motorTorque = movement * speed;
    }

    void HandleSteering()
    {
        float turnAngle = rotation * maxAngle;
;
        frontLeftWheelCollider.steerAngle = turnAngle;
        frontRightWheelCollider.steerAngle = turnAngle;

        UpdateWheels();
    }

    void UpdateWheel(WheelCollider col, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        col.GetWorldPose(out position, out rotation);

        trans.position = position;
        trans.rotation = rotation;
    }

    public void ForceDestination(Vector3 targetPoint, float minDist)
    {
        destinationPoint = targetPoint;
        minDistance = minDist;
        navState = NavState.Forced;
    }

    void HandleAutomatic()
    {
        Debug.Log("Tractor going to :" + destinationPoint + " from :" + transform.position);
        float distanceToTarget = Vector3.Distance(transform.position, destinationPoint);
        if (distanceToTarget > minDistance)
        {
            Vector3 targetDirection = (destinationPoint - transform.position).normalized;
            float steeringAngle = Vector3.Angle(targetDirection, transform.forward);
            if (steeringAngle > 5f)
            {
                float rotationDirection = Vector3.Cross(targetDirection, transform.forward).y;
                float steerDirection = Mathf.Sign(rotationDirection);
                float steerAmount = Mathf.Clamp(steeringAngle / maxAngle, 0f, 1f) * steerDirection;
                frontLeftWheelCollider.steerAngle = maxAngle * steerAmount;
                frontRightWheelCollider.steerAngle = maxAngle * steerAmount;
            }
            frontLeftWheelCollider.motorTorque = speed;
            frontRightWheelCollider.motorTorque = speed;

            UpdateWheels();
        }
        else
        {
            navState = NavState.PlayerControl;
        }
    }

    void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheel(frontRightWheelCollider, frontRightWheel);
        UpdateWheel(backLeftWheelCollider, backLeftWheel);
        UpdateWheel(backRightWheelCollider, backRightWheel);
    }
}
