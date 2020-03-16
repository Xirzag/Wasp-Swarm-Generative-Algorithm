using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGenerator : MonoBehaviour
{

    public int maxIter = 20;

    // Start is called before the first frame update
    public void Generate(List<Wasp> wasps) {
        
        BrickStructure structure = new BrickStructure();

        var bricks = InitStructure(structure);
        structure.RecalculateHollows(bricks);

        int iter = 0;

        while(structure.HaveHollows() && iter < maxIter) {

            List<Wasp> posibleTemplates = new List<Wasp>();
            Vector2Int hollow = structure.PopFirstHollow();

            foreach (var wasp in wasps)
            {
                if(structure.TemplateFitIn(hollow, wasp)) {
                    posibleTemplates.Add(wasp);
                }

            }

            if(posibleTemplates.Count > 0) {
                var newBricks = structure.AddBricks(hollow, Wasp.Select(posibleTemplates));
                if(newBricks.Count > 0)
                    structure.RecalculateHollows(newBricks);
            }
            iter++;

        }

        GetComponent<StructureRenderer>().Render(structure);

    }

    private List<Vector2Int> InitStructure(BrickStructure structure)
    {
        structure.AddBrick(Vector2Int.zero, 1);
        return new List<Vector2Int>(){Vector2Int.zero};
    }
}
