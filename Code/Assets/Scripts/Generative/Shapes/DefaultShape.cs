using System.Collections.Generic;
using UnityEngine;

class DefaultShape : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {
        return new HashSet<Vector2Int>(){
            new Vector2Int(0,0),
            new Vector2Int(1,0),
            new Vector2Int(2,0),
            new Vector2Int(3,0),
            new Vector2Int(4,0),
            new Vector2Int(2,1),
            new Vector2Int(2,2),
            new Vector2Int(2,3),
            new Vector2Int(2,4),
            new Vector2Int(3,4)
        };
    }

}