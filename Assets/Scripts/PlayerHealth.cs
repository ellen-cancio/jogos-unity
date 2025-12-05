using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private Rigidbody2D rb;
    public HealthBar healthBar;

    // NOVO: Referência ao Animator para controlar as animações
    private Animator anim;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        // NOVO: Pega o componente Animator do objeto
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection, float knockbackForce = 10f)
    {
        // Se já estiver morto, ignora o dano
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log("Player levou dano! Vida atual: " + currentHealth);

        // Aplica knockback
        if (rb != null)
        {
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player morreu!");

        // NOVO 1: Aciona o Trigger "Morrer" no Animator
        if (anim != null)
        {
            // O nome "Morrer" deve ser exatamente o mesmo parâmetro Trigger que você configurou no Animator.
            anim.SetTrigger("Morrer");
        }
    }

    // Esta função é chamada no FINAL da animação de morte via "Animation Event"
    public void FinishDeath()
    {
        Debug.Log("Animação de morte finalizada. Destruindo objeto.");
        // A linha abaixo usa a função FinishDeath para destruir o objeto no final da animação.
        Destroy(gameObject);

        // Aqui você também pode adicionar lógica de Game Over, recarregar cena, etc.
        // GameManager.Instance.ShowGameOverScreen();
    }

}