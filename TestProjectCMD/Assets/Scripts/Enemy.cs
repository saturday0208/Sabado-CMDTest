using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform ledgeCheck;
    [SerializeField] protected Transform detectCheck;
    [SerializeField] protected Transform respawnPoint;
    private EnemyHealth enemyHealth;

    public int facingDirection { get; private set; }
    public Rigidbody2D rb2d { get; private set; }
    public Animator animator { get; private set; }
    public GameObject aliveGO { get; private set; }

    public LayerMask isGround;
    public LayerMask isPlayer;

    public bool isStunned = false;
    public bool hasDetectedPlayer;
    private bool isFlipped = false;

    public CombatEntity combatEntity;

    public PlayerController detectedPlayer;

    private void Start()
    {
        facingDirection = 1;

        //change if something bad happens (supposedly this is a transform.Find("Alive").gameobject)
        aliveGO = this.transform.gameObject;
        rb2d = aliveGO.GetComponent<Rigidbody2D>();
        animator = aliveGO.GetComponent<Animator>();
        //enemyHealth = aliveGO.GetComponent<EnemyHealth>();
    }


    private void FixedUpdate()
    {
        PhysicsUpdate();
    }

    public virtual void PhysicsUpdate()
    {
        CheckWall();
        CheckLedge();

        hasDetectedPlayer = CheckPlayer();

        if (hasDetectedPlayer)
        {
            RaycastHit2D col = CheckPlayer();
            detectedPlayer = col.transform.GetComponent<PlayerController>();
        }
        else
        {
            detectedPlayer = null;
        }
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, 0.3f, isGround);
    }

    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, 0.55f, isGround);
    }

    public virtual RaycastHit2D CheckPlayer()
    {
        return Physics2D.Raycast(detectCheck.position, aliveGO.transform.right, 4f, isPlayer);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        aliveGO.transform.Rotate(0f, 180f, 0f);

        if (isFlipped) isFlipped = false;
        else isFlipped = true;
    }

    public void LookAtPlayer()
    {
        if (detectedPlayer != null)
        {
            Vector3 flipped = transform.localScale;
            flipped.z *= -1f;
            if (transform.position.x < detectedPlayer.transform.position.x && isFlipped)
            {
                transform.localScale = flipped;
                Flip();
            }
            else if (transform.position.x > detectedPlayer.transform.position.x && !isFlipped)
            {
                transform.localScale = flipped;
                Flip();
            }
        }
    }

    public void Death()
    {
        animator.speed = 1;
        //rb2d.gravityScale = 0;
        rb2d.velocity = Vector2.zero;
        animator.SetTrigger("IsDead");
        StartCoroutine(RespawnEnemy());
        AudioManager.Instance.PlaySFX("DogDie");
    }

    private IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(5f);
        this.transform.position = respawnPoint.transform.position;
        combatEntity.HealToMax();
        animator.SetTrigger("IsWalk");
        AudioManager.Instance.PlaySFX("DogRespawn");
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * 0.3f));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * 0.55f));
        Gizmos.DrawLine(detectCheck.position, detectCheck.position + (Vector3)(Vector2.right * facingDirection * 6f));

    }

    public void BiteSound()
    {
        AudioManager.Instance.PlaySFX("Bite");
    }
}
