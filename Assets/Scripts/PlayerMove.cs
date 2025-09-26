using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Clip audio à jouer quand le joueur saute (assigné dans l’Inspector)
    [SerializeField] AudioClip sfxJump; 
    // Composant AudioSource qui jouera les sons
    private AudioSource audioSource;

    // Valeur d’entrée horizontale (−1 = gauche, 0 = immobile, 1 = droite)
    private float x;
    // Composant pour gérer l’affichage du sprite (retourner à gauche/droite)
    private SpriteRenderer spriteRenderer;
    // Composant pour gérer les animations du joueur
    private Animator animator;
    // Composant physique pour gérer les forces (notamment le saut)
    private Rigidbody2D rb;

    // Indique si le joueur doit sauter à la prochaine frame physique
    private bool jump = false;

    private bool isGrounded = false;


    void Awake()
    {
        // Récupère les composants nécessaires attachés au GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
     
    }

    // Update est appelé une fois par frame (logique liée aux entrées joueur)
    void Update()
    {
        // ---- Déplacement horizontal ----
        x = Input.GetAxis("Horizontal");
        animator.SetFloat("x", Mathf.Abs(x)); 
        transform.Translate(Vector2.right * 4f * Time.deltaTime * x);

        // ---- Orientation du sprite ----
        if (x > 0f) { spriteRenderer.flipX = false; } 
        if (x < 0f) { spriteRenderer.flipX = true; }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            jump = true; // signal qu’il faut sauter dans FixedUpdate
            audioSource.PlayOneShot(sfxJump);

        }

        // ---- Animation d’attaque ----
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("Attack", true); 
        }
        else
        {
            animator.SetBool("Attack", false); 
        }

        // ---- Animation de courrir ----
        if (x != 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }


    private void FixedUpdate()
    {
        transform.Translate(Vector2.right * 2f * Time.deltaTime * x);

        // ---- Saut ----
        if (jump) 
        {
            jump = false; 
            rb.AddForce(Vector2.up * 250f); 
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}