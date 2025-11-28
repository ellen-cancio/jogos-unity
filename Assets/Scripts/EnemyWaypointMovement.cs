using UnityEngine;
using System.Collections.Generic;

public class EnemyWaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints;

    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float waypointReachedDistance = 0.1f;
    public bool loop = true;

    [Header("Combat Settings")] // Configurações de ataque adicionadas
    public float damage = 10f;
    public float attackCooldown = 1f;
    public float knockbackForce = 15f;

    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private Vector2 movementDirection;
    private float lastAttackTime; // Controle de tempo do ataque

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Validação dos Waypoints
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy!");
            enabled = false;
            return;
        }

        SetTargetWaypoint(currentWaypointIndex);
    }

    void FixedUpdate()
    {
        MoveTowardsWaypoint();
        CheckIfWaypointReached();
    }

    // --- LÓGICA DE MOVIMENTAÇÃO (Igual ao seu código) ---

    void SetTargetWaypoint(int index)
    {
        if (waypoints.Count == 0) return;

        currentWaypointIndex = index;
        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;
    }

    void MoveTowardsWaypoint()
    {
        if (waypoints.Count == 0) return;

        Vector2 targetPosition = waypoints[currentWaypointIndex].position;
        movementDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Movimento
        rb.linearVelocity = movementDirection * moveSpeed;
    }

    void CheckIfWaypointReached()
    {
        if (waypoints.Count == 0) return;

        float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint <= waypointReachedDistance)
        {
            GoToNextWaypoint();
        }
    }

    void GoToNextWaypoint()
    {
        currentWaypointIndex++;

        if (currentWaypointIndex >= waypoints.Count)
        {
            if (loop)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                enabled = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        SetTargetWaypoint(currentWaypointIndex);
    }

    // --- LÓGICA DE COMBATE (Adicionada) ---

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttackPlayer(collision.gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TryAttackPlayer(collision.gameObject);
        }
    }

    void TryAttackPlayer(GameObject player)
    {
        // Verifica o tempo de recarga (Cooldown)
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // PROCURA PELO MainCharacterController
            MainCharacterController controller = player.GetComponent<MainCharacterController>();

            if (controller != null)
            {
                Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;

                // Chama o TakeDamage no MainCharacterController
                controller.TakeDamage(damage, knockbackDirection, knockbackForce);

                lastAttackTime = Time.time;
            }
        }
    }
}