using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Variáveis de Configuração
    public float maxHealth = 100f;
    public float currentHealth;

    // Componentes Públicos e Privados
    public HealthBar healthBar; // Referência para a UI da barra de vida (opcional)

    private Rigidbody2D rb;
    private Animator anim;

    // Referência específica para o script de movimento do jogador
    private MainCharacterController playerMovementScript;

    void Start()
    {
        currentHealth = maxHealth;

        // Obtendo os componentes
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Obtendo o script de controle de movimento
        // ATENÇÃO: MainCharacterController deve ser o nome exato do seu script de movimento!
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
        Debug.Log("Player levou dano! Vida atual: " + currentHealth);

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

        // 1. Aciona a animação de morte via Trigger no Animator
        if (anim != null)
        {
            anim.SetTrigger("Morrer");
        }

        // 2. Desativa o script de controle para PARAR o personagem
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = false;
        }

        // 3. Para qualquer movimento físico restante (Correção de Obsoleto)
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // ✅ CORRIGIDO: Usa linearVelocity
        }

        // Opcional: Desabilita o colisor
        // GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// Esta função DEVE ser chamada via Animation Event no ÚLTIMO FRAME da animação "Morrer".
    /// </summary>
    public void FinishDeath()
    {
        Debug.Log("Animação de morte finalizada. Destruindo objeto.");

        // Destrói o objeto do personagem, finalizando o ciclo de vida
        Destroy(gameObject);

        // Adicione aqui a lógica de fim de jogo (Exemplo:
        // GameManager.Instance.LoadGameOverScene();
    }
}