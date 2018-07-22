using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TileMaker : MonoBehaviour {
    public Transform tile;
    public int minimumEndPointDistance;
    public int maximumEndPointDistance;
    public int numIntermediatePoints;
    

    // Use this for initialization

    private Vector3 endPoint;


	void Start () {
        // Let us assume the size of a tile is an int. This allows us to round to multiples of it
        // more easily by leveraging integer arithmetic. 
        int tileSize = (int)tile.transform.localScale.x;
        Vector2 rand = Random.insideUnitCircle;
        rand = rand.normalized * minimumEndPointDistance + rand * (maximumEndPointDistance - minimumEndPointDistance);
        // Make the vector aligned to even numbers (tiles are 2x2), and plus I like even numbers
        rand = RoundToMultipleOf(rand, tileSize);

        endPoint = new Vector3(rand.x, 0, rand.y);
        List<Vector3> points = new List<Vector3>();
        
        if(numIntermediatePoints > 0)
        {
            float intermediatePointStep = endPoint.magnitude/(numIntermediatePoints+1);
            for(int i = 1; i <= numIntermediatePoints; i++)
            {
                Vector2 intermediateRand = Random.insideUnitCircle;
                rand = intermediateRand.normalized * intermediatePointStep * i;
                rand = RoundToMultipleOf(rand, tileSize);
                points.Add(new Vector3(rand.x, 0, rand.y));
            }
        }
        points.Add(endPoint);
        Vector3 prev = new Vector3(0, 0, 0);
        foreach (Vector3 pt in points)
        {
            Debug.Log("Prev " + prev);
            Debug.Log("pt " + pt);
            // Draw a path from the x position of the previous point to the x position of 
            // the next point. Use the y valueo f the previous point.
            
            for(float x = Mathf.Min(pt.x, prev.x); x <= Mathf.Max(pt.x, prev.x); x+= tileSize)
            {
                Debug.Log("Creating tile: " + new Vector3(x, 0, prev.z));
                Instantiate(tile, new Vector3(x, 0, prev.z), Quaternion.identity);
            }

            // Draw a path from where we left off above to the x, y position of the
            // next point. Use the x value of the new point because we've walked above
            // to that x position.
            for(float z = Mathf.Min(pt.z, prev.z); z <= Mathf.Max(pt.z, prev.z) ; z += tileSize)
            {
                Instantiate(tile, new Vector3(pt.x, 0, z), Quaternion.identity);
            }
            prev = pt;
        }
        

        
    }
    // Sets the x and y values of a vector2 to the nearest multiple of num.
	private Vector2 RoundToMultipleOf(Vector2 vec, int num)
    {
        vec.x = Mathf.Round(vec.x / num) * num;
        vec.y = Mathf.Round(vec.y / num) * num;
        return vec;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
