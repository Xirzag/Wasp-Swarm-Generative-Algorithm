using System.Collections.Generic;
using UnityEngine;

class DickShape : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {

        int y = 5 + seed % 10;
        int r = 3 + seed % 3;

        HashSet<Vector2Int> dick = new HashSet<Vector2Int>();

        for(int i = 0; i < y; i++) {
            dick.Add(new Vector2Int(-r, i + r));
            dick.Add(new Vector2Int( r, i + r));
        }

        for(int i = -r; i < r; i++) {
            dick.Add(new Vector2Int(i,  y + r));
            dick.Add(new Vector2Int(i,  y + r - 2));
        }

        var eggLeft = CircleOutline.MidPointCircleDraw(-r, 0, r);
        var eggRight = CircleOutline.MidPointCircleDraw(r, 0, r);

        dick.UnionWith(eggLeft);
        dick.UnionWith(eggRight);

        return dick;
        
    }

}