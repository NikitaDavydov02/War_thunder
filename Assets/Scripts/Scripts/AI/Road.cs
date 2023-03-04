using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour {
    [SerializeField]
    public List<Vector3> controlPoints;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public float Distance(int startPoint, int endPoint)
    {
        float distande = 0;
        if (startPoint < endPoint)
        {
            for(int i = startPoint + 1; i <= endPoint; i++)
            {
                float add = Mathf.Abs(Vector3.Magnitude(controlPoints[i] - controlPoints[i - 1]));
                distande += add;
            }
        }
        if (startPoint > endPoint)
        {
            for (int i = endPoint + 1; i <= startPoint; i++)
            {
                float add = Mathf.Abs(Vector3.Magnitude(controlPoints[i] - controlPoints[i - 1]));
                distande += add;
            }
        }
        if (distande != 0)
            return distande;
        else
            return 0;
    }
}
