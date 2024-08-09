using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{

    public Events Events { get; private set; }



    private void Awake()
    {
        Events = new Events();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
