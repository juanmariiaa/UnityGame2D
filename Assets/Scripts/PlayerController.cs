using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    public float climbSpeed = 4f; // velocidad para escalar
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    private Animator playerAnimation;

    private Vector3 respawnPoint;
    public GameObject fallDetector;
    public TMP_Text scoreText;

    private bool isOnLadder = false; // si está en la escalera

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        playerAnimation = GetComponent<Animator>();
        respawnPoint = transform.position;
        scoreText.text = "Score: " + Scoring.totalScore;
    }

    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (isOnLadder)
        {
            // Movimiento vertical en la escalera
            float vertical = Input.GetAxis("Vertical");
            player.linearVelocity = new Vector2(direction * speed, vertical * climbSpeed);
            
            // Cambia a la animación de escalera solo si se está moviendo arriba o abajo
            playerAnimation.SetBool("OnLadder", vertical != 0);
        }
        else
        {
            // Movimiento horizontal normal
            if (direction != 0f)
            {
                player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
                transform.localScale = new Vector2(Mathf.Sign(direction) * 0.2469058f, 0.2469058f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "FallDetector")
        {
            transform.position = respawnPoint;
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
            player.gravityScale = 0f; // desactiva la gravedad mientras está en la escalera
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            isOnLadder = false;
            player.gravityScale = 1f; // restablece la gravedad al salir de la escalera
            playerAnimation.SetBool("OnLadder", false); // vuelve a la animación normal
        }
    }

    
}
