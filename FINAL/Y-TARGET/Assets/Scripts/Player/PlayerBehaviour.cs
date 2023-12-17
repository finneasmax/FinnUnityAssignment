using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : Character
{
    public GameObject _mainCam;
    private Camera _cam;
    public HealthBarBehaviour _healthBar;

    private bool _isShooting;

    private Vector3 _target;
    private RaycastHit hit;

    public override void Awake()
    {
        base.Awake();
        //_healthBar=GetHealthBar();
        _mainCam = GameObject.Find("Main Camera");
        _cam = _mainCam.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        if (_hp < 1)
        {
            isAlive = false;
            GameBehaviour.Instance.CheckWinner();
        }
        //Moving---------------------------------------------------
        canMove = isAlive ? true : false;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedZ = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedZ) + (right * curSpeedX);
        moveDirection = Vector3.ClampMagnitude(moveDirection, (isRunning ? runSpeed : walkSpeed));


        //Jumping-----------------------------------------------------
        if (Input.GetKeyDown(KeyCode.Space) && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            //Debug.Log("Jump!");
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        GravityApplication();

        //Finally Applying Movement---------------------------------------
        characterController.Move(moveDirection * Time.deltaTime);
        //Debug.Log($"Player {moveDirection}");


        //Aiming/Looking---------------------------------------------
        rotantionX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotantionX = Mathf.Clamp(rotantionX, -lookXlimit, lookXlimit);
        _cam.transform.localRotation = Quaternion.Euler(rotantionX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);


        //Shooting--------------------------------------------------
        Vector3 point = new(_cam.pixelWidth / 2, _cam.pixelHeight / 2, 0);
        //Ray ray = _cam.ScreenPointToRay(point);
        Vector3 bulletOrigin = _mainCam.transform.position + (_mainCam.transform.forward * 3);
        Vector3 targetPoint = _cam.ScreenToWorldPoint(point);
        Ray ray = new Ray(bulletOrigin,(bulletOrigin-targetPoint).normalized);

        //_target = _cam.ScreenToWorldPoint(point);
        //if (Physics.Raycast(ray, out hit))
        //{
        //    _target = hit.transform.position;
        //}

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObj = hit.transform.gameObject;
            if (hitObj.GetComponent<CharacterController>())
            {
                //Debug.Log("Gotcha");

            }
        }

        //if (Input.GetMouseButtonDown(1))
        //{

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        GameObject hitObj = hit.transform.gameObject;
        //        ReactiveTarget target = hitObj.GetComponent<ReactiveTarget>();

        //        if (target != null)
        //            target.ReactToHit();
        //        else
        //            StartCoroutine(SphereIndicator(hit.point));
        //    }
        //}

        _isShooting |= Input.GetMouseButtonDown(0);

        if (_isShooting && isAlive)
        {
            //if (Physics.Raycast(ray, out hit))
            //{
            //    _target = _cam.ScreenToWorldPoint(point);
            //}
            _target = hit.point;
            //Debug.Log($"Clicked On : {hit.transform.gameObject.name}");
            _weapon.Use(_target,_mainCam.transform,this.gameObject);
        }
        _isShooting = false;

        Debug.DrawLine(_mainCam.transform.position, _target, Color.red);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //ApplyImpulse(transform.forward, 50);
        if (collision.gameObject.layer==8)
        {
            BulletBehaviour bulletBehaviour = collision.gameObject.GetComponent<BulletBehaviour>();

            if (bulletBehaviour.user != this.gameObject)
            {
                SendDamage(1);
            }
        }
    }

    IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        //Destroy(sphere);
    }

    public override void SendDamage(int damage)
    {
        _hp -= damage;
        _healthBar.UpdateHealthBar(_hp, _maxHp);
        Debug.Log($"{_hp}");
    }
}
