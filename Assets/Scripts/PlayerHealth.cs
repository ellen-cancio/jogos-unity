using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Variáveis de Configuração
    public float maxHealth = 100f;
    public float currentHealth;

    // Componentes Públicos e Privados
    public HealthBar healthBar;

    private Rigidbody2D rb;
    private Animator anim;

    // Referência específica para o script de movimento do jogador
    private MainCharacterController playerMovementScript;

    // Usamos Awake() para garantir que a saúde seja inicializada antes de qualquer Start()
    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        // Obtendo os componentes
        // Se o script estiver no objeto filho (Visual), use GetComponentInParent<>
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        // Obtendo o script de controle de movimento
        playerMovementScript = GetComponent<MainCharacterController>();
    }

    /// <summary>
    /// Aplica dano ao jogador e verifica se ele morreu.
    /// </summary>
    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockbackForce = 10f)
    {
        // Ignora dano se o jogador já estiver morto
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        // 🛑 Adicione este log para ver o dano inicial
        Debug.Log("DANO RECEBIDO! Valor: " + damage + ". Nova Vida: " + currentHealth);
        // Aplica knockback
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        // Atualiza a barra de vida (se anexada)
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        // Verifica a condição de morte
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Lógica executada quando a vida do jogador chega a zero.
    /// </summary>
    void Die()
    {
        Debug.Log("Player morreu! Acionando animação e parando movimento.");

        // 1. Desativação IMEDIATA do script de controle
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // 2. Aciona a animação de morte via Trigger
        if (anim != null)
        {
            anim.SetTrigger("Morrer");
        }

        // 3. Para qualquer movimento físico restante (usando linearVelocity)
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Esta função DEVE ser chamada via Animation Event no ÚLTIMO FRAME da animação "Morrer".
    /// </summary>
    public void FinishDeath()
    {
        Debug.Log("Animação de morte finalizada. Destruindo objeto.");

        // Destrói o objeto do personagem, finalizando o ciclo de vida
        Destroy(gameObject);
    }
}