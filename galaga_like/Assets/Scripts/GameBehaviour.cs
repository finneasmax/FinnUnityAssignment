using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    public static GameBehaviour Instance;
    public float playerSpeed= 4f;
    public float BulletSpeed = 4f;

    public float GameBounds = 4.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
