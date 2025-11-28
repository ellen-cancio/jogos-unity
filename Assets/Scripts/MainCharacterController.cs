using UnityEngine;

public class MainCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;     // Velocidade horizontal
    public float jumpForce = 10f;    // Força do pulo
    public Transform groundCheck;    // Objeto vazio no pé do personagem
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;    // O que é chão?

    [Header("Visual Settings")]
    public Transform visual;         // O objeto filho que contém o Sprite
    private Animator anim;

    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;

    // Variáveis privadas
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Pega o Animator que está dentro do objeto visual
        if (visual != null)
        {
            anim = visual.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("ERRO: Você esqueceu de arrastar o objeto 'Visual' no Inspector!");
        }

        // Inicializa a vida
        currentHealth = maxHealth;
    }

    void Update()
    {
        // --- LÓGICA DE MOVIMENTO ---

        // Verifica se está no chão
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Movimento Horizontal
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Animações
        if (anim != null)
        {
            anim.SetBool("isrunning", Mathf.Abs(moveInput) > 0f && isGrounded);
            anim.SetBool("isjumping", Mathf.Abs(rb.linearVelocity.y) > 0f && !isGrounded);
        }

        // Virar o personagem (Flip)
        if (moveInput > 0.01f)
        {
            visual.localScale = new Vector3(4, 4, 4); // Direita
        }
        else if (moveInput < -0.01f)
        {
            visual.localScale = new Vector3(-4, 4, 4); // Esquerda
        }

        // Pulo
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    // --- LÓGICA DE VIDA E DANO ---
    // Este método é chamado pelo Inimigo
    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockbackForce = 10f)
    {
        currentHealth -= damage;
        Debug.Log("Player levou dano! Vida atual: " + currentHealth);

        // Aplica knockback (Empurrão)
        if (rb != null)
        {
            // Zera a velocidade atual para o impacto ser seco
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");
        gameObject.SetActive(false); // Desativa o player da cena
    }
}