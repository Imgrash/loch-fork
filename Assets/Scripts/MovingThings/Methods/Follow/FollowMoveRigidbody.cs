using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FollowMoveRigidbody : FollowBase
{
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override void Follow()
    {
        rb.MovePosition(toFollow.position);
        rb.MoveRotation(toFollow.rotation);
    }
}
