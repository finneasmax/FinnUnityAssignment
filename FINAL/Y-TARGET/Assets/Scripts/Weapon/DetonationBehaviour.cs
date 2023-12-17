using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetonationBehaviour : MonoBehaviour
{
    public float size = 100f;
    public float sizeFactor = 1f;
    private float destroyDelay = 0.2f;
    private float intesity = 2;
    public bool _addImpulse = false;
    public GameObject user;

    //private void Awake()
    //{
    //    growSpeed *= sizeFactor;
    //}

    void Update()
    {
        transform.localScale += new Vector3(size, size, size) * Time.deltaTime;
        Destroy(this.gameObject, destroyDelay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer==10 && other.gameObject!=user && _addImpulse)
        {
            //CharacterController character = other.gameObject.GetComponent<CharacterController>();
            //character.Move(((other.transform.position - transform.position).normalized * 40));
            //Debug.Log("Moved");
            ////Debug.Log($"Moved");
            ///

            Character character = other.gameObject.GetComponent<Character>();
            if(character!=null)
            {
                character.moveDirection = ((other.transform.position - transform.position).normalized + Vector3.up) * 30;
                PlayerBehaviour player = other.gameObject.GetComponent<PlayerBehaviour>();
                if(player!=null)
                {
                    player.SendDamage(1);
                    Debug.Log("Player Hit");

                }
                else
                {
                    character.SendDamage(1);
                    Debug.Log("Character Hit");
                }
            }
        }
    }
}
