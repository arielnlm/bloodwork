using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    public Ability ability;
    public float cooldownTime;
    public float activeTime;
    public KeyCode key;

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            //Debug.Log("Current speed: " + gameObject.GetComponent<PlayerMovement>().CurrentSpeed);
            ability.Use(gameObject);
        }
    }
}
