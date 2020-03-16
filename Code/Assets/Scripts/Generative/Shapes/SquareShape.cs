using System.Collections.Generic;
using UnityEngine;

class SquareShape : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {

        int x = 5 + seed % 15;
        int y = 5 + seed % 15;

        HashSet<Vector2Int> shape = new HashSet<Vector2Int>();

        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                shape.Add(new Vector2Int(i,j));
            }   
        }

        return shape;
        
    }

}