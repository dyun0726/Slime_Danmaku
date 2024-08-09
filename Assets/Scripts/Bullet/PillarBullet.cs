using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBullet : BossBullet
{
    private Animator animator;
    public bool isFirst = false;
    public bool left = false;
    public bool right = false;
    public int count = 0;
    public float waitingTime = 0.5f;
    private int maxCount = 5;
    private string bulletName = "Paladin_Pillar";
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        
    }

    public void Ready()
    {
        if (isFirst)
        {
            transform.localScale = new Vector3(1, 0.25f, 1);
            StartCoroutine(Fire());
        }
        else
        {
            transform.localScale = Vector3.one;
            animator.SetTrigger("Fire_First");
        }
        
    }

    private void CreateNext()
    {
        if (count < maxCount)
        {
            if (left) 
            {
                GameObject bulletGO = PoolManager.instance.GetGO(bulletName);
                PillarBullet bullet = bulletGO.GetComponent<PillarBullet>();
                bullet.Damage = Damage;
                bullet.isFirst = false;
                bullet.left = true;
                bullet.right = false;
                bullet.count = count + 1;
                bulletGO.transform.position = transform.position + Vector3.left * 2;
                bullet.Ready();
            }

            if (right) 
            {
                GameObject bulletGO = PoolManager.instance.GetGO(bulletName);
                PillarBullet bullet = bulletGO.GetComponent<PillarBullet>();
                bullet.Damage = Damage;
                bullet.isFirst = false;
                bullet.left = false;
                bullet.right = true;
                bullet.count = count + 1;
                bulletGO.transform.position = transform.position + Vector3.right * 2;
                bullet.Ready();
            }
        }
        
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(waitingTime);
        transform.localScale = Vector3.one;
        animator.SetTrigger("Fire_First");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Debug.Log("In BossBullet:" + other.gameObject.name);
        if (other.gameObject.layer == 10) { // player 일때
            Vector2 dir = (other.transform.position - transform.position).normalized;
            PlayerManager.Instance.TakeDamage(Damage, dir);
        }
        
    }
}
