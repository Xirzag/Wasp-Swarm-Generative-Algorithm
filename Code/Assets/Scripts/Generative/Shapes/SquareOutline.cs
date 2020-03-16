using System.Collections.Generic;
using UnityEngine;

class SquareOutline : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {

        int x = 5 + seed % 10;
        int y = 5 + seed % 10;
        
        HashSet<Vector2Int> shape = new HashSet<Vector2Int>();

        for(int i = 0; i < x; i++) {
            shape.Add(new Vector2Int(i, 0));
            shape.Add(new Vector2Int(i, y - 1));
        }

        for(int j = 0; j < y; j++) {
            shape.Add(new Vector2Int(0, j));
            shape.Add(new Vector2Int(x - 1, j));
        }   

        return shape;
        
    }

}