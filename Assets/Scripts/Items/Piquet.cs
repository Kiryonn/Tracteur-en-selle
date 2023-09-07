using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piquet : MonoBehaviour
{
    public List<Transform> attachs;
    [SerializeField] Material ropeMaterial;
    [SerializeField] float ropeWidth;

    [SerializeField] float colliderWidth;

    public float deepness;
    public void AttachPique(Piquet secondPiquet)
    {
        Debug.Log("Trying to attach 2 piquets");
        for (int i = 0; i < attachs.Count; i++)
        {
            if (attachs[i] && secondPiquet.attachs[i])
            {
                CreateRopeBetweenPoint(attachs[i].position, secondPiquet.attachs[i].position);
            }
        }
    }

    void CreateRopeBetweenPoint(Vector3 pointA, Vector3 pointB)
    {
        GameObject rope = new GameObject();
        rope.name = "rope";

        rope.transform.parent = transform;

        LineRenderer lineR = rope.AddComponent<LineRenderer>();
        
        lineR.positionCount = 2;

        lineR.SetPosition(0, pointA);
        lineR.SetPosition(1, pointB);

        lineR.material = ropeMaterial;

        lineR.SetWidth(ropeWidth, ropeWidth);

        Vector3 center = (pointA + pointB) / 2f;

        GameObject boxColl = GameObject.CreatePrimitive(PrimitiveType.Cube);
        boxColl.transform.position = center;

        Vector3 rotation = Vector3.Normalize(pointA - pointB);

        boxColl.transform.rotation = Quaternion.LookRotation(rotation);

        Debug.Log("Longueur de la cloture : "+Vector3.Distance(pointA, pointB));

        boxColl.transform.localScale = new Vector3(colliderWidth,
                                                    1f, 
                                                    Vector3.Distance(pointA, pointB));
        boxColl.GetComponent<MeshRenderer>().enabled = false;
    }
}
