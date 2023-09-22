using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isCharacterControlled;

    //Controllers arduino or smth
    ArduinoConnector arduino;
    float arduinoMovementNormalized;
    float arduinoRotationNormalized;
    [Header("Tractor Movement")]

    [SerializeField] float maxGroundedTime = 5f;
    [SerializeField] float groundedTime;

    float modifier = 1f; // float to control speed with like mud or stop the car
    [SerializeField] float speed = 1f;
    [SerializeField] float rotationSpeed;
    ResourceController resourceController;
    Rigidbody rb;
    public float movement { get; private set; }
    public float rotation { get; private set; }
    [SerializeField] float maxAngle;
    //[HideInInspector]
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

    public GameObject tractorGraphics;

    [Header("Character Movement")]

    public float charaSpeed;
    [SerializeField] float gravity;
    [SerializeField] float turnSpeed;
    Vector3 currentDirection = Vector3.forward;
    Vector3 currentVelocity;
    float blendVal = 0f;
    [SerializeField] float smoothAnim;
    public CharacterController characterController { get; private set; }
    public GameObject charaGraphics;

    public Animator playerAnim;

    [Header("Shaders")]
    public Renderer cabin;

    // Automatic control values
    [Header("Navmesh values")]
    [SerializeField] float maxAutoSpeed = 4f;
    Vector3 destinationPoint;
    float minDistance;
    [SerializeField] SpeedSystem uiSpeed;
    public bool destinationReached { get; private set; }
    PostProcessManager processManager;

    [Header("Specific Equipement")]

    public Animator tractorAnim;
    public Animator secateurAnimator;

    public EquipmentRecup equipment;
    public bool isEquipped { get { return equipment != null; } }
    GameObject duplicate;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        resourceController = GetComponent<ResourceController>();
        characterController = GetComponent<CharacterController>();
        arduino = GetComponentInChildren<ArduinoConnector>();
        processManager = GetComponent<PostProcessManager>();
    }

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
    }

    private void Update()
    {
        arduinoMovementNormalized = arduino.speed / 700 - arduino.reverseSpeed / 700;
        //arduinoRotationNormalized = arduino.speed / 1000;

        //arduinoRotationNormalized = -Mathf.Clamp(arduino.qx*7f,-1f,1f);

        arduinoRotationNormalized = ArduinoRotation(arduino.roll, -20f, 20f, (-1.5f, 1.5f));



        movement = (Mathf.Abs(Input.GetAxis("Vertical")) > Mathf.Abs(arduinoMovementNormalized)) ? Input.GetAxis("Vertical") : arduinoMovementNormalized;

        //arduinoRotationNormalized = Mathf.Lerp(-1, 1, arduinoRotationNormalized);

        rotation = (Mathf.Abs(Input.GetAxis("Horizontal")) > Mathf.Abs(arduinoRotationNormalized)) ? Input.GetAxis("Horizontal") : arduinoRotationNormalized;
        //rotation = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MyDebug.Log("Escape is pressed");
            GameManager.Instance.PauseGame();
        }

    }

    float ArduinoRotation(float value, float min, float max, (float, float) deadZone)
    {
        if (value >= deadZone.Item1 && value <= deadZone.Item2)
        {
            return 0;
        }
        bool m = (Mathf.Abs(value - max) < Mathf.Abs(value - min));
        float rapport;

        if (m)
        {
            rapport = Mathf.Abs(value / max);
            rapport = Mathf.Lerp(0.5f, 1f, rapport);
        }
        else
        {
            rapport = Mathf.Abs(value / min);
            rapport = Mathf.Lerp(0.5f, -1f, rapport);
        }

        float result = Mathf.SmoothStep(-1f, 1f, rapport);
        //MyDebug.Log("Rapport : " + rapport + " & Result = " + result);

        return result;
    }

    private void FixedUpdate()
    {
        if (!isCharacterControlled)
        {
            IsGrounded();
            float y = Mathf.InverseLerp(5f, 17f, rb.velocity.magnitude);
            uiSpeed.particleSystem.emissionRate = Mathf.Lerp(0f, uiSpeed.maxParticle, y);
            processManager.ChangeLensDistord(Mathf.Lerp(0f, -0.5f, y * 1.2f));
            switch (navState)
            {
                case NavState.PlayerControl:
                    if (canMove)
                    {
                        if (resourceController.SuffisantEnergy())
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

                    }
                    else
                    {
                        rotation = 0f;
                        movement = 0f;
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
            uiSpeed.particleSystem.emissionRate = 0f;
            playerAnim.SetFloat("AnimSpeed", charaSpeed / 3f);
            if (!characterController.isGrounded)
            {
                Vector3 velocity = Vector3.zero;
                velocity.y += gravity * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
            }
            if (canMove)
            {
                Vector3 direction = transform.TransformDirection(new Vector3(rotation, 0f, movement));
                HandleMovement(direction);
            }
            else
            {
                playerAnim.SetFloat("Blend", 0f);
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
        blendVal = Mathf.MoveTowards(playerAnim.GetFloat("Blend"), movement, smoothAnim);
        playerAnim.SetFloat("Blend", blendVal);

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

        Vector3 targetDirection = (destinationPoint - transform.position).normalized;

        //Vector3 centerOfWheels = (frontLeftWheel.position + frontRightWheel.position) / 2;
        //Debug.DrawRay(centerOfWheels, targetDirection, Color.red);
        //MyDebug.Log("Distance restante : " + distanceToTarget);

        if (distanceToTarget > minDistance)
        {
            float steeringAngle = Vector3.Angle(targetDirection, transform.forward);
            if (steeringAngle > 15f)
            {
                float rotationDirection = -Vector3.Cross(targetDirection, transform.forward).y;
                float steerDirection = Mathf.Sign(rotationDirection);
                float steerAmount = Mathf.Clamp(steeringAngle / maxAngle, 0f, 1f) * steerDirection;

                frontLeftWheelCollider.steerAngle = maxAngle * steerAmount;
                frontRightWheelCollider.steerAngle = maxAngle * steerAmount;
            }
            else
            {
                frontLeftWheelCollider.steerAngle = 0f;
                frontRightWheelCollider.steerAngle = 0f;
            }

            frontLeftWheelCollider.motorTorque = Mathf.Lerp(speed, 0f, rb.velocity.magnitude / maxAutoSpeed);

            frontRightWheelCollider.motorTorque = Mathf.Lerp(speed, 0f, rb.velocity.magnitude / maxAutoSpeed);

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

    void SwitchControl(string To, bool keep)
    {
        if (To == "Character")
        {
            UIManager.instance.HideEnergy(true);
            rb.isKinematic = true;
            characterController.enabled = true;
            isCharacterControlled = true;
            charaGraphics.SetActive(true);
            if (keep)
            {
                GameObject empty = new GameObject();
                empty = Instantiate(empty);
                empty.transform.position = tractorGraphics.transform.position;
                empty.transform.rotation = tractorGraphics.transform.rotation;
                empty.transform.localScale = tractorGraphics.transform.lossyScale;
                duplicate = Instantiate(tractorGraphics, empty.transform);
            }
            LeanTween.moveX(gameObject, transform.position.x + 10f, 0f);
            tractorGraphics.SetActive(false);
            GameManager.Instance.SwitchCam(CamTypes.Character);
        }
        else
        {
            UIManager.instance.HideEnergy(false);
            if (duplicate)
            {
                Destroy(duplicate);
            }
            rb.isKinematic = false;
            characterController.enabled = false;
            isCharacterControlled = false;
            charaGraphics.SetActive(false);
            tractorGraphics.SetActive(true);
            GameManager.Instance.SwitchCam(CamTypes.Tractor);
        }
    }

    public IEnumerator SwitchControls(string to, bool transition, bool keepGraphic = false)
    {
        //MyDebug.Log("Switching player");
        if (transition)
        {
            canMove = false;
            GameManager.Instance.GetComponent<TransitionManager>().FadeTransition(1f, 3f, 1f);
            yield return new WaitForSeconds(3f);
            canMove = true;
        }

        SwitchControl(to, keepGraphic);
    }

    public void SetEquipment(EquipmentRecup _equipment, bool switchCam = true)
    {
        if (equipment != null)
        {
            equipment.RemoveEquipment();
        }
        equipment = _equipment;
        if (switchCam)
        {
            GameManager.Instance.SwitchCam(CamTypes.Equipments);
        }

    }

    public void StopAllMovements(bool yesOrNo)
    {
        if (yesOrNo)
        {
            canMove = false;
            rb.isKinematic = true;
            characterController.enabled = false;
        }
        else
        {
            canMove = true;
            rb.isKinematic = false;
            characterController.enabled = true;
        }
    }

    void IsGrounded()
    {

        WheelHit hit1;
        WheelHit hit2;

        if (!(backLeftWheelCollider.GetGroundHit(out hit1) && frontRightWheelCollider.GetGroundHit(out hit2)))
        {
            groundedTime += Time.deltaTime;
        }
        else
        {
            groundedTime = 0f;
        }

        if (groundedTime > maxGroundedTime)
        {
            GameManager.Instance.SpawnPlayer(true);
            groundedTime = 0f;
        }

    }

    public Animator PlayAltAnim(string animName)
    {
        GameObject normChar = charaGraphics.transform.GetChild(1).gameObject;
        GameObject altChar = charaGraphics.transform.GetChild(2).gameObject;
        altChar.SetActive(true);
        normChar.SetActive(false);

        Animator animator = altChar.GetComponent<Animator>();

        animator.SetTrigger(animName);

        return animator;
    }

    public void AlternateCharacter(int indexOfChild, string animName)
    {
        GameObject normChar = charaGraphics.transform.GetChild(1).gameObject;
        GameObject altChar = charaGraphics.transform.GetChild(2).gameObject;

        normChar.SetActive(indexOfChild == 1);
        altChar.SetActive(indexOfChild == 2);

        if (indexOfChild == 2)
        {
            altChar.GetComponent<Animator>().Play(animName);
        }
        
    }

    public void AlternateCharacter(bool alt)
    {
        GameObject child0 = charaGraphics.transform.GetChild(1).gameObject;
        if (child0.activeSelf)
        {
            child0.SetActive(false);
            charaGraphics.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            child0.SetActive(true);
            charaGraphics.transform.GetChild(2).gameObject.SetActive(false);
        }
    }
}
