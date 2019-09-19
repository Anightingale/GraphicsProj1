using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WavesGenerator : MonoBehaviour
{
    public float Size = 9.5f;
    public float Height = 1.5f;
    public int numSideVertices = 200;
    public Shader shader;
    public PointLight pointLight;
    public Texture2D texture;
    


    // Start is called before the first frame update
    void Start()
    {
        MeshFilter waterMesh = this.gameObject.AddComponent<MeshFilter>();
        Mesh mesh = CreateWaterMesh();
        waterMesh.mesh = mesh;

        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();


        //change size and position of mesh to fit landscape


        renderer.material.shader = shader;
        renderer.material.mainTexture = texture;

        MeshCollider myMC = this.gameObject.AddComponent<MeshCollider>();

        myMC.sharedMesh = mesh;

        GameObject terrain = GameObject.Find("Terrain");
        DiamondSquare script = terrain.GetComponent<DiamondSquare>();
        Vector2 MinAndMax = script.MinAndMax;
        float average = (MinAndMax.x + MinAndMax.y)/2;
        transform.localPosition = new Vector3(-50.0f, average, -50.0f);
        transform.localScale += new Vector3(Size, Size, Size);
    }

    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }


    //generate a flat mesh for the water so that there is sufficient amount of vertices
    Mesh CreateWaterMesh()
    {
        Vector3[] vertices;
        int[] triangles;
        Vector2[] uvs;

        float distBetweenVertices = Size / numSideVertices;

        //Sets vertices row by row order to form a plane mesh.
        vertices = new Vector3[numSideVertices * numSideVertices];
        int currVertex = 0;
        for (int zAxis = 0; zAxis < numSideVertices; zAxis++)
        {
            for (int xAxis = 0; xAxis < numSideVertices; xAxis++)
            {
                vertices[currVertex] = new Vector3(zAxis * distBetweenVertices, 0, xAxis * distBetweenVertices);
                currVertex++;
            }
        }

        //creates clockwise triangles in the mesh using the vertices
        //each square in the mesh contains 2 triangles, 3 vertices each triangles, therefore, 6 items stored per square
        //formula for number of vertex indices that need to be stored = ((numVerticesPerSide-1)^2)*6
        triangles = new int[((numSideVertices - 1) * (numSideVertices - 1)) * 6];
        int currTriangle = 0;
        currVertex = 0;
        for (int zAxis = 0; zAxis < numSideVertices - 1; zAxis++)
        {
            for (int xAxis = 0; xAxis < numSideVertices - 1; xAxis++)
            {
                //storing indices for order of vertices
                //vertices stored like (0.0, 0.0, 0.0) (0.0, 0.0, 0.1) (0.0, 0.0, 0.2)

                /*
                //2 clockwise triangles to form a square
                //triangle1
                triangles[currTriangle] = currVertex; 
                triangles[currTriangle + 1] = currVertex + numSideVertices; // add numSideVertices currVertex to get vertex directly below currVertex
                triangles[currTriangle + 2] = currVertex + 1; // add 1 to currVertex to get vertex directly next to currVertex

                //triangle 2
                triangles[currTriangle + 3] = currVertex + 1;
                triangles[currTriangle + 4] = currVertex + numSideVertices;
                triangles[currTriangle + 5] = currVertex + numSideVertices + 1; // add numSideVertices+1 to currVertex to get vertex diagonal to currVertex

                currTriangle += 6;
                currVertex++;
                */

                triangles[currTriangle] = currVertex;
                triangles[currTriangle + 1] = currVertex + 1; // add numSideVertices currVertex to get vertex directly below currVertex
                triangles[currTriangle + 2] = currVertex + numSideVertices; // add 1 to currVertex to get vertex directly next to currVertex

                //triangle 2
                triangles[currTriangle + 3] = currVertex + 1;
                triangles[currTriangle + 4] = currVertex + numSideVertices + 1;
                triangles[currTriangle + 5] = currVertex + numSideVertices; // add numSideVertices+1 to currVertex to get vertex diagonal to currVertex

                currTriangle += 6;
                currVertex++;
            }
            currVertex++;
        }


        //update uv so texture to fits mesh
        //Mesh.uv is an array of Vector2s that can have values between (0,0) and (1,1)
        uvs = new Vector2[numSideVertices * numSideVertices];
        currVertex = 0;
        for (int zAxis = 0; zAxis < numSideVertices; zAxis++)
        {
            for (int xAxis = 0; xAxis < numSideVertices; xAxis++)
            {
                uvs[xAxis+(zAxis * numSideVertices)].x = (float)xAxis/numSideVertices;
                uvs[xAxis+(zAxis * numSideVertices)].y = (float)zAxis/numSideVertices;

                currVertex++;
            }
        }



        //Update mesh
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

      

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.uv = uvs;

        mesh.RecalculateNormals();

        return mesh;
    }
}
