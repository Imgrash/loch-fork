using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LochWalk : MonoBehaviour
{
    private Animator animator;
    private GameObject interactedObject;
    public GameObject Mark;

    private Rigidbody rb;
    public float speed = 100;
    private Vector3 moveVector
    {       get
        {
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }

    public Vector3 jump;
    public float jumpForce = 1;

    private bool _isGrounded;

    private bool GetGrounded()
    {
            var bottomCenterPoint = new Vector3(_collider.bounds.center.x, _collider.bounds.min.y, _collider.bounds.center.z);

            //создаем невидимую физическую капсулу и проверяем не пересекает ли она обьект который относится к полу

            //_collider.bounds.size.x / 2 * 0.9f -- эта странная конструкция берет радиус обьекта.
            // был бы обязательно сферой -- брался бы радиус напрямую, а так пишем по-универсальнее

            return Physics.CheckCapsule(_collider.bounds.center, bottomCenterPoint, _collider.bounds.size.x / 2 * 1.0f, GroundLayer);
            // если можно будет прыгать в воздухе, то нужно будет изменить коэфициент 0.9 на меньший.
    }

    public LayerMask GroundLayer;
    private CapsuleCollider _collider;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        //rb.GetComponent<RectTransform>().pivot = new Vector2(0, 0);  //Центрируйте ось вращения
    }

    void FixedUpdate()
    {
        //moveVector.x = Input.GetAxis("Horizontal");
        //moveVector.z = Input.GetAxis("Vertical");
        //if (moveVector)
        //{ 
            rb.AddRelativeForce(moveVector * speed, ForceMode.Impulse);
            animator.SetBool("walk", true);
        //}
        //rb.transform.rotation.y = moveVector;
        MayIJump();
        Debug.Log(moveVector * speed);
        //rb.transform.rotation.y = moveVector;
        //MayIJump();
        //if (moveVector.x > 0)
        //    RotateHim(5);
        //if (moveVector.x < 0)
        //    RotateHim(-5);
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.F) && interactedObject != null)
        {
            //interactedObject.GetComponent<IngredientScript>().WhenPressed();
            Debug.Log("взаимодействие");
            interactedObject = null;
            Mark.SetActive(false);
        }
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        //transform.rotation = Quaternion.LookRotation(moveVector, Camera.main.transform.up);
        

        //transform.RotateAround(rb.position, Camera.main.transform.up, moveVector.x);
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        transform.rotation = Quaternion.LookRotation(moveVector, Camera.main.transform.up);
    }

    private void RotateHim(float angle)
    {
        transform.RotateAround(rb.position, Camera.main.transform.up, moveVector.x);
    }

    private void MayIJump()
    {
        _isGrounded = GetGrounded();
        if (_isGrounded && (Input.GetAxis("Jump") > 0))
        {
             Debug.Log("прыжок");
             rb.AddForce(jump * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        interactedObject = c.gameObject;
        Mark.SetActive(true);
        Debug.Log("F");

    }
    void OnTriggerExit()
    {
        interactedObject = null;
        Mark.SetActive(false);
    }
}
