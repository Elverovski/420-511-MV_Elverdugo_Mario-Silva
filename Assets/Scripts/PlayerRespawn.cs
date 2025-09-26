using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    public GameObject checkpoint;

    private Transform currentCheckPoint;
    private PlayerHealth playerHealth;
    private Animator animator;
    

    void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        animator = checkpoint.GetComponent<Animator>();
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
            

            currentCheckPoint = collision.transform;
            animator.SetTrigger("Appear");
            Debug.Log("Checkpoint active: " + collision.name);


        }
    }
}
