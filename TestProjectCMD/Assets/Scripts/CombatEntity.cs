using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class CombatEntity : MonoBehaviour
{
    public FloatReactiveProperty CurrentHealth = new FloatReactiveProperty(5f);
    public float maxHealth = 5;

    [SerializeField] private float knockbackForce = 1f;
    [SerializeField] private float knockbackUpwardForce = 20f;
    public bool isKnockedBack = false;

    public ParticleSystem particle;

    private Animator animator => GetComponent<Animator>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(CurrentHealth.Value > 0)
            {
                StartCoroutine(HitEffect(collision));
            }
        }
    }

    private IEnumerator HitEffect(Collider2D hit)
    {
        AudioManager.Instance.PlaySFX("Hit");

        CombatEntity targetEntity = hit.GetComponent<CombatEntity>();

        targetEntity.TakeDamage();
        targetEntity.particle.Play();
        targetEntity.isKnockedBack = true;

        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float direction = Mathf.Sign(hit.transform.position.x - this.transform.position.x);
            rb.velocity = new Vector2(direction * knockbackForce, knockbackUpwardForce);
        }

        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1f;

        yield return new WaitForSeconds(0.2f);

        targetEntity.isKnockedBack = false;
    }

    private void TakeDamage()
    {
        CurrentHealth.Value -= 1;
    }

    //To heal enemy HP back on respawn
    public void HealToMax()
    {
        CurrentHealth.Value = maxHealth;
    }
}
