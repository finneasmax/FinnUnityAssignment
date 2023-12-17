using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int _hp;
    public int _maxHp;
    public Weapon _weapon;
    public Weapon _weapon2;
    public Weapon _weapon3;
    public bool isAlive = true;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float tempGrav;



    public float DistanceToGround = 0.1f;

    public float lookSpeed = 2f;
    public float lookXlimit = 90f;

    public Vector3 moveDirection = Vector3.zero;
    public float rotantionX = 0;

    public bool canMove = true;
    public bool _hasPeaked = false;
    public bool _isJumping = false;
    public bool _isFlying = false;

    public CharacterController characterController;
    public CapsuleCollider _col;


    public virtual void Awake()
    {

        characterController = this.GetComponent<CharacterController>();
        _col = GetComponent<CapsuleCollider>();
    }

    //public virtual void Update()
    //{
    //    if (!characterController.isGrounded)
    //    {
    //        //moveDirection.y -= gravity * Time.deltaTime;

    //        if (characterController.velocity.y < jumpPower)
    //        {
    //            _hasPeaked = true;
    //        }

    //        float gravMult = _hasPeaked ? Mathf.Clamp(jumpPower / 10, 4, 19) : 1;
    //        float tempGrav = gravity * gravMult;
    //        //Debug.Log($"{gravMult}");
    //        moveDirection.y -= tempGrav * Time.deltaTime;
    //    }
    //    else
    //    {
    //        _hasPeaked = false;
    //    }
    //}

    public virtual void GravityApplication()
    {
        if (!characterController.isGrounded)
        {
            //moveDirection.y -= gravity * Time.deltaTime;

            if (characterController.velocity.y < jumpPower)
            {
                _hasPeaked = true;
            }

            float gravMult = _hasPeaked ? Mathf.Clamp(jumpPower / 10, 4, 19) : 1;
            float tempGrav = gravity * gravMult;
            //Debug.Log($"Jump Adjusted");
            moveDirection.y -= tempGrav * Time.deltaTime;
        }
        else
        {
            _hasPeaked = false;
        }
        //Debug.Log($"{_hasPeaked}");
    }

    public virtual HealthBarBehaviour GetHealthBar()
    {
        HealthBarBehaviour healthBar = GetComponentInChildren<HealthBarBehaviour>();
        return healthBar;
    }

    public virtual FloatingHealthBarBehaviour GetFloatingHealthBar()
    {
        FloatingHealthBarBehaviour healthBar = GetComponentInChildren<FloatingHealthBarBehaviour>();
        return healthBar;
    }

    public virtual void ApplyImpulse(Vector3 impulseSource, float strength)
    {
        Vector3 impulseDirection = (transform.position - impulseSource).normalized;

        impulseDirection.y = 1;
        moveDirection = impulseDirection * strength;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer==11)
        {
            ApplyImpulse(hit.gameObject.transform.position,50);
        }
    }

    public virtual void SendDamage(int damage)
    {
        _hp -= damage;
    }
}
