using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class MapAIManager : MonoBehaviour
{
    [SerializeField]
    List<Vector3> controlPoints;
    [SerializeField]
    List<int> indexesOfRedPositionsInControlPoints;
    [SerializeField]
    List<int> indexesOfBluePositionsInControlPoints;
    System.Random r;
    public float HeightTargetCorrection = 1f;
    // Start is called before the first frame update
    private void Awake()
    {
        r = new System.Random();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector3 GetRandomPositionFor(bool isRed)
    {
        int posIndex = GetIndexOfRandomPosition(isRed);
        Vector3 end;
        if (isRed)
            end = controlPoints[indexesOfRedPositionsInControlPoints[posIndex]];
        else
            end = controlPoints[indexesOfBluePositionsInControlPoints[posIndex]];
        return end;
    }
    public Queue<Vector3> GetRoadToRandomPosotion(Vector3 start, bool forRed)
    {
        return FindPath(start, GetRandomPositionFor(forRed));
    }
    private int GetIndexOfRandomPosition(bool forRed)
    {
        if(forRed)
            return r.Next(0, indexesOfRedPositionsInControlPoints.Count-1);
        else
            return r.Next(0, indexesOfBluePositionsInControlPoints.Count - 1);
        //return 0;
    }
    public Queue<Vector3> FindPath(Vector3 start, Vector3 end)
    {
        ////Debug.Log("Finding path...");
        ////Debug.Log("Start " + start);
        ////Debug.Log("End " + end);
        NavMeshPath path = new NavMeshPath();
        bool result = NavMesh.CalculatePath(start, end, NavMesh.AllAreas, path);
        if (result)
            ////Debug.Log("Pathis find");
        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            //Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red, 1000);
           // //Debug.Log("Point");
        }
           
        Queue<Vector3> output = new Queue<Vector3>();
        foreach (Vector3 v in path.corners)
            output.Enqueue(v);
        return output;
    }
    public Vector3 CalculateGunDirectionOnTarget(Vector3 targetPosition, float curbSpeed, Vector3 targetVelocity)
    {
        targetPosition += Vector3.up * HeightTargetCorrection;
        float v0 = curbSpeed;
        float z0t = targetPosition.y;
        float d = targetPosition.magnitude;
        float g = 9.81f;
        float time = Mathf.Sqrt(2 * ((curbSpeed * curbSpeed - z0t * g - Mathf.Sqrt((z0t * g - v0 * v0) * (z0t * g - v0 * v0) - (g * g * d * d))) / g));
        time = d / curbSpeed;
        //Debug.Log("Time of flying: " + time);
        targetPosition += targetVelocity * time;
        /**float ex = targetPosition.x / (curbSpeed * time);
        float ey = targetPosition.y / (curbSpeed * time);
        float ez = (targetPosition.z + g * time * time / 2) / (curbSpeed * time);*/
        float ex = targetPosition.x / (curbSpeed * time);
        float ez = targetPosition.z / (curbSpeed * time);
        float ey = (targetPosition.y + g * time * time / 2) / (curbSpeed * time);
        return new Vector3(ex, ey, ez);
    }
}
