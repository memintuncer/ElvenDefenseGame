using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData 
{
    
    private PoolData PoolData;
    private GameManagerData GameManagerData;
    private DefenderGeneratorData DefenderGeneratorData;
    

    public PoolData GetPoolData()
    {
        return PoolData;
    }

    public void SetPoolData(PoolData pool_data)
    {
        PoolData = pool_data;
    }

    public GameManagerData GetGameManagerData()
    {
        return GameManagerData;
    }

    public void SetGameManagerData(GameManagerData game_manager_data)
    {
        GameManagerData = game_manager_data;
    }

    public DefenderGeneratorData GetDefenderGeneratorData()
    {
        return DefenderGeneratorData;
    }

    public void SetDefenderGeneratorData(DefenderGeneratorData defender_generator_data) 
    {
        DefenderGeneratorData = defender_generator_data;
    }


    
}
