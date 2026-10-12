using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FollowBase : MonoBehaviour, IFollow
{
    public Transform toFollow;
    public abstract void Follow();

    // Update is called once per frame
    void FixedUpdate()
    {
        Follow();
    }
}
