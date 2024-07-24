using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private GameObject _bulletGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Player hit");
            _bulletGameObject.SetActive(false);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstruction"))
        {
            Debug.Log("Obstruction hit");
            _bulletGameObject.SetActive(false);
        }
    }
}
