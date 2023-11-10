using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldOfView : MonoBehaviour
{
 
    private Mesh mesh;
    private Vector3 origin;
    private float startingAngle;
    private float fov;
 
    private Vector3 GetVectorFromAngle(float angle){
        float angleRad= angle*(Mathf.PI/180f);

        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    private float GetAngleFromVectorFloat(Vector3 dir){
        dir= dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n>0) n+=360;

        return n;
    }

    private void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh=mesh;
        
        fov = 90f;
        this.origin=Vector3.zero;
    }

    void Update()
    {
        Vector3 origin = Vector3.zero;
        int rayCount=10;
        float angle =startingAngle;
        float angleIncrease = fov/rayCount;
        float viewDistance=50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int [] triangles = new int[rayCount * 3];

        vertices[0]=origin;


        int vertexIndex=1;
        int triangleIndex = 0;
        for (int i=0; i<= rayCount; i++){
            Vector3 vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i>0){
                triangles[triangleIndex + 0]=0;
                triangles[triangleIndex + 1]=vertexIndex - 1;
                triangles[triangleIndex + 2]=vertexIndex;

                triangleIndex+=3;
            }
            vertexIndex++;
            angle -= angleIncrease;
        }


        mesh.vertices=vertices;
        mesh.uv = uv;
        mesh.triangles=triangles;
        
        foreach (var item in vertices)
        {
            Debug.Log(item);
            
        }
    }

    public void SetOrigin(Vector3 origin){
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection){
        startingAngle = GetAngleFromVectorFloat(aimDirection) - fov/2f;
    }

    
}
