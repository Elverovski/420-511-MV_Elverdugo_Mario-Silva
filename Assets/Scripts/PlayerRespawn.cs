using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;

    private Transform currentCheckPoint;
    private PlayerHealth playerHealth;
    private Animator animator;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
    }

    public void Respawn()
    {
        if (currentCheckPoint != null)
        {
            transform.position = currentCheckPoint.position;
            playerHealth.Respawn();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            Debug.Log("Checkpoint active: " + collision.name);

            currentCheckPoint = collision.transform;
            animator.SetTrigger("appear");

        }
    }
}
