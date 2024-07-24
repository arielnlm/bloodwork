using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Range(0, 50)][SerializeField] private float _radius = 5f;
    [Range(0, 360)][SerializeField] private float _defaultAngle = 45f;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private LayerMask _obstructionLayer;
    private GameObject _playerRef;

    public bool directionRight = true;

    private float angle;

    public bool canSeePLayer {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        angle = _defaultAngle;
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVCheckForPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator FOVCheckForPlayer()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f); //check every 0.2 sec

        while (true)
        {
            yield return wait; //sleep 0.2
            FOV();
        }
    }

    private void FOV()
    {
        Collider2D[] rangeCollisionCheck = Physics2D.OverlapCircleAll(this.transform.position, _radius, _targetLayer);

        if (rangeCollisionCheck.Length > 0 )
        {
            Transform playerTrans = rangeCollisionCheck[0].transform;
            Vector2 directionToTarget = (playerTrans.position - transform.position).normalized;


            if (FindAngle(directionToTarget))
            {
                float distanceToTarget = Vector2.Distance(transform.position, playerTrans.position);

                //Check if its hitting an object 
                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionLayer))
                {
                    canSeePLayer = true;
                }
                else
                {
                    canSeePLayer = false;
                }
            }
            else
            {
                canSeePLayer = false;
            }
        }
        else if (canSeePLayer)
        {
            canSeePLayer = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, angle / 2);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * _radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * _radius);

        if (canSeePLayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _playerRef.transform.position);
        }
    }

    public void ChaseMode()
    {
        angle = 360;
    }

    public void SearchMode()
    {
        angle = _defaultAngle;
    }

    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        int num = directionRight ? 1 : -1;

        return new Vector2(num * Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), num * Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
    }

    private bool FindAngle(Vector2 directionToTarget)
    {
        int num = directionRight ? 1 : -1;
        return Vector2.Angle(num * transform.right, directionToTarget) < angle / 2;
    }
}
