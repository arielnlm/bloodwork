using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public new string name; //new because ScriptableObject already has name
    public float cooldownTime;
    public float activeTime;
    protected AbilityState abilityState = AbilityState.ready;

    private void OnEnable()
    {
        abilityState = AbilityState.ready;
    }

    public abstract void Use(GameObject parent);

    protected abstract void Ready(GameObject parent);
    protected abstract void Active(GameObject parent);
    protected abstract void CoolDown();

    
    protected enum AbilityState
    {
        ready,
        active,
        cooldown
    }
}
