using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wasp{
    public int[,] template = new int[3,3];
    public float health = 0;

    public void ModifyHealth(float v)
    {
        health += v;
    }

    public override string ToString(){
        string msg = "\n";
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                msg += template[i,j] + ",";
            }
            msg += '\n';
        }
        msg += "Points: " + health;
        return msg;
    }

    public static Wasp Select(List<Wasp> wasps)
    {
        float minPoints = 0;
        float total = 0;
        foreach(var wasp in wasps) {
            total += Mathf.Max(minPoints, wasp.health);
        }

        float random = UnityEngine.Random.Range(0, total);
        float accumulation = 0;
        foreach(var wasp in wasps) {
            accumulation += Mathf.Max(minPoints, wasp.health);
            if(random < accumulation) {
                return wasp;
            }
        }
        return wasps[wasps.Count - 1];
    }

}