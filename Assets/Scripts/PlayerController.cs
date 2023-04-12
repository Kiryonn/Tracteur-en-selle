using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isCharacterControlled;
    [Header("Tractor Movement")]

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

    [SerializeField] GameObject tractorGraphics;

    [Header("Character Movement")]

    [SerializeField] float charaSpeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    Vector3 currentDirection = Vector3.forward;
    Vector3 currentVelocity;
    CharacterController characterController;
    [SerializeField] GameObject charaGraphics;
    [SerializeField] Animator playerAnim;

    [Header("Shaders")]
    public Renderer cabin;

    // Automatic control values

    Vector3 destinationPoint;
    float minDistance;
    public bool destinationReached { get; private set; }

    [Header("Specific Equipement")]

    public Animator tractorAnim;
    public Animator secateurAnimator;

    public enum NavState 
    {
        PlayerControl,
        Forced,
        Stopped
    }

    public NavState navState { get; private set; }
    private void Start()
    {
        navState = NavState.PlayerControl;
        rb = GetComponent<Rigidbody>();
        resourceController = GetComponent<ResourceController>();
        characterController = GetComponent<CharacterController>();
        if (isCharacterControlled) StartCoroutine(SwitchControls("Character",false));
        if (!isCharacterControlled) StartCoroutine(SwitchControls("Tractor",false));
    }

    private void Update()
    {
        movement = Input.GetAxis("Vertical");
        rotation = Input.GetAxis("Horizontal");
        
    }

    private void FixedUpdate()
    {
        if (!isCharacterControlled)
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
                    destinationReached = false;
                    HandleAutomatic();
                    break;
                case NavState.Stopped:
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (canMove)
            {
                Vector3 direction = transform.TransformDirection(new Vector3(rotation, 0f, movement));
                HandleMovement(direction);
            }
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

    void HandleMovement(Vector3 direction)
    {
        
        if (direction.magnitude >= 0.1f)
        {
            if (movement >= 0)
            {
                //currentDirection = Vector3.SmoothDamp(currentDirection, direction, ref currentVelocity, turnSpeed);
                //transform.rotation = Quaternion.LookRotation(currentDirection);
                transform.Rotate(Vector3.up * rotation * turnSpeed * Time.deltaTime);

            }
            else
            {
                transform.Rotate(Vector3.up * -rotation * turnSpeed * Time.deltaTime);
            }
            
            characterController.Move(characterController.transform.forward * movement * charaSpeed * Time.deltaTime);
        }
        playerAnim.SetFloat("Blend", movement);
        
    }

    void HandleRotation(Vector3 direction)
    {
        if (direction.magnitude > 0f)
        {
            
        }
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

    public void ForceDestination(Vector3 startPoint, Vector3 targetPoint, float minDist)
    {
        transform.position = startPoint;
        transform.LookAt(targetPoint);
        destinationPoint = targetPoint;
        frontLeftWheelCollider.steerAngle = 0f;
        frontRightWheelCollider.steerAngle = 0f;
        minDistance = minDist;
        navState = NavState.Forced;
    }

    void HandleAutomatic()
    {
        
        float distanceToTarget = Vector3.Distance(transform.position, destinationPoint);
        //Debug.Log("Distance restante : " + distanceToTarget);
        if (distanceToTarget > minDistance)
        {
            /*
            Vector3 targetDirection = (destinationPoint - transform.position).normalized;
            float steeringAngle = Vector3.Angle(targetDirection, transform.forward);
            Debug.Log("Angle = " + steeringAngle);
            if (steeringAngle > 15f)
            {
                float rotationDirection = Vector3.Cross(targetDirection, transform.forward).y;
                float steerDirection = Mathf.Sign(rotationDirection);
                float steerAmount = Mathf.Clamp(steeringAngle / maxAngle, 0f, 1f) * steerDirection;
                //float steerAmount = steeringAngle / maxAngle;
                Debug.Log("Clamp : " + Mathf.Clamp(steeringAngle / maxAngle, 0f, 1f));
                frontLeftWheelCollider.steerAngle = maxAngle * steerAmount;
                frontRightWheelCollider.steerAngle = maxAngle * steerAmount;
            }
            else
            {
                frontLeftWheelCollider.steerAngle = 0f;
                frontRightWheelCollider.steerAngle = 0f;
            }
            */
            frontLeftWheelCollider.motorTorque = speed;
            frontRightWheelCollider.motorTorque = speed;

            UpdateWheels();
        }
        else
        {
            navState = NavState.PlayerControl;
            destinationReached = true;
        }
    }

    void UpdateWheels()
    {
        UpdateWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateWheel(frontRightWheelCollider, frontRightWheel);
        UpdateWheel(backLeftWheelCollider, backLeftWheel);
        UpdateWheel(backRightWheelCollider, backRightWheel);
    }

    public void StopTractor()
    {
        frontLeftWheelCollider.brakeTorque = 10000f;
        frontRightWheelCollider.brakeTorque = 10000f;
        Invoke("ReleaseTractor", 1f);
    }

    void ReleaseTractor()
    {
        frontLeftWheelCollider.brakeTorque = 0f;
        frontRightWheelCollider.brakeTorque = 0f;
    }

    public void SwitchControl(string To)
    {
        if (To == "Character")
        {
            rb.isKinematic = true;
            characterController.enabled = true;
            isCharacterControlled = true;
            charaGraphics.SetActive(true);
            tractorGraphics.SetActive(false);
        }
        else
        {
            rb.isKinematic = false;
            characterController.enabled = false;
            isCharacterControlled = false;
            charaGraphics.SetActive(false);
            tractorGraphics.SetActive(true);
        }
    }

    public IEnumerator SwitchControls(string to, bool transition)
    {
        if (transition)
        {
            GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1f, 3f, 1f);
            yield return new WaitForSeconds(3f);
        }
        SwitchControl(to);
    }
}
