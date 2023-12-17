using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Old_EnemyBehaviour : MonoBehaviour
{

    private CharacterController characterController;
    private CapsuleCollider _col;
    private GameObject _player;
    private Vector3 _playerLocation;

    public float hp=10;
    public float maxHp = 10;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    private float BulletSpeed = 100f;
    private float rotationSpeed = 200f;
    private Quaternion AimRotation;

    private readonly float _sphereRadius = 0.75f;
    private float _obstacleRange = 5.0f;
    public GameObject _bullet;
    private GameObject _newBullet;
    public GameObject Eyes;
    private Vector3 _target;
    private Transform LockLook;

    public Transform PatrolRoute;
    public List<Transform> Locations;
    private int _locationIndex = 0;
    private NavMeshAgent _agent;
    private FloatingHealthBarBehaviour _healthBar;


    private float movementDirectionY;
    private float rot = 0;

    public bool canMove = true;
    public bool _hasPeaked = false;
    public bool _isJumping = false;
    public bool _isShooting = false;

    //Circling----------------------------------
    private float _rotY;
    private Vector3 _offset;
    private float _rotSpeed = 24f;



    public Vector3 moveDirection = Vector3.zero;



    void Start()
    {
        _healthBar = GetComponentInChildren<FloatingHealthBarBehaviour>();
        characterController = GetComponent<CharacterController>();
        _col = GetComponent<CapsuleCollider>();
        _player = GameObject.Find("Player");
        _agent = GetComponent<NavMeshAgent>();
        //InitializePatrolRoute();
        //MoveToNextPatrolLocaton();
    }

    void Update()
    {
        //if (_agent.remainingDistance < 0.2f && !_agent.pathPending)
        //{
        //    MoveToNextPatrolLocaton();
        //}
        //_playerLocation = _player.transform.position;
        //Debug.Log($"Player is at {_playerLocation}");

        //moveDirection -= this.transform.position - _playerLocation.normalized*Time.deltaTime;
        //Debug.Log($"This Way {moveDirection}");
        //Debug.Log($"I'm at {this.transform.position}");

        //if(characterController.isGrounded)
        //{
        //    Jump();
        //}

        if (hp<1)
        {
            Destroy(this.gameObject);
        }

        //Roomba Movement------------------------------------

        //if ((transform.position - _player.transform.position).magnitude > 20f)
        //{
        //    transform.Translate(0, 0, runSpeed * Time.deltaTime);
        //    //Debug.Log("Chasing");
        //}
        //else
        //{
        //    //transform.Translate(0, 0, 0);
        //    Eyes.transform.LookAt(_player.transform.position);
        //    Debug.Log($"{_player.transform.position}");
        //    //Debug.Log("Not Chasing");
        //}

        //Eyes Rotation-------------------------------------------

        //Eyes.transform.LookAt(_player.transform.position);

        //transform.LookAt(_player.transform.position);
        //Vector3 eulerAngles = transform.eulerAngles;
        //eulerAngles.x = 0f;
        //eulerAngles.z = 0f;
        //transform.eulerAngles = eulerAngles;

        
        //Moving---------------------------


        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //Vector3 right = transform.TransformDirection(Vector3.right);

        //bool isRunning = Input.GetKey(KeyCode.LeftShift);
        //float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * 0 : 0;
        //float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * 0 : 0;
        //movementDirectionY = moveDirection.y;
        //moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        //--------------------------
        //Jumping---------------------------
        if (characterController.velocity.y < jumpPower)
        {
            _hasPeaked = true;
        }

        if (!characterController.isGrounded)
        {
            //float gravMult = _hasPeaked ? Mathf.Clamp(jumpPower / 10, 4, 19) : 1;
            float gravMult = 20;
            float tempGrav = gravity * gravMult;
            //Debug.Log($"Gravity: {tempGrav}");
            moveDirection.y -= tempGrav * Time.deltaTime;
        }
        else
        {
            _hasPeaked = false;
        }
        //-------------------------------------


        //if (characterController.isGrounded)
        //{
        //    Debug.Log("Grounded");
        //}
        //else
        //{
        //    Debug.Log("Not Grounded");
        //}

        //if(_agent.enabled==false)
        //{
        //    characterController.Move(moveDirection * Time.deltaTime);
        //    Debug.Log($"{moveDirection}");
        //}

        PlayerWatch();
        //Shoooting-------------------------------------------------

        Ray ray = new(Eyes.transform.position, Eyes.transform.forward);

        if (Physics.SphereCast(ray, _sphereRadius, out RaycastHit hit))
        {
            GameObject hitObj = hit.transform.gameObject;


            if (hitObj.GetComponent<CharacterController>())
            {
                _target = hitObj.transform.position;
                //Debug.Log("Hey!");

                if (!_isShooting)
                {
                    StartCoroutine(Shoot());
                    _isShooting = true;
                }

            }
            else
            {
                //Debug.Log($"{hitObj.name}");
                //Eyes.transform.LookAt(_player.transform.position);
            }

            //transform.rotation = Quaternion.Euler(transform.rotation.x, Eyes.transform.rotation.y, transform.rotation.z);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= 1;
            _healthBar.UpdateHealthBar(hp, maxHp);
            Debug.Log("ouch");
        }
    }

    void Jump()
    {
        Debug.Log("Jump");
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            //Debug.Log($"Jumping Power: {jumpPower}");
            _isJumping = true;
        }
        //else
        //{
        //    moveDirection.y = movementDirectionY;
        //}
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    //void MoveToNextPatrolLocaton()
    //{
    //    if (Locations.Count == 0)
    //        return;

    //    _agent.destination = Locations[_locationIndex].position;

    //    _locationIndex = (_locationIndex + 1) % Locations.Count;
    //}

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "Player")
        {
            if ((transform.position - _player.transform.position).magnitude > 40f)
            {
                _agent.enabled = true;
                //_agent.enabled = true;
                _agent.destination = _player.transform.position;
            }
            else
            {
                //_agent.destination = _player.transform.position + Vector3.right * 4;

                _agent.destination = _agent.transform.position;
                //_agent.enabled = false;
                //Circling();

            }
        }
    }

    private IEnumerator Shoot()
    {
        for (int i = 0; i <= 10; i++)
        {
            _newBullet = Instantiate(_bullet, Eyes.transform.position + (transform.forward * 4), Quaternion.Euler(0, 0, 0), Eyes.transform);

            Rigidbody BulletRB = _newBullet.GetComponent<Rigidbody>();

            BulletRB.velocity = (_player.transform.position - (Eyes.transform.position + (transform.forward * 4))).normalized * BulletSpeed;

            yield return new WaitForSeconds(0.1f);

        }
        yield return new WaitForSeconds(0.5f);
        _isShooting = false;
    }

    private void Circling()
    {
        //Attempt #1


        ////needs to be moved
        //_rotY = transform.eulerAngles.y;
        //_offset = new Vector3(0, 0, 15);
        ////_offset = _player.transform.position - transform.position;

        //_rotY += _rotSpeed * Time.deltaTime;

        //Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        //Debug.Log($"{_rotY}");

        ////moveDirection = (((_player.transform.position - (rotation * _offset))-new Vector3(transform.position.x,0, transform.position.z)).normalized)* runSpeed * Time.deltaTime;
        //_agent.destination = (_player.transform.position - (rotation * _offset)) - new Vector3(transform.position.x, 0, transform.position.z);


        //Attempt #2

        //_offset = new Vector3(0, 0, 20);
        //_rotY -= _rotSpeed * Time.deltaTime;

        //Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        //_agent.destination = _player.transform.position - (rotation * _offset);
        //Debug.Log("Circling");


        //Attempt #3

        Vector3 directionToPlayer = _player.transform.position - transform.position;

        // Calculate a perpendicular direction (rotate 90 degrees around up vector)
        Vector3 circleDirection = Vector3.Cross(directionToPlayer.normalized, Vector3.up);

        // Rotate towards the circle direction
        Quaternion targetRotation = Quaternion.LookRotation(circleDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _rotSpeed);

        // Move forward (or apply any movement logic)
        transform.Translate(Vector3.forward * Time.deltaTime * _rotSpeed);
    }

    private void PlayerWatch()
    {
        Eyes.transform.LookAt(_player.transform.position);

        transform.LookAt(_player.transform.position);
        Vector3 eulerAngles = transform.eulerAngles;
        eulerAngles.x = 0f;
        eulerAngles.z = 0f;
        transform.eulerAngles = eulerAngles;
    }
}
