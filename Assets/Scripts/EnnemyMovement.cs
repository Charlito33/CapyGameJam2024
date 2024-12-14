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
    private int _endPointIndex = 1;
    private bool _movingForward = true;
    [SerializeField] private float movementSpeed;
    [SerializeField] private int waitTimeMs;
    [SerializeField] private pathMode mode;
    [SerializeField] private List<Vector2> endPoints;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        movementSpeed = 100f;
        waitTimeMs = 0;
        endPoints[0] = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), endPoints[_endPointIndex]) < 0.1f)
        {
            UpdateEndPointIndex();
        }
        var h = endPoints[_endPointIndex].x - transform.position.x;
        var v = endPoints[_endPointIndex].y - transform.position.y;
        _movementInput = new Vector2(h, v).normalized;
        _rb.linearVelocity = _movementInput * (Time.fixedDeltaTime * movementSpeed);
    }

    private void UpdateEndPointIndex()
    {
        if (_endPointIndex == 0 && !_movingForward)
        {
            _movingForward = true;
            _endPointIndex++;
        }
        else if (_endPointIndex == endPoints.Count - 1)
        {
            switch (mode)
            {
                case pathMode.BackToStart:
                    _endPointIndex = 0;
                    break;
                case pathMode.BackAndForth:
                    _movingForward = false;
                    _endPointIndex--;
                    break;
                case pathMode.StopAtEnd:
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
                _endPointIndex++;
            }
            else
            {
                _endPointIndex--;
            }
        }
        Thread.Sleep(waitTimeMs);
    }
}
