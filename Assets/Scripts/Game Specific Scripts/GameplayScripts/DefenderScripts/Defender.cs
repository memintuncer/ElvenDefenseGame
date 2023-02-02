using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public abstract class Defender : MonoBehaviour
{
    protected Monster TargetMonster;
    [SerializeField] GameManager GameManager;
    public List<Bullet> Bullets;
    protected List<Monster> TargetMonsters = new List<Monster>();
    [SerializeField]  protected int LevelUpGoldAmount;
    [SerializeField] protected float AttackPower;
    private float AttackTimer= 0.75f;
    [SerializeField] GameObject BulletPrefab;
    protected bool IsAttacking = false;
    private Animator DefenderAnimator;
    protected int BulletIndex;
    private int LevelupCount = 10,KillCount=0;
    [SerializeField] private DefenderTypes.Type Type;
    Bullet CurrentBullet = null;
    BulletData BulletData;
    int level = 0;
    public float GetAttackPower()
    {
        return AttackPower;
    }

    public void SetAttackPower(float attack_power)
    {
        AttackPower = attack_power;
    }

    public int GetKillCount()
    {
        return KillCount;
    }

    public void SetKillCount(int kill_count)
    {
        KillCount = kill_count;
    }


    public float GetAttackTimer()
    {
        return AttackTimer;
    }

    public int GetBulletIndex()
    {
        return BulletIndex;
    }

    public DefenderTypes.Type GetDefenderType()
    {
        return Type;
    }
    

    public void SetGameManager(GameManager game_manager)
    {
        GameManager = game_manager;
    }

    public void SetTargetMonsters(List<Monster> target_monsters)
    {
        TargetMonsters = target_monsters;
    }

    void Start()
    {
        SetStartConditions();
    }

   


    protected void CheckMonstersOnTheLevel()
    {
        int target_monsters_count = GameManager.GetTargetMonsters().Count;
        if (target_monsters_count > 0)
        {
            TargetMonster = GameManager.GetTargetMonsters()[0];
            Attack();
        }
        
    }


    protected void Attack()
    {
        if (!IsAttacking)
        {
            DefenderAnimator.SetTrigger("Attack");
            IsAttacking = true;
        }

        SendBullet();
        
    }

    void SendBullet()
    {
        
        if (AttackTimer == 0.75f)
        {
            float timer = 0;
            Bullet bullet = Bullets[BulletIndex];
            CurrentBullet = bullet;
            bullet.gameObject.SetActive(true);
            if (BulletData != null)
            {

                
                bullet.LoadBulletData(BulletData);
                timer = BulletData.MovementTimer;
                BulletData = null;
            }
            bullet.SetTarget(TargetMonster);
            bullet.SetAttackPower(AttackPower);
            bullet.transform.DOMove(TargetMonster.transform.position, 0.25f- timer);
            BulletIndex++;
            if(BulletIndex == Bullets.Count)
            {
                BulletIndex = 0;
            }
            
        }
        AttackTimer -= Time.deltaTime;
        
        if(AttackTimer < 0)
        {
            AttackTimer = 0.75f;
        }
    }

    protected virtual void SetStartConditions()
    {
        DefenderAnimator = transform.GetChild(0).GetComponent<Animator>();
        if (Bullets.Count < 10)
        {
            PoolingSystem.Instance.GetBulletFromPool(Type, 10-Bullets.Count, Bullets, this);
        }
        
       
        
    }

    


    public void LevelUp()
    {
       

        
        if (KillCount >= LevelupCount && level<3)
        {
            AttackPower += 5;
            foreach (Bullet bullet in Bullets)
            {
                bullet.SetAttackPower(AttackPower);
            }
            LevelupCount += 10;
        }
    }


   


    public void IncreaseDeathCount()
    {
        KillCount++;
        CheckLevelUpState();
    }

    public void CheckLevelUpState()
    {
        if(KillCount >= LevelupCount)
        {
            LevelUp();
           
        }
    }


    public void LoadDefenderData(DefenderData defender_data)
    {
        float pos_x = defender_data.PosX;
        float pos_y = defender_data.PosY;
        transform.position = new Vector2(pos_x, pos_y);
        AttackPower = defender_data.AttackPower;
        AttackTimer = defender_data.AttackTimer;
        KillCount = defender_data.KillCount;
        BulletData = defender_data.BulletData;
        BulletIndex = defender_data.BulletIndex;
        PoolingSystem.Instance.GetBulletFromPool(Type, 10, Bullets, this);
        CurrentBullet = Bullets[BulletIndex];
        CurrentBullet.gameObject.SetActive(true);
        if (GameManager.GetTargetMonsters().Count > 0)
        {
            TargetMonster = GameManager.GetTargetMonsters()[0];
            CurrentBullet.transform.position = new Vector2(BulletData.PosX, BulletData.PosY);
            float timer = BulletData.MovementTimer;
            CurrentBullet.transform.DOMove(TargetMonster.transform.position, 0.25f - timer);
            BulletData = null;

            CurrentBullet.SetTarget(TargetMonster);
            CurrentBullet.SetAttackPower(AttackPower);
        }
       
    }


    void LoadBulletData()
    {

    }

    public BulletData SaveBulletData()
    {
        return CurrentBullet.SaveBulletData();
    }
}
