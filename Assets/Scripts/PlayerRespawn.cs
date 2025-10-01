using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;

    private Transform currentCheckPoint;
    private Animator checkpointAnimator;

    public void Respawn()
    {
        if (currentCheckPoint != null)
        {
            transform.position = currentCheckPoint.position;
 
            if (checkpointSound != null)
                AudioSource.PlayClipAtPoint(checkpointSound, transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            currentCheckPoint = collision.transform;
            checkpointAnimator = collision.GetComponent<Animator>();
            if (checkpointAnimator != null)
                checkpointAnimator.SetTrigger("Appear");

            Debug.Log("Checkpoint activé: " + collision.name);
        }
    }
}
