using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public int maxHealth = 100;
    private int currentHealth;
    void Awake() => currentHealth = maxHealth;

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player récupère " + amount + " HP. HP = " + currentHealth);
    }



    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Player prend " + dmg + " dégâts. HP restants = " + currentHealth);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player est mort !");
        // Ici : désactiver le joueur, lancer une animation, recharger la scène, etc.
        // Exemple (selon ton architecture) :
        GetComponent<PlayerMove>().enabled = false;
        animator.SetTrigger("Dead");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Healing"))
        {
            Heal(30); 
        }
        if (collision.CompareTag("Trap"))
        {
            Die();
        }
    }

}