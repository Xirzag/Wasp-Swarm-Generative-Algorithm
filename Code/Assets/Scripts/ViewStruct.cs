using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewStruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var shape = new TreeShape().GetShape(3);
        var bs = new BrickStructure();
        var d = new Dictionary<Vector2Int, int>();
        foreach (var item in shape)
        {
            bs.AddBrick(item, 1);
        }
        // Debug.Log(bs.GetBricks().Count);
        GetComponent<StructureRenderer>().Render(bs);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
