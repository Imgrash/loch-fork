using Scripts.MovingThings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody)), RequireComponent(typeof(Animator)), RequireComponent(typeof(Transform))]
public class BaseVariables : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public new Transform transform;
    [HideInInspector]
    public Rigidbody rb;
    private void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }
}
