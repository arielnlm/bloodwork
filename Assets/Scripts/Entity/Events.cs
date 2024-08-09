using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : ScriptableObject
{
    public Action<float> OnMove;
    public Action<KeyState> OnJump;
}
