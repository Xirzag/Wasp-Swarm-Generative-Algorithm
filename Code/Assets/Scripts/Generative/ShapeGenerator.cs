using System.Collections.Generic;
using UnityEngine;

public interface ShapeGenerator {
    
    HashSet<Vector2Int> GetShape(int seed);

}