using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class Monster : MonoBehaviour
{



    public enum MonsterStates
    {
        Idle,
        Moving,
        Attack,
        TakingDamage,
        Death,
        ReachedCastle,
        Null
    }

    [SerializeField] MonsterTypes.Type Type;
    [SerializeField] protected float Health, MovementSpeed, AttackPower, StartingHealth,HealthIncreaseAmount;
    [SerializeField] protected Image HealthBarImage;
    protected Vector2 DestinationPoint;
    protected Animator MonsterAnimator;
    protected MonsterStates State;
    protected List<Transform> DestinationTransforms = new List<Transform>();
    protected int DestinationIndex = 0;
    protected float HealthBarImageAmount=1;



    [SerializeField] protected PathType PathType;
    [SerializeField] protected PathMode PathMode;
    protected Sequence PathFollowingSequence ;
    protected float MovementTimer=0;
    public float HealthBarSize;



    public void SetStartingHealth(float health)
    {
        StartingHealth = health;
    }

    public float GetMovementTimer()
    {
        return MovementTimer;
    }

    public void SetMovementTimer(float movement_timer)
    {
        MovementTimer = movement_timer;
    }
    public void SetSequence(Sequence sequence)
    {
        PathFollowingSequence = sequence;
    }
    public void SetHealthBarImage()
    {
        HealthBarImage.fillAmount = 1;
    }

    public float GetHealthBarImageAmount()
    {
        return HealthBarImage.fillAmount;
    }
    public MonsterStates GetMonsterState()
    {
        return State;
    }
    public float GetHealth()
    {
        return Health;
    }

    public void SetHealth(int health_decrease)
    {
        Health -= health_decrease;
    }

    public void NewHealth(int wave_count)
    {
        Health += wave_count * HealthIncreaseAmount;
       
    }
   

     public float GetMovementSpeed()
    {
        return Health;
    }

    public void SetMovementSpeed(float movement_speed_decrease)
    {
        MovementSpeed -= movement_speed_decrease;
    }
     public float GetAttackPower()
    {
        return Health;
    }

    public void SetAttackPower(float attack_power_decrease)
    {
        Health -= attack_power_decrease;
    }

    public MonsterTypes.Type GetMonsterType()
    {
        return Type;
    }


    public int GetDestinationIndex()
    {
        return DestinationIndex;
    }

    public void SetDestinationIndex(int destionation_index)
    {
        DestinationIndex = destionation_index;
    }

    public void SetDestionations(List<Transform> destionation_list)
    {
        DestinationIndex = 0;
        DestinationTransforms = destionation_list;
        DestinationPoint = destionation_list[1].position;
    }
    public void SetDestination(Vector3 point)
    {
        DestinationPoint = point;
    }
    public void SendMonsterToPool()
    {
        State = MonsterStates.Null;
        Health = StartingHealth;
        State = MonsterStates.Idle;
        HealthBarImage.fillAmount = 1;
        DestinationIndex = 0;
        MovementTimer = 0;
        PathFollowingSequence.Kill();
       
        gameObject.SetActive(false);
        
        
    }

    protected virtual void Movement()
    {
       
    }


   

    protected virtual void MonsterStateMachine()
    {
        switch (State)
        {
            case MonsterStates.Idle:
                State = MonsterStates.Moving;
                break;
            case MonsterStates.Moving:
                if (Health > 0)
                {
                    Movement();
                }
                else
                {
                    State = MonsterStates.Death;
                }
                
                break;
            case MonsterStates.Attack:
                break;
            case MonsterStates.Death:
                
                PathFollowingSequence.Kill();
                SendEnemyKilledMessage();
                MonsterAnimator.SetTrigger("Death");
                State = MonsterStates.Null;
                break;
            case MonsterStates.TakingDamage:
                break;

            case MonsterStates.Null:
                break;
        }
    }

    void SendEnemyKilledMessage()
    {
        EventParam killed_enemy_message_param = new EventParam(this);
        EventManager.TriggerEvent(GameConstants.GameEvents.KILLED_MONSTER, killed_enemy_message_param);
    }
    protected virtual void SetStartConditions()
    {

        MonsterAnimator = transform.GetChild(0).GetComponent<Animator>();
        State = MonsterStates.Idle;
       
        
    }

    protected virtual void MoveToNextDestionation()
    {

    }


    public void TakeDamage(float damage,Bullet bullet)
    {
        Health -= damage;
        SetHealthBarImage(damage);
        if (Health <= 0)
        {
            bullet.GetDefenderParent().IncreaseDeathCount();
        }
    }
    

    void SetHealthBarImage(float damage)
    {
        float fill_amount = HealthBarImage.fillAmount;
        float decrease_amount = damage / HealthBarSize;
        HealthBarImage.fillAmount -= decrease_amount;
        HealthBarImageAmount = HealthBarImage.fillAmount;
    }
    

    public void IncreaseHealth(int health_increase_amount)
    {
        float previous_health = StartingHealth;
        float new_health = previous_health + health_increase_amount;
        Health = new_health;
    }


    public void UpdateMonstersForSurvivalMode()
    {
        
    }

    public void LoadMonsterData(MonsterData monster_data, List<Transform> destinations)
    {
        float pos_x = monster_data.PosX;
        float pos_y = monster_data.PosY;
        MovementTimer = monster_data.MovementTimer;
        transform.position = new Vector2(pos_x, pos_y);
        DestinationIndex = monster_data.DestinationIndex;
        Health = monster_data.Health;
        HealthBarImage.fillAmount = monster_data.HealthImageAmount;
        DestinationTransforms = destinations;
        DestinationPoint = destinations[DestinationIndex].position;
        
        
    }

   
   

    public void SaveDestionationPoints(MonsterData monster_data)
    {
        MonsterData.MonsterDestionationPoint start_point = new MonsterData.MonsterDestionationPoint(transform.position.x,transform.position.y);
        
        List<MonsterData.MonsterDestionationPoint> destination_points = new List<MonsterData.MonsterDestionationPoint>();
        destination_points.Add(start_point);

      
        for (int i = DestinationIndex+1; i < DestinationTransforms.Count; i++)
        {
            MonsterData.MonsterDestionationPoint destination_point = new MonsterData.MonsterDestionationPoint(DestinationTransforms[i].position.x, DestinationTransforms[i].position.y);
            destination_points.Add(destination_point);
        }
        monster_data.MonsterDestionationPoints = destination_points;
        
    }


   
}
