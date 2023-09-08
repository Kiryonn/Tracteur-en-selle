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
    [SerializeField] int segments = 5;
    [SerializeField] int lengthMultiplier = 4;

    // Use this for initialization
    void Start()
    {
        //MyDebug.Log("Distance : "+Vector3.Distance(neil.transform.position, thomas.transform.position));
        vertexCount = (((int)Vector3.Distance(neil.transform.position, thomas.transform.position)) * lengthMultiplier) - 1;
        //MyDebug.Log("Vertex Count : " + vertexCount);
        //vertexCount = Vector3.Distance(neil.transform.position, thomas.transform.position);

        joints = new List<GameObject>();
        line = GetComponent<LineRenderer>();
        line.SetWidth(0.05f, 0.05f);
        line.SetColors(Color.black, Color.blue);

        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Vector3 dir = neil.transform.position - thomas.transform.position;
        dir = dir.normalized;
        float dist = Vector3.Distance(pos, neil.transform.position);
        //MyDebug.Log("Vertex Count : " + Vector3.Distance(pos, neil.transform.position) / segments);
        for (float i = 0f; i < dist; i += dist/segments)
        {
            //pos = transform.TransformPoint(pos);
            joints.Add((GameObject)Instantiate(emptyPrefab, pos, Quaternion.identity));
            pos = Vector3.MoveTowards(pos, neil.transform.position, dist/segments);
            //pos = transform.TransformPoint(new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + i * 0.25f));
            
        }
        for (int j = 0; j < joints.Count - 1; j++)
        {
            joints[j].transform.parent = this.transform;
            joints[j].GetComponent<HingeJoint>().connectedBody = joints[j + 1].GetComponent<Rigidbody>();
        }
        joints[0].AddComponent<HingeJoint>().connectedBody = thomas.GetComponent<Rigidbody>();
        joints[segments - 1].GetComponent<HingeJoint>().connectedBody = neil.GetComponent<Rigidbody>();
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