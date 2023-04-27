using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RopeGeneratorScript : MonoBehaviour
{

    private LineRenderer line;
    private List<GameObject> joints;
    private int vertexCount;
    private float NTDistance;
    public GameObject emptyPrefab;
    public GameObject neil;
    public GameObject thomas;

    // Use this for initialization
    void Start()
    {
        vertexCount = (((int)Vector3.Distance(neil.transform.position, thomas.transform.position)) * 4) - 1;
        joints = new List<GameObject>();
        line = GetComponent<LineRenderer>();
        line.SetWidth(0.05f, 0.05f);
        line.SetColors(Color.black, Color.blue);
        for (int i = 0; i < vertexCount; i++)
        {
            joints.Add((GameObject)Instantiate(emptyPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + (i + 1) * 0.25f), Quaternion.identity));
        }
        for (int j = 0; j < joints.Count - 1; j++)
        {
            joints[j].transform.parent = this.transform;
            joints[j].GetComponent<HingeJoint>().connectedBody = joints[j + 1].rigidbody;
        }
        joints[0].AddComponent<HingeJoint>().connectedBody = thomas.rigidbody;
        joints[vertexCount - 1].GetComponent<HingeJoint>().connectedBody = neil.rigidbody;
    }

    // Update is called once per frame
    void Update()
    {
        line.SetVertexCount(joints.Count);
        for (int i = 0; i < joints.Count; i++)
        {
            line.SetPosition(i, joints[i].transform.position);
        }
    }
}