using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBehaviour : MonoBehaviour
{
    public float YLimit = 4.0f;

    public KeyCode UpDirection;
    public KeyCode DownDirection;



    void Start()
    {
        
    }
    void Update()
    {
     if (GameBehaviour.Instance.State== GameBehaviour.GameStates.Play)
        {
            if (Input.GetKey(UpDirection) && transform.position.y < YLimit)
            {
                transform.position += new Vector3(0, GameBehaviour.Instance.paddleSpeed, 0) * Time.deltaTime;
            }
            else if (Input.GetKey(DownDirection) && transform.position.y > -YLimit)
            {
                transform.position -= new Vector3(0, GameBehaviour.Instance.paddleSpeed, 0) * Time.deltaTime;
            }

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameBehaviour.Instance.paddleSpeed += GameBehaviour.Instance.BallSpeedIncrement;
            //Debug.Log($"Paddle Speed {GameBehaviour.Instance.paddleSpeed}");
        }
    }

}
