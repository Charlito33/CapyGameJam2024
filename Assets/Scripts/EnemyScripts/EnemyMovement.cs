using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyMovement : MonoBehaviour
{

    private enum pathMode
    {
        BackAndForth,
        BackToStart,
        StopAtEnd
    }

    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    [SerializeField] public int nextEndPointIndex = 1;
    private bool _movingForward = true;
    private bool _movementBlocked;
    private float _timeSinceBlocked;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float cooldownTime;
    [SerializeField] private pathMode mode;
    [SerializeField] public List<Vector2> endPoints;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        movementSpeed = 500f;
        cooldownTime = 0.5f;
        endPoints[0] = new Vector2(transform.position.x, transform.position.y);
        if (endPoints.Count == 1)
        {
            _movementBlocked = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_movementBlocked)
        {
            if (nextEndPointIndex != endPoints.Count || mode != pathMode.StopAtEnd)
            {
                UpdateBlockedMovement();
            }
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), endPoints[nextEndPointIndex]) < 0.1f)
        {
            UpdateEndPointIndex();
            _movementBlocked = true;
            return;
        }
        var h = endPoints[nextEndPointIndex].x - transform.position.x;
        var v = endPoints[nextEndPointIndex].y - transform.position.y;
        _movementInput = new Vector2(h, v).normalized;
        _rb.linearVelocity = _movementInput * (Time.fixedDeltaTime * movementSpeed);
    }

    private void UpdateEndPointIndex()
    {
        if (nextEndPointIndex == 0 && !_movingForward)
        {
            _movingForward = true;
            nextEndPointIndex++;
        }
        else if (nextEndPointIndex == endPoints.Count - 1)
        {
            switch (mode)
            {
                case pathMode.BackToStart:
                    nextEndPointIndex = 0;
                    break;
                case pathMode.BackAndForth:
                    _movingForward = false;
                    nextEndPointIndex--;
                    break;
                case pathMode.StopAtEnd:
                    _movementBlocked = true;
                    nextEndPointIndex++;
                    break;
                default:
                    Debug.LogError("Invalid path mode");
                    break;
            }
        }
        else
        {
            if (_movingForward)
            {
                nextEndPointIndex++;
            }
            else
            {
                nextEndPointIndex--;
            }
        }
    }

    private void UpdateBlockedMovement()
    {
        if (_timeSinceBlocked >= cooldownTime)
        {
            _movementBlocked = false;
            _timeSinceBlocked = 0;
        }
        else
        {
            _timeSinceBlocked += Time.fixedDeltaTime;
        }
    }
}
