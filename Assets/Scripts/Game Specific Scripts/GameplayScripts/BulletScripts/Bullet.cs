using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Bullet : MonoBehaviour
{
    [SerializeField] private float AttackPower,Speed;
    public Transform TargetMonsterTransform;
    private Defender DefenderParent;
    private Monster TargetMonster;
    Animator BulletAnimator;
    DefenderTypes.Type DefenderType;
    private float Timer = 0.25f;
    float MovementTimer = 0;
    public Defender GetDefenderParent()
    {
        return DefenderParent;
    }
    public void SetDefenderParent(Defender defender_parent)
    {
        DefenderParent = defender_parent;
    }

    public void SetAttackPower(float new_power)
    {
        AttackPower = new_power;
    }


    void Start()
    {
        BulletAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        MovementTimer += Time.deltaTime; 
        MoveToTarget();
    }


    public void MoveToTarget()
    {
      
        
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            ResetBullet();
        }
       
    }

    public void SetTarget(Monster target)
    {
        TargetMonster = target;
    }

    public void ResetBullet()
    {
        
        TargetMonster.TakeDamage(AttackPower,this);
        transform.position = DefenderParent.transform.position;
        Timer = 0.25f;
        MovementTimer = 0;
        gameObject.SetActive(false);
        
    }

    public BulletData SaveBulletData()
    {
        BulletData bullet_data = new BulletData();
        bullet_data.PosX = transform.position.x;
        bullet_data.PosY = transform.position.y;
        bullet_data.MovementTimer = MovementTimer;
        return bullet_data;
    }

    public void LoadBulletData(BulletData bullet_data)
    {
        float pos_x = bullet_data.PosX;
        float pos_y = bullet_data.PosY;
        transform.position = new Vector2(pos_x, pos_y);
    }

  
}
