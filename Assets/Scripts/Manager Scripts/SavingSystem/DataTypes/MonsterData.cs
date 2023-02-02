using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterData 
{
    [System.Serializable]
    public class MonsterDestionationPoint
    {
        public float PointX, PointY;

        public MonsterDestionationPoint(float point_x,float point_y)
        {
            PointX = point_x;
            PointY = point_y;
        }
    }


    public float Health, HealthImageAmount,MovementTimer;
    public int DestinationIndex;
    public float PosX, PosY;
    public List<MonsterDestionationPoint> MonsterDestionationPoints;
    
   
}
