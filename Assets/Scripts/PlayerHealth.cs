using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [Header("UI - Coeurs")]
    public GameObject heart1;         
    public GameObject heart2;          
    public GameObject heart3;


    public GameObject myCanvasGameObject;
    private bool restart = false;

    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        UpdateHearts();
    }

    public int maxHealth = 3;
    private int currentHealth;
    void Awake() => currentHealth = maxHealth;

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

        if (currentHealth <= 0) Die();
    }

    public void Die()
    {
        
        if (currentHealth == 0)
        {
            Debug.Log("Player est mort !");
            GetComponent<PlayerMove>().enabled = false;
            animator.SetTrigger("Dead");
            ShowCanvas();

        }

        if(restart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
       
        
    }

    void UpdateHearts()
    {
        heart1.SetActive(false);
        heart2.SetActive(false);
        heart3.SetActive(false);

        if (currentHealth >= 1) heart1.SetActive(true);
        if (currentHealth >= 2) heart2.SetActive(true);
        if (currentHealth >= 3) heart3.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Healing"))
        {
            Heal(1);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Trap"))
        {
            TakeDamage(1);
        }
    }

    public void Respawn()
    {
        currentHealth = 3;
        animator.ResetTrigger("Dead");
        animator.Play("Idle");
    }

    public void HideCanvas()
    {
        myCanvasGameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        myCanvasGameObject.SetActive(true);
    }

}