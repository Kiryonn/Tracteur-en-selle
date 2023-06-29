using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Offset
{
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;

    public Offset(Vector3 pos, Vector3 rot, Vector3 scl)
    {
        position = pos;
        rotation = rot;
        scale = scl;
    }

    public Offset(Transform trans)
    {
        position = trans.position;
        rotation = trans.rotation.eulerAngles;
        scale = Vector3.one;
    }

    public void SetOffset(Transform _object, Space space = Space.Self, bool ignoreScale = false)
    {
        if (space == Space.World)
        {
            _object.position = position;
            _object.rotation = Quaternion.Euler(rotation);
            if (!ignoreScale) { _object.localScale = scale; }
            
        }
        else
        {
            _object.localPosition = position;
            _object.localRotation = Quaternion.Euler(rotation);
            if (!ignoreScale) { _object.localScale = scale; }
        }
        
    }

}