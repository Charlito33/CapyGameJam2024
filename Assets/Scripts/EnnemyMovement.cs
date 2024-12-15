using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnnemyMovement : MonoBehaviour
{

    private enum pathMode
    {
        BackAndForth,
        BackToStart,
        StopAtEnd
    }

    private Rigidbody2D _rb;
    private Vector2 _movementInput;
    private int _nextEndPointIndex = 1;
    private bool _movingForward = true;
    private bool _movementBlocked;
    private float _timeSinceBlocked;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float cooldownTime;
    [SerializeField] private pathMode mode;
    [SerializeField] private List<Vector2> endPoints;
 
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
            if (_nextEndPointIndex != endPoints.Count || mode != pathMode.StopAtEnd)
            {
                UpdateBlockedMovement();
            }
            _rb.linearVelocity = Vector2.zero;
            return;
        }
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), endPoints[_nextEndPointIndex]) < 0.1f)
        {
            UpdateEndPointIndex();
            _movementBlocked = true;
            return;
        }
        var h = endPoints[_nextEndPointIndex].x - transform.position.x;
        var v = endPoints[_nextEndPointIndex].y - transform.position.y;
        _movementInput = new Vector2(h, v).normalized;
        _rb.linearVelocity = _movementInput * (Time.fixedDeltaTime * movementSpeed);
    }

    private void UpdateEndPointIndex()
    {
        if (_nextEndPointIndex == 0 && !_movingForward)
        {
            _movingForward = true;
            _nextEndPointIndex++;
        }
        else if (_nextEndPointIndex == endPoints.Count - 1)
        {
            switch (mode)
            {
                case pathMode.BackToStart:
                    _nextEndPointIndex = 0;
                    break;
                case pathMode.BackAndForth:
                    _movingForward = false;
                    _nextEndPointIndex--;
                    break;
                case pathMode.StopAtEnd:
                    _movementBlocked = true;
                    _nextEndPointIndex++;
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
                _nextEndPointIndex++;
            }
            else
            {
                _nextEndPointIndex--;
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
