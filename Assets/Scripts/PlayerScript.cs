using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    public float moveSpeed = 5f;

    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public AudioClip shootingSound;

    public int startingBullets = 45;
    public Transform bulletSpawnPoint;
    public TextMeshProUGUI bulletText;
    public GameObject gameOverPanel;

    private bool isFacingRight = true;
    private bool isGameOver = false;

    private bool isMovingLeft = false;
    private bool isMovingRight = false;

    public AudioClip zombieBite;

    void Start()
    {
        anim = GetComponent<Animator>();
        gameOverPanel.SetActive(false);

        if (GameData.remainingBullets <= 0)
        {
            GameData.remainingBullets = startingBullets;
        }

        bulletText.text = GameData.remainingBullets.ToString();
    }

    void Update()
    {
        if (isGameOver) return;

        HandleMovement();
    }

    void HandleMovement()
    {
        if (isMovingRight)
        {
            anim.SetBool("walk", true);
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            transform.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            isFacingRight = true;
        }
        else if (isMovingLeft)
        {
            anim.SetBool("walk", true);
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            transform.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            isFacingRight = false;
        }
        else
        {
            anim.SetBool("walk", false);
        }
    }

    // Button event methods
    public void StartMovingLeft()
    {
        if (isGameOver) return;
        isMovingLeft = true;
    }

    public void StopMovingLeft()
    {
        isMovingLeft = false;
    }

    public void StartMovingRight()
    {
        if (isGameOver) return;
        isMovingRight = true;
    }

    public void StopMovingRight()
    {
        isMovingRight = false;
    }

    public void Shoot()
    {
        anim.SetTrigger("shoot");

        Rigidbody2D playerRb = GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    public void FireBullet()
    {
        if (isGameOver || GameData.remainingBullets <= 0) return;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * bulletSpeed, 0);

        AudioManager.Instance.PlaySFX(shootingSound);

        GameData.remainingBullets--;
        bulletText.text = GameData.remainingBullets.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player touched
        if (collision.gameObject.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySFX(zombieBite);
            gameOverPanel.SetActive(true);
            isGameOver = true;
            Time.timeScale = 0f;
        }
    }
}