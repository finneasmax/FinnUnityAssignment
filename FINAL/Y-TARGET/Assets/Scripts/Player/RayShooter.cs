using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RayShooter : MonoBehaviour
{
    private GameObject _mainCam;
    private Camera _cam;
    private CharacterController characterController;

    private bool _isShooting;
    public GameObject Bullet;
    public float BulletSpeed = 200f;

    private Vector3 _target;
    private RaycastHit hit;

    private PlayerBehaviour _playerBehaviour;
    //private TextMeshProUGUI _crossHair;

    private void Start()
    {
        //_crossHair = GameObject.Find("CrossHair").GetComponent<TextMeshProUGUI>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _mainCam = GameObject.Find("Main Camera");
        _cam = _mainCam.GetComponent<Camera>();
        characterController = GetComponent<CharacterController>();

        // Lock cursor to the middle of the screen and hide it.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //_crossHair.transform.position = new Vector3((Screen.width / 2)-_crossHair.renderedWidth, (Screen.height / 2)-_crossHair.renderedHeight, 0);
        Vector3 point = new(_cam.pixelWidth / 2, _cam.pixelHeight / 2, 0);
        Ray ray = _cam.ScreenPointToRay(point);
        _target = _cam.ScreenToWorldPoint(point);
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

        if (Input.GetMouseButtonDown(1))
        {

            if (Physics.Raycast(ray, out hit))
            {
                //_target = hit.point;
                GameObject hitObj = hit.transform.gameObject;
                ReactiveTarget target = hitObj.GetComponent<ReactiveTarget>();

                if (target != null)
                    target.ReactToHit();
                else
                    StartCoroutine(SphereIndicator(hit.point));
            }
        }

        _isShooting |= Input.GetMouseButtonDown(0);

        if (_isShooting && _playerBehaviour.isAlive)
        {

            GameObject newBullet = Instantiate(Bullet, _mainCam.transform.position + (_mainCam.transform.forward*2), Quaternion.Euler(0, 0, 0));

            Rigidbody BulletRB = newBullet.GetComponent<Rigidbody>();

            BulletRB.velocity = ((_mainCam.transform.position + (_mainCam.transform.forward*2)) - _target).normalized * BulletSpeed;
        }
        _isShooting = false;
    }

    //// OnGUI runs after the scene has been rendered.
    //void OnGUI()
    //{
    //    // Size of the rectangular GUI that will contain the text.
    //    int size = 12;

    //    // Position of the text. Note that subtracting the scaled size will
    //    // ensure that the star is centered.
    //    float posX = _cam.pixelWidth / 2 - size / 4;
    //    float posY = _cam.pixelHeight / 2 - size / 2;

    //    // Change the color of the GUI's contents to red.
    //    GUI.contentColor = Color.red;

    //    // Render a label that defines a position and the text it contains.
    //    GUI.Label(new Rect(posX, posY, size, size), "*");
    //}

    IEnumerator SphereIndicator(Vector3 pos)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = pos;

        yield return new WaitForSeconds(1);

        //Destroy(sphere);
    }

    private void targetRotation()
    {
        //Special thanks to ChatGPT...
        Vector3 direction = _target - this.transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, -angle);
        this.transform.rotation = rotation;
        //Debug.Log("Targeting...");
    }
}
