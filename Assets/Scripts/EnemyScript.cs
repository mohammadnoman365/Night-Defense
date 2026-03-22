using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Animator anim;

    private Transform playerTransform;

    public AudioClip zombieDie; 

    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        SetMoveSpeed();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                playerTransform.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    void SetMoveSpeed()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "Night 1")
        {
            moveSpeed = 2f;
        }
        else if (sceneName == "Night 2")
        {
            moveSpeed = 2.5f;
        }
        else if (sceneName == "Night 3")
        {
            moveSpeed = 3f;
        }
        else
        {
            moveSpeed = 2f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bullet hit
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AudioManager.Instance.PlaySFX(zombieDie);
            anim.SetTrigger("die");
            Destroy(collision.gameObject);
            Destroy(gameObject, 1.5f);
        }
    }
}