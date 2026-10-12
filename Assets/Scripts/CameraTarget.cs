using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public LochMovement loch;
    Vector3 pos = new Vector3();
    public Vector3 offset = new Vector3();
    private void Start()
    {
        pos = loch.transform.position;
    }

    private void Update()
    {
        pos.x = loch.transform.position.x + offset.x;
        pos.z = loch.transform.position.z + offset.z;
        if(!loch.isJump)
            pos.y = loch.transform.position.y + offset.y;
        transform.position = pos;
    }
}
