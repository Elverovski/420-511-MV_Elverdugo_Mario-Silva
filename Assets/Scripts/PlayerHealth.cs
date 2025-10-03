using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("UI - Coeurs")]
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    [SerializeField] private PlayerRespawn respawnSystem;
    public GameObject targetCanvasGameObject;

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

        
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float knockbackForceX = 300f;  
            float knockbackForceY = 250f;  
            
            float direction = GetComponent<SpriteRenderer>().flipX ? 1f : -1f;

            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(direction * knockbackForceX, knockbackForceY));
        }

        UpdateHearts();

        if (currentHealth <= 0)
            Die();
    }


    private async void Die()
    {
        Debug.Log("Player est mort !");
        GetComponent<PlayerMove>().enabled = false;
        animator.SetTrigger("Dead");
        await Task.Delay(3000);
        ShowCanvas();
        Invoke(nameof(CallRespawnOrReload), 5f);
    }

    private void CallRespawnOrReload()
    {
        HideCanvas();

        if (respawnSystem != null && respawnSystem.HasCheckpoint())
        {
            respawnSystem.Respawn();
            currentHealth = maxHealth;
            animator.ResetTrigger("Dead");
            animator.Play("Idle");
            GetComponent<PlayerMove>().enabled = true;
            UpdateHearts();


            foreach (HealingItem item in FindObjectsByType<HealingItem>(FindObjectsSortMode.None))
            {
                item.ResetItem();
            }

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
            HealingItem heal = collision.GetComponent<HealingItem>();
            if (heal != null && heal.CanHeal())
            {
                Heal(1);
                heal.Use();
            }
        }
        else if (collision.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }

    public void ShowCanvas()
    {
        targetCanvasGameObject.SetActive(true);
    }

    public void HideCanvas()
    {
        targetCanvasGameObject.SetActive(false);
    }
}
