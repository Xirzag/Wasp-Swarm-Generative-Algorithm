using System.Collections.Generic;
using UnityEngine;

class TreeShape : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {

        int treeHeight = 5 + seed % 10;
        int treeRadius = 2 + seed % 2;

        int leafsRadius = 5 + seed % 3;
        int smallLeafsRadius = 1 + seed % 2;

        int center_y = treeHeight + leafsRadius;

        var leafs = CircleOutline.MidPointCircleDraw(0, center_y, leafsRadius);

        int treeWoodStart = 0;

        foreach (Vector2Int leaf in leafs) {
            if(leaf.x == treeRadius || leaf.x == -treeRadius) {
                treeWoodStart = center_y 
                            - UnityEngine.Mathf.Abs(leaf.y - center_y) + 1;
                break;
            }
        }

        HashSet<Vector2Int> tree = new HashSet<Vector2Int>();

        for(int i = 0; i < treeWoodStart; i++) {
            tree.Add(new Vector2Int(-treeRadius, i));
            tree.Add(new Vector2Int( treeRadius, i));
        }

     
        tree.UnionWith(leafs);

        var smallLeafs = CircleOutline.MidPointCircleDraw(treeRadius + smallLeafsRadius,
                 treeWoodStart / 2, smallLeafsRadius);

        tree.UnionWith(leafs);
        tree.UnionWith(smallLeafs);
        
        return tree;
        
    }

}