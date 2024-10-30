using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

     // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (direction > 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
        }
        else if (direction < 0f)
        {
            player.linearVelocity = new Vector2(direction * speed, player.linearVelocity.y);
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
}