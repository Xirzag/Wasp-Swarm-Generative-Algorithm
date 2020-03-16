using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRenderer : MonoBehaviour
{

    public GameObject brickPrefab;
    List<GameObject> renderedBricks = new List<GameObject>();

    public void Render(BrickStructure structure)
    {
        var bricks = structure.GetBricks();
        foreach (var brick in renderedBricks)
        {
            Destroy(brick);    
        }
        
        foreach (var brick in bricks)
        {
            if (brick.Value != 0) {
                Vector3 position = new Vector3(brick.Key.x, brick.Key.y, 0);
                var g = Instantiate(brickPrefab, transform.position + position, transform.rotation);
                renderedBricks.Add(g);
                g.transform.parent = transform;
            }
        }
    }

}
