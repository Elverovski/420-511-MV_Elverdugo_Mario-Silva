using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI - Coeurs")]
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    [SerializeField] private PlayerRespawn respawnSystem;

    public int maxHealth = 3;
    private int currentHealth;

    private Animator animator;

    void Awake() => currentHealth = maxHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        UpdateHearts();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player récupère " + amount + " HP. HP = " + currentHealth);
        UpdateHearts();
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player prend " + dmg + " dégâts. HP restants = " + currentHealth);

        UpdateHearts();

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("Player est mort !");
        GetComponent<PlayerMove>().enabled = false;
        animator.SetTrigger("Dead");

        Invoke(nameof(CallRespawn), 1f);
    }

    private void CallRespawn()
    {
        respawnSystem.Respawn(); 
        currentHealth = maxHealth; 
        animator.ResetTrigger("Dead");
        animator.Play("Idle");
        GetComponent<PlayerMove>().enabled = true;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        heart1.SetActive(currentHealth >= 1);
        heart2.SetActive(currentHealth >= 2);
        heart3.SetActive(currentHealth >= 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Healing"))
        {
            Heal(1);
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }
}
