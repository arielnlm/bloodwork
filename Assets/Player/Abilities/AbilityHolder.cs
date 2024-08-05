using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    public Ability ability;
    public float cooldownTime;
    public float activeTime;
    public KeyCode key;

    
    private AbilityState abilityState = AbilityState.ready;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (abilityState) 
        {
            case AbilityState.ready:
                if (Input.GetKeyDown(key))
                {
                    Debug.Log("Current Speed: " + gameObject.GetComponent<PlayerMovement>().CurrentSpeed);
                    ability.Activate(gameObject);
                    abilityState = AbilityState.active;
                    activeTime = ability.activeTime;
                }
            break;
            case AbilityState.active:
                if (activeTime < 0f)
                {
                    abilityState = AbilityState.cooldown;
                    cooldownTime = ability.cooldownTime;
                }
                else
                    activeTime -= Time.deltaTime;
                break;
            case AbilityState.cooldown:
                if (cooldownTime < 0f)
                    abilityState = AbilityState.ready;
                else 
                    cooldownTime -= Time.deltaTime;

                break;
        }
    }



    enum AbilityState
    {
        ready,
        active,
        cooldown
    }
}
