using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerBehaviour : MonoBehaviour
{
    public float Xlimit = 3.8f;

    [SerializeField] TextMeshProUGUI PlayerLivesGUI;

    public KeyCode ControlRight;
    public KeyCode ControlLeft;
    public KeyCode Fire;
    public KeyCode Dodge;
    private float _hurrySpeed = 4;
    public int PlayerHP = 3;

    public GameObject CurrentBullet;

    private bool _isShooting;
    private bool _isDodging;

    void Start()
    {
        PlayerLivesGUI.text = "Remaining Lives: " + PlayerHP;
    }

    
    void Update()
    {
        if (Input.GetKey(ControlLeft) && transform.position.x > -Xlimit)
        {
            transform.position -= new Vector3(GameBehaviour.Instance.playerSpeed, 0, 0) * Time.deltaTime;
        }
        else if (Input.GetKey(ControlRight) && transform.position.x < Xlimit)
        {
            transform.position += new Vector3(GameBehaviour.Instance.playerSpeed, 0, 0) * Time.deltaTime;
        }

        if (Input.GetKeyDown(Fire))
        {
            Instantiate(CurrentBullet, this.transform.position + new Vector3(0, 1, 0), this.transform.rotation);
   
        }

        if (Input.GetKeyDown(Dodge) && !_isDodging)
        {
            StartCoroutine(Dodging());
            _isDodging = true;
        }

        if (PlayerHP < 1)
        {
            Debug.Log("You Died");
            Destroy(this.transform.gameObject);
        }

    }

    IEnumerator Dodging()
    {
        GameBehaviour.Instance.playerSpeed += _hurrySpeed;
        yield return new WaitForSeconds(0.2f);
        _isDodging = false;
        GameBehaviour.Instance.playerSpeed -= _hurrySpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_isDodging)
        {
            Debug.Log("Ouch!");
            PlayerHP -= 1;
            PlayerLivesGUI.text = "Remaining Lives: " + PlayerHP;
        }
        else
        {
            Debug.Log("Dodged");
        }
    }

}
