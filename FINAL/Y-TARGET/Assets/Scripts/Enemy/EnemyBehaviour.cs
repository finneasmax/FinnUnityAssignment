using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : Character
{
    private GameObject _player;
    private Collider _playerCol;
    private Vector3 _playerLocation;

    public GrenadeGun GrenadeGun;

    private float BulletSpeed = 100f;
    private float rotationSpeed = 200f;
    private Quaternion AimRotation;

    private readonly float _sphereRadius = 0.75f;
    private float _obstacleRange = 5.0f;
    public GameObject _bullet;
    private GameObject _newBullet;
    public GameObject Eyes;
    public FloatingHealthBarBehaviour _healthBar;
    private NavMeshAgent _agent;

    public LayerMask layerMask;

    private Vector3 _target;
    private Transform LockLook;

    public Transform PatrolRoute;
    public List<Transform> Locations;
    private int _locationIndex = 0;

    private int _attackedSide = 1;


    private float movementDirectionY;
    private float rot = 0;
    public float currentCheckingAngle = 0;


    public bool _isShooting = false;
    private bool _beingErratic = false;
    private bool _waiting = false;
    private bool _tiredOfWaiting = false;

    //Circling----------------------------------
    private float _rotY;
    private Vector3 _offset;
    private float _rotSpeed = 24f;

    private GameObject _targetCharacter;
    private Vector3 _destination;
    private Vector3 _directionToDestination=Vector3.zero;
    private Vector3 avoidanceDirection;

    enum Action
    {
        GroundChasing,
        Circling,
        PathFinding
    }
    private Action _currentAction;

    enum Attacks
    {
        StandardSnipe,
        GrenadeJump
    }
    private Attacks _currentAttack;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        avoidanceDirection = transform.forward;
        //_healthBar=GetFloatingHealthBar();
        characterController = GetComponent<CharacterController>();
        _col = GetComponent<CapsuleCollider>();
        _player = GameObject.Find("Player");
        _playerCol = _player.GetComponent<Collider>();
        _targetCharacter = _player;
        //InitializePatrolRoute();
        //MoveToNextPatrolLocaton();
        //Debug.Log($"Enemy weapon {_weapon}");
        _currentAction = Action.GroundChasing;
    }

    void Update()
    {
        if (_hp < 1)
        {
            Destroy(this.gameObject);
        }

        //_destination = transform.forward;
        //Debug.Log($"{_destination}");

        //if (!_pathIsBlocked)
        //{
        //    _destination = _targetCharacter.transform.position;
        //}

        PlayerWatch();
        //ProximityDetection();

        movementDirectionY = moveDirection.y;

        _destination = _player.transform.position;

        _directionToDestination = (_destination - transform.position).normalized;



        //if(!_isFlying)
        //{
        //    _directionToTarget.y = 0;
        //}

        //Moving---------------------------


        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //Vector3 right = transform.TransformDirection(Vector3.right);

        //bool isRunning = Input.GetKey(KeyCode.LeftShift);
        //float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * 0 : 0;
        //float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * 0 : 0;
        //movementDirectionY = moveDirection.y;
        //moveDirection = (forward * curSpeedX) + (right * curSpeedY);



        //--------------------------

        //if (_pathIsBlocked)
        //{
        //    PathCheck();
        //}

        //if (_pathIsBlocked)
        //{
        //    PathCheck();
        //}


        if ((_player.transform.position-transform.position).magnitude > 50f && !_isFlying)
        {
            _currentAction = Action.GroundChasing;
        }
        else if ((_player.transform.position - transform.position).magnitude < 50f && !_isFlying)
        {
            _currentAction = Action.Circling;
        }


        if (_currentAction == Action.GroundChasing)
        {
            _agent.enabled = true;
            _agent.updateRotation = false;
            characterController.enabled = false;
            _agent.destination = _player.transform.position;
            //Debug.Log("Chasing");
        }
        else if (_currentAction == Action.Circling)
        {
            _agent.enabled = false;
            characterController.enabled = true;
            Circling();
        }





        if (!characterController.isGrounded && !_isFlying)
        {
            moveDirection.y = movementDirectionY;
        }



        //if (characterController.isGrounded)
        //{
        //    moveDirection.y = jumpPower;
        //    //Debug.Log($"Jumping Power: {jumpPower}");
        //}
        //else
        //{
        //    moveDirection.y = movementDirectionY;
        //}

        moveDirection = Vector3.ClampMagnitude(moveDirection, walkSpeed);

        if (characterController.enabled)
        {
            //Debug.Log("Character Controller");
            GravityApplication();

            characterController.Move(moveDirection * Time.deltaTime * walkSpeed);
            //Debug.Log($"Enemy {moveDirection}");
        }

        //GravityApplication();

        //characterController.Move(moveDirection * Time.deltaTime * walkSpeed);

        //PlayerWatch();

        //Shoooting-------------------------------------------------

        Ray eyesRay = new(Eyes.transform.position, Eyes.transform.forward);

        if (Physics.SphereCast(eyesRay, _sphereRadius, out RaycastHit hit))
        {
            GameObject hitObj = hit.transform.gameObject;
            //Debug.Log($"Hit Name: {hitObj.name}");

            

            if (hitObj.GetComponent<PlayerBehaviour>())
            {
                _target = _player.transform.position;
                //_target = hit.point;

                //if (!_isShooting)
                //{
                //    GrenadeGun.Use(_target, Eyes.transform, this.gameObject);
                //    Debug.Log("grenade");
                //}

                if (!_isShooting)
                {
                    StartCoroutine(ShootStandardGun());
                    _isShooting = true;  
                }
            }
        }
        //Debug.Log($"{_currentAction}");
        //Debug.Log($"Agent {_agent.enabled}");
        //Debug.Log($"Contrller {characterController.enabled}");
        //Debug.Log($"Magnitude {(_player.transform.position - transform.position).magnitude}");
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Bullet"))
        //{
        //    _hp -= 1;
        //    _healthBar.UpdateHealthBar(_hp, _maxHp);
        //    Debug.Log("ouch");
        //}
        if (collision.gameObject.GetComponent<BulletBehaviour>())
        {
            BulletBehaviour bulletBehaviour = collision.gameObject.GetComponent<BulletBehaviour>();

            if (bulletBehaviour.user != this.gameObject)
            {
                _hp -= 1;
                _healthBar.UpdateHealthBar(_hp, _maxHp);
                Debug.Log("ouch");
            }
        }
        //else
        //{
        //    _attackedSide *= -1;
        //    Debug.Log("Wall");
        //    Debug.Log($"{_attackedSide}");
        //}
        
    }


    void Jump()
    {
        //Debug.Log("Jump");
        if (characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in PatrolRoute)
        {
            Locations.Add(child);
        }
    }

    private IEnumerator ShootStandardGun()
    {
        for (int i = 0; i <= 10; i++)
        {
            _weapon.Use(_target, Eyes.transform, this.gameObject);

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        _isShooting = false;
    }

    private IEnumerator ShootGrenadeGun()
    {
        for (int i = 0; i <= 2; i++)
        {
            GrenadeGun.recoil = 0;
            GrenadeGun.Use(_target, Eyes.transform, this.gameObject);

            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(2f);
        _isShooting = false;
    }

    private void Circling()
    {
        if ((_player.transform.position - transform.position).magnitude > 40f)
        {
            moveDirection = (_player.transform.position - transform.position).normalized;
        }
        else
        {
            //Vector3 circleDirection = Vector3.Cross((_player.transform.position - transform.position).normalized, Vector3.up);

            //moveDirection = circleDirection*_attackedSide;

            moveDirection = transform.right * _attackedSide;
        }

        //if ((_player.transform.position - transform.position).magnitude > 30f)
        //{
        //    moveDirection = (_player.transform.position - transform.position).normalized;
        //}
        //else
        //{
        //    moveDirection = Vector3.zero;
        //}
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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<BulletBehaviour>())
        {
            BulletBehaviour bulletBehaviour = other.GetComponent<BulletBehaviour>();

            if (bulletBehaviour.user != this.gameObject && (other.transform.position-transform.position).magnitude<8)
            {
                Vector3 objectDirection = other.ClosestPointOnBounds(transform.position) - transform.position;

                if (Vector3.Dot(transform.right, objectDirection) > 0.000f)
                {
                    Debug.Log("Bullet Right");
                    _attackedSide = -1;
                    moveDirection = transform.right * -1;
                }
                else if (Vector3.Dot(transform.right, objectDirection) < 0.000f)
                {
                    Debug.Log("Bullet Left");
                    _attackedSide = 1;
                    moveDirection = transform.right;
                }
            }
        }
        else if (other.gameObject.CompareTag("Player") && (other.transform.position - transform.position).magnitude < 30)
        {
            if(!_waiting)
            {
                StartCoroutine(Waiting());
                _waiting = true;
            } 
        }


        //if (other.gameObject.layer == 8)
        //{
        //    Vector3 objectDirection = other.ClosestPointOnBounds(transform.position) - transform.position;
        //    if (Vector3.Dot(transform.right, objectDirection) > 0.000f)
        //    {
        //        Debug.Log("Bullet Right");
        //        _attackedSide = -1;
        //        moveDirection = transform.right * -1;
        //    }
        //    else if (Vector3.Dot(transform.right, objectDirection) < 0.000f)
        //    {
        //        Debug.Log("Bullet Left");
        //        _attackedSide = 1;
        //        moveDirection = transform.right;
        //    }
        //    Debug.Log("Bullet");
        //}
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.layer == 9)
        {
            _attackedSide *= -1;
            Debug.Log("Wall");
            Debug.Log($"{_attackedSide}");
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Debug.Log("Collision");
    //    if (other.gameObject.layer==9)
    //    {
    //        _attackedSide *= -1;
    //        Debug.Log("Wall");
    //        Debug.Log($"{_attackedSide}");
    //    }
    //}



    private void ProximityDetection()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 10, layerMask);

        foreach (Collider hits in detectedObjects)
        {
            Debug.DrawLine(transform.position, hits.ClosestPointOnBounds(transform.position), Color.red);
            Vector3 objectDirection = hits.ClosestPointOnBounds(transform.position) - transform.position;

            if (hits.gameObject.layer == 8)
            {
                if (Vector3.Dot(transform.right, objectDirection) > 0.000f)
                {
                    Debug.Log("Bullet Right");
                    _attackedSide = -1;
                }
                else if(Vector3.Dot(transform.right, objectDirection) < 0.000f)
                {
                    Debug.Log("Bullet Left");
                    _attackedSide = 1;
                }
                Debug.Log("Bullet");
            }
        }
        detectedObjects = new Collider[0];
    }


    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 30);
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(Random.Range(2, 20));
        _tiredOfWaiting = true;
        _waiting = false;
    }

    //private void PathFind()
    //{
    //    _pathFinding = true;
    //    if (_player != null)
    //    {

    //        //Vector3 directionToPlayer = _player.transform.position - transform.position;
    //        //directionToPlayer.Normalize();

    //        //float radius = 0;
    //        //float maxDistance = 0;
    //        //Vector3 Origin = transform.position;
    //        //Vector3 Direction = transform.forward;

    //        //// Check for obstacles along the movement direction
    //        //RaycastHit hit;
    //        //if (Physics.SphereCast(Origin, radius, Direction, out hit,maxDistance,layerMask,QueryTriggerInteraction.Ignore))
    //        //{
    //        //    Debug.DrawLine(transform.position, hit.point, Color.red);
    //        //}


    //        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, 30,layerMask) ;

    //        foreach (Collider hits in detectedObjects)
    //        {
    //            //Debug.Log($"{hits.ClosestPointOnBounds(transform.position)-transform.position}");
    //            Debug.DrawLine(transform.position, hits.ClosestPointOnBounds(transform.position), Color.red);
    //            Vector3 obstacleDirection=hits.ClosestPointOnBounds(transform.position) - transform.position;
    //            //obstacleDirection.y = 0;
    //            moveDirection = (obstacleDirection.normalized);
    //            //Debug.DrawLine(transform.position, hits.transform.position,Color.red);  
    //        }
    //        detectedObjects = new Collider[0];
    //    }
    //    _pathFinding = false;
    //}
}

