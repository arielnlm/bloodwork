using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{

    protected Entity _entity;

    private void Awake()
    {
        _entity = GetComponent<Entity>();
    }
}
