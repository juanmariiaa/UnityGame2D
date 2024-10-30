using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private float verticalDirection = 0f; // Para el movimiento en escaleras
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private bool isOnLadder = false; // Nuevo estado para saber si está en una escalera

    private Animator playerAnimation;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    public TMP_Text scoreText;
    public HealthBar healthBar;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score: " + Scoring.totalScore;
    }

    void Update()
    {
        // Respawn if health is zero
        if (Health.totalHealth <= 0f)
        {
            Respawn();
            return;
        }

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");
        verticalDirection = Input.GetAxis("Vertical"); // Captura el movimiento vertical

        // Movimiento en escaleras
        if (isOnLadder)
        {
            player.gravityScale = 0; // Desactiva la gravedad mientras esté en la escalera
            player.linearVelocity = new Vector2(direction * speed, verticalDirection * speed); // Permite movimiento en ambas direcciones
            playerAnimation.SetBool("OnLadder", true); // Cambia a la animación de escalera
        }
        else
        {
            player.gravityScale = 1; // Restaura la gravedad
            playerAnimation.SetBool("OnLadder", false);

            if (direction > 0f)
            {
                player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
                transform.localScale = new Vector2(0.2469058f, 0.2469058f);
            }
            else if (direction < 0f)
            {
                player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
                transform.localScale = new Vector2(-0.2469058f, 0.2469058f);
            }
            else
            {
                player.linearVelocity = new Vector2(0, player.linearVelocity.y);
            }

            if (Input.GetButtonDown("Jump") && isTouchingGround)
            {
                player.linearVelocity = new Vector2(player.linearVelocity.x, jumpSpeed);
            }
        }

        playerAnimation.SetFloat("Speed", Mathf.Abs(player.linearVelocity.x));
        playerAnimation.SetBool("OnGround", isTouchingGround);

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void Respawn()
    {
        transform.position = respawnPoint;
        Health.totalHealth = 1f;
        healthBar.SetSize(Health.totalHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            Respawn();
        }
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        else if (collision.tag == "NextLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "PreviousLevel")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            respawnPoint = transform.position;
        }
        else if (collision.tag == "Crystal")
        {
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;
            collision.gameObject.SetActive(false);
        }
        else if (collision.tag == "Ladder")
        {
            isOnLadder = true;
        }
        else if (collision.tag == "End") // Detecta si el jugador toca el objeto de finalización del juego
        {
            SceneManager.LoadScene(0); // Vuelve a la escena inicial
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder") // Detecta si el jugador sale de la escalera
        {
            isOnLadder = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Spike")
        {
            healthBar.Damage(0.01f);
        }
    }
}
