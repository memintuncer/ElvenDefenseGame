using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] private float CastleHealth, StartingCastleHealth;
    [SerializeField] Transform CastleSprites;

    public float GetCastleHealth()
    {
        return CastleHealth;
    }

    private void OnEnable()
    {
        EventManager.StartListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, CastleDamaged);
    }

    private void OnDisable()
    {
        EventManager.StopListening(GameConstants.GameEvents.MONSTER_REACHED_TO_CASTLE, CastleDamaged);
    }


    void CastleDamaged(EventParam param)
    {
        CastleHealth -= param.GetMonsterDamage();
        CheckCastleHealth(CastleHealth);
    }


    void CheckCastleHealth(float new_health)
    {
        if(new_health <=StartingCastleHealth/2 && new_health > 0)
        {
            CastleSprites.GetChild(0).gameObject.SetActive(false);
            CastleSprites.GetChild(1).gameObject.SetActive(true);
        }

        if (new_health <= 0)
        {
            CastleSprites.GetChild(1).gameObject.SetActive(false);
            CastleSprites.GetChild(2).gameObject.SetActive(true);
            EventManager.TriggerEvent(GameConstants.GameEvents.CASTLE_DESTROYED, new EventParam());
        }
    }

    private void Start()
    {
        StartingCastleHealth = CastleHealth;
    }

}
