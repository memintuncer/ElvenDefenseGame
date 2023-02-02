using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class MeleeMonsters : Monster
{
    protected override void Movement()
    {
        MovementTimer += Time.deltaTime;
        MonsterAnimator.SetTrigger("Moving");
        
        CheckDestion();
       
        
        
    }


    void CheckDestion()
    {
      
        if(transform.position.x == 1.5 && transform.position.y == -1.5)
        {
            Time.timeScale = 0;
        }
        
        

        if((DestinationPoint.x - transform.position.x) <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if((DestinationPoint.x - transform.position.x) > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        

    }
  


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Castle")
        {
            if(State != MonsterStates.Death)
            {
                SendMonsterAttackedToCastleMessage();
                PathFollowingSequence.Kill();
                SendMonsterToPool();
            }
            
        }

        if(collision.tag == "Destionation" && DestinationIndex< DestinationTransforms.Count)
        {
            try
            {
                DestinationIndex++;
                DestinationPoint = DestinationTransforms[DestinationIndex + 1].position;
            }

            catch
            {
               
            }
            
        }
    }

   

    void SendMonsterAttackedToCastleMessage()
    {
        EventParam monster_param = new EventParam(this,AttackPower);
        EventManager.TriggerEvent(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, monster_param);
    }

    protected override void MoveToNextDestionation()
    {
        DestinationIndex++;
        DestinationPoint = DestinationTransforms[DestinationIndex].position;
    }


    

}
