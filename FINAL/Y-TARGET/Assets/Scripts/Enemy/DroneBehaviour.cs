using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBehaviour : Character
{
    public GameObject Eyes;


    private void Update()
    {
        Ray flightRay = new(Eyes.transform.position, Eyes.transform.forward);
    }
}
