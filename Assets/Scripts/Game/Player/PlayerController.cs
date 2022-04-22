using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] 
    private float speed;
    [SerializeField] 
    private float jumpForce;
    [SerializeField] 
    private GameObject followTransform;
    [SerializeField] 
    private float aimSensativity = 1;



    private bool isJumping;
    private Vector3 moveDirection = Vector3.zero;
    private Vector2 inputVector = Vector2.zero;
    Vector2 lookInput = Vector2.zero;

    private Rigidbody playerRigidbody;


    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        if (!(inputVector.magnitude > 0))
        {
            moveDirection = Vector3.zero;
        }

        //Rotate player
        transform.Rotate(Vector3.up * inputVector.x * aimSensativity);
        //Move player in forward or backwards
        transform.position += transform.forward * inputVector.y* Time.deltaTime * speed;
    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
    }

    public void OnLookingAround(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

}
