using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    //public float jumpSpeed = 1f;
    public float gravity = 10f;
    public float tempGrav;
    private PlayerBehaviour _playerBehaviour;



    public float DistanceToGround = 0.1f;
    public LayerMask GroundLayer;

    public float lookSpeed = 2f;
    public float lookXlimit = 90f;

    Vector3 moveDirection = Vector3.zero;
    float rotantionX = 0;

    public bool canMove = true;
    public bool _hasPeaked = false;
    public bool _isJumping = false;

    private CharacterController characterController;
    private CapsuleCollider _col;



    void Start()
    {
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        characterController=this.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _col = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        canMove = _playerBehaviour.isAlive ? true : false;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedZ) + (right * curSpeedX);
        moveDirection = Vector3.ClampMagnitude(moveDirection, (isRunning ? runSpeed : walkSpeed));
        Debug.Log($"Movement: {moveDirection}");



        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            Debug.Log("Jump!");
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }




        if (!characterController.isGrounded)
        {
            //moveDirection.y -= gravity * Time.deltaTime;

            if (characterController.velocity.y < jumpPower)
            {
                _hasPeaked = true;
            }

            float gravMult = _hasPeaked ? Mathf.Clamp(jumpPower / 10, 4, 19) : 1;
            float tempGrav = gravity * gravMult;
            //Debug.Log($"{gravMult}");
            moveDirection.y -= tempGrav * Time.deltaTime;
        }
        else
        {
            _hasPeaked = false;
        }

        //if(characterController.isGrounded)
        //{
        //    Debug.Log("Grounded");
        //}
        //else
        //{
        //    Debug.Log("Not Grounded");
        //}





        characterController.Move(moveDirection * Time.deltaTime);
        //Debug.Log($"Velocity: {characterController.velocity}");
        //Debug.Log($"Movement: {moveDirection}");
        //Debug.Log($"Is Jumping? : {_isJumping}");


        rotantionX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotantionX = Mathf.Clamp(rotantionX, -lookXlimit, lookXlimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotantionX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);

        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, DistanceToGround, GroundLayer, QueryTriggerInteraction.Ignore);

        return grounded;
    }
}
