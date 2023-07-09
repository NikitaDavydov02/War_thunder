using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapAIManager : MonoBehaviour
{
    [SerializeField]
    List<Vector3> controlPoints;
    [SerializeField]
    List<Vector2> connections;
    [SerializeField]
    List<int> indexesOfPositions;
    System.Random r;
    // Start is called before the first frame update
    void Start()
    {
        r = new System.Random();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Queue<Vector3> GetRoadToRandomPosotion(Vector3 start)
    {
        int posIndex = GetIndexOfRandomPosition();
        Vector3 end = controlPoints[posIndex];
        return FindPath(start, end);
    }
    private int GetIndexOfRandomPosition()
    {
        return r.Next(0, indexesOfPositions.Count);
    }
    public Queue<Vector3> FindPath(Vector3 start, Vector3 end)
    {

    }
}
