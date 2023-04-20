using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomJoint : MonoBehaviour
{
    [SerializeField] Transform tracteur;
    [SerializeField] float distanceMinimal = 5f;
    [SerializeField] float force = 50f;
    [SerializeField] float forceRotation = 10f;
    [SerializeField] Transform anchor;
    public bool isAttached;

    [Header("Limits")]
    [SerializeField] float maxAngleX;
    [SerializeField] float maxAngleY;
    [SerializeField] float maxAngleZ;

    Rigidbody rb;

    [Header("VFX")]
    [SerializeField] Vector3 anchorOffset;
    [SerializeField] Vector3 trailerOffset;
    TrailerChainVFX trailerChainVFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trailerChainVFX = GetComponent<TrailerChainVFX>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (isAttached)
        {
            Vector3 direction = tracteur.position - transform.position;
            float distance = direction.magnitude;

            if (distance > distanceMinimal)
            {

                Vector3 targetPosition = tracteur.position - direction.normalized * distanceMinimal;
                rb.MovePosition(targetPosition);

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                Quaternion deltaRotation = targetRotation * Quaternion.Inverse(transform.rotation);
                deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);
                if (angle > 180) angle = -360;

                float angleX = Mathf.Clamp(angle * axis.x, -maxAngleX, maxAngleX);
                float angleY = Mathf.Clamp(angle * axis.y, -maxAngleY, maxAngleY);
                float angleZ = Mathf.Clamp(angle * axis.z, -maxAngleZ, maxAngleZ);

                axis = new Vector3(angleX / angle, angleY / angle, angleZ / angle);

                //rb.MoveRotation(Quaternion.Euler(axis.normalized * Time.deltaTime));
                rb.AddTorque(axis * angle * forceRotation, ForceMode.Force);
            }
        }
        
    }

    public void SetTracteur(Transform t)
    {
        tracteur = t;
    }

    public void UpdateJoint(bool b)
    {
        isAttached = b;
        if (b)
        {
            trailerChainVFX.AttachVFX(tracteur.transform, anchorOffset, rb.transform, trailerOffset);
        }
        else
        {
            trailerChainVFX.DetachVFX();
        }
    }


}
