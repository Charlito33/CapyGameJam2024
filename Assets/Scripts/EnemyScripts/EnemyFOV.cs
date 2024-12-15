using UnityEngine;

public class EnemyFOV : MonoBehaviour
{    
    [SerializeField] private float viewAngle = 90f; // Cone angle
    [SerializeField] private float viewDistance = 5f; // Max distance
    [SerializeField] private Transform player; // Reference to player
    
    private EnemyMovement _enemyMovement;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private LayerMask _obstacleMask; // To detect walls
    private LineRenderer _lineRenderer;

    void Start()
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _rb = GetComponent<Rigidbody2D>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 50; // Number of points in the cone
    }

    void Update()
    {        
        var h = _enemyMovement.endPoints[_enemyMovement.nextEndPointIndex].x - transform.position.x;
        var v = _enemyMovement.endPoints[_enemyMovement.nextEndPointIndex].y - transform.position.y;
        _direction = new Vector2(h, v);
        DrawFOV();
        CheckPlayerInFOV();
    }

    private void DrawFOV()
    {
        var rayCount = 50;
        var angleStep = viewAngle / rayCount;
        var startAngle = transform.eulerAngles.z - viewAngle / 2;
        Vector3[] points = new Vector3[rayCount + 2];
        
        points[0] = transform.position;
        for (int i = 0; i < rayCount; i++)
        {
            var angle = startAngle + angleStep * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * _direction.normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewDistance, _obstacleMask);
            points[i + 1] = hit.collider == null ? (transform.position + (Vector3)(direction * viewDistance)) : hit.point;
        }
        points[rayCount + 1] = transform.position;
        _lineRenderer.positionCount = points.Length;
        _lineRenderer.SetPositions(points);
    }

    private void CheckPlayerInFOV()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        var distanceToPlayer = Vector2.Distance(player.position, transform.position);
        var angleToPlayer = Vector2.Angle(_direction.normalized, directionToPlayer);

        if (distanceToPlayer > viewDistance || angleToPlayer > viewAngle / 2)
        {
            return;
        }
        var hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, _obstacleMask);
        if (hit.collider == null)
        {
            Debug.Log("Player is in FOV!");
        }
    }
}