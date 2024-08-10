using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : Controller
{
    private bool _jumpKey = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Input.GetAxisRaw("Horizontal");
        _entity.Events.OnMove?.Invoke(direction);

    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpKey = true;
            _entity.Events.OnJump?.Invoke(KeyState.PRESSED);
            return;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _jumpKey = false;
            _entity.Events.OnJump?.Invoke(KeyState.RELEASED);
            return;
        }

        if (_jumpKey)
        {
            _entity.Events.OnJump?.Invoke(KeyState.HELD);
        }
    }
}
