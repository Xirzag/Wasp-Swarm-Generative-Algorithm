using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class CircleOutline : ShapeGenerator
{

    public HashSet<Vector2Int> GetShape(int seed)
    {

        int r = 5 + seed % 10;
        
        return MidPointCircleDraw(0, 0, r);
        
    }

    // Implementing Mid-Point Circle 
    // Drawing Algorithm 
    public static HashSet<Vector2Int> MidPointCircleDraw(int x_centre,  
                            int y_centre, int r)  
    { 

        HashSet<Vector2Int> shape = new HashSet<Vector2Int>();
          
        int x = r, y = 0; 
      
        // Printing the initial point on the 
        // axes after translation 
        shape.Add(new Vector2Int(x + x_centre, y + y_centre));
        shape.Add(new Vector2Int(-x + x_centre, y + y_centre));
        shape.Add(new Vector2Int(x_centre, r + y_centre));
        shape.Add(new Vector2Int(x_centre, -r + y_centre));

      
        // Initialising the value of P 
        int P = 1 - r; 
        while (x > y) 
        { 
              
            y++; 
          
            // Mid-point is inside or on the perimeter 
            if (P <= 0) 
                P = P + 2 * y + 1; 
          
            // Mid-point is outside the perimeter 
            else
            { 
                x--; 
                P = P + 2 * y - 2 * x + 1; 
            } 
          
            // All the perimeter points have already  
            // been printed 
            if (x < y) 
                break; 
          
            // Printing the generated point and its  
            // reflection in the other octants after 
            // translation 
            shape.Add(new Vector2Int(x + x_centre, y + y_centre));
            shape.Add(new Vector2Int(-x + x_centre, y + y_centre));
            shape.Add(new Vector2Int(x + x_centre, -y + y_centre));
            shape.Add(new Vector2Int(-x + x_centre, -y + y_centre));
          
            // If the generated point is on the  
            // line x = y then the perimeter points 
            // have already been printed 
            if (x != y)  
            { 
                shape.Add(new Vector2Int(y + x_centre, x + y_centre));
                shape.Add(new Vector2Int(-y + x_centre, x + y_centre));
                shape.Add(new Vector2Int(y + x_centre, -x + y_centre));
                shape.Add(new Vector2Int(-y + x_centre, -x + y_centre));
            }

        } 

        return shape; 
    } 

}