using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BrickStructure
{
    Dictionary<Vector2Int, int> bricks = new Dictionary<Vector2Int, int>();
    List<Vector2Int> hollows = new List<Vector2Int>();
    
    public List<Vector2Int> AddBricks(Vector2Int center, Wasp wasps)
    {
        List<Vector2Int> newBricks = new List<Vector2Int>();
        for(int i = -1; i < 2; i++){
            for(int j = -1; j < 2; j++){
                Vector2Int pos = new Vector2Int(i,j) + center;
                if(!bricks.ContainsKey(pos)) {
                    if(wasps.template[i+1, j+1] != 0) {
                        newBricks.Add(pos);
                        bricks.Add(pos, wasps.template[i+1, j+1]);
                    }
                }
            }
        }

        return newBricks;
    }

    public bool TemplateFitIn(Vector2Int center, Wasp wasp)
    {
        bool b;
        return TemplateFitIn(center, wasp, null, out b);
    }

    public bool TemplateFitIn(Vector2Int center, Wasp wasp, HashSet<Vector2Int> shape, out bool dontFitShape)
    {
        bool mismatch = false;
        for(int i = -1; i < 2; i++){
            for(int j = -1; j < 2; j++){
                Vector2Int pos = new Vector2Int(i,j) + center;
                if(shape != null && !shape.Contains(pos)) {// && template.template[i + 1, j + 1] != 0) {
                    dontFitShape = true;
                    return false;
                }
                if(bricks.ContainsKey(pos) && bricks[pos] != wasp.template[i + 1, j + 1]) {
                    mismatch = true;
                }
            }
        }
        dontFitShape = false;
        return !mismatch;
    }

    public void RecalculateHollows(List<Vector2Int> newBricks)
    {
        foreach (var brick in newBricks)
        {
            hollows.Add(brick);
        }
    }

    public bool HaveBrickIn(Vector2Int pos) {
        return bricks.ContainsKey(pos);
    }

    public int GetBrick(Vector2Int pos) {
        return bricks[pos];
    }

    public void AddBrick(Vector2Int position, int value) {
        bricks.Add(position, value);
    }

    public void AddBricks(Dictionary<Vector2Int, int> bricks) {
        this.bricks.Union(bricks);
    }

    public Dictionary<Vector2Int, int> GetBricks()
    {
        return bricks;
    }

    public bool HaveHollows() {
        return hollows.Count > 0;
    }

    public Vector2Int PopFirstHollow() {
        Vector2Int hollow = hollows[0];
        hollows.RemoveAt(0);
        return hollow;
    }

}
