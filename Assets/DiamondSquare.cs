using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour
{
    public int mDivisions;
    public float mSize;
    public float mHeight;
	public Shader shader;
	public PointLight pointLight;
    public Texture2D texture;
    public Material material;
    

	public Vector3[] mVerts;
    public Vector2 MinAndMax;
    
    int mVertCount;

    public static float OutSize;
    public static float OutHeight;


    // Start is called before the first frame update
    void Start()
    {
        OutSize = mSize;
        OutHeight = mHeight;
		MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        Mesh mesh = CreateTerrain();
        terrainMesh.mesh = mesh;

      
		MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        //renderer.material.mainTexture = texture;
        

        renderer.material.shader = shader;

        renderer.material.SetFloat("_Height1", Mathf.Lerp(MinAndMax.x, MinAndMax.y, 0.8f));
        renderer.material.SetFloat("_Height2", Mathf.Lerp(MinAndMax.x, MinAndMax.y, 0.6f));



        //renderer.material = material;

        renderer.material.mainTexture = texture;
 


    }
    

    void Update()
	{
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
		renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }
    // Update is called once per frame
    Mesh CreateTerrain()
    {
        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;

        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                //halfsize can make the mesh locate in the center
                //uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

                    triOffset += 6;
                }
            }
        }
        mVerts[0].y = Random.Range(-mHeight, mHeight);
        mVerts[mDivisions].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-mHeight, mHeight);

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquare = 1;
        int squareSize = mDivisions;
        for(int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquare; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquare; k++)
                {
                    ApplyDiamondSquare(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquare *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;

        }
        MinAndMax = GetMaxMinHeight();

        //Desert
        Vector2 DesertX = new Vector2(0.0f, 0.5f);
        Vector2 DesertY = new Vector2(0.0f, 1.0f);
        Vector2 DesertZ = new Vector2(0.5f, 1.0f);

        //Grass
        Vector2 GrassX = new Vector2(0.0f, 0.0f);
        Vector2 GrassY = new Vector2(0.0f, 0.5f);
        Vector2 GrassZ = new Vector2(0.5f, 0.5f);


        //Snow
        Vector2 SnowX = new Vector2(0.5f, 0.5f);
        Vector2 SnowY = new Vector2(0.5f, 1.0f);
        Vector2 SnowZ = new Vector2(1.0f, 1.0f);

        float YValueOne = (MinAndMax.y - MinAndMax.x) * 0.45f + MinAndMax.x;
        float YValueTwo = (MinAndMax.y - MinAndMax.x) * 0.8f + MinAndMax.x;



        for (int i = 0; i < tris.Length; i+=3)
        {
            int tempX = tris[i];
            int tempY = tris[i+1];
            int tempZ = tris[i+2];

            float AverageHeight = (mVerts[tempX].y + mVerts[tempY].y + mVerts[tempZ].y)/3;

            if (AverageHeight < YValueOne)
            {
                uvs[tempX] = GrassX;
                uvs[tempY] = GrassY;
                uvs[tempZ] = GrassZ;
            }
            else if(AverageHeight < YValueTwo)
            {
                uvs[tempX] = DesertX;
                uvs[tempY] = DesertY;
                uvs[tempZ] = DesertZ;
            }
            else
            {
                uvs[tempX] = SnowX;
                uvs[tempY] = SnowY;
                uvs[tempZ] = SnowZ;
            }
        }




        mesh.vertices = mVerts;
		
        mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


        MeshCollider myMC = this.gameObject.AddComponent<MeshCollider>();

        myMC.sharedMesh = mesh;

        return mesh;
    }

    void ApplyDiamondSquare(int row, int col, int size,float offset)
    {
        int halfSize = (int)(size * 0.5f);
        int topLeft = row * (mDivisions + 1) + col;
        int botLeft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
    
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y) * 0.25f + Random.Range(-offset, 1.1f*offset);

        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y)/3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }

    Vector2 GetMaxMinHeight()
    {
        float minVector = float.PositiveInfinity;
        float maxVector = float.NegativeInfinity;

        for (int i = 0; i< mVerts.Length; i++)
        {
            if (mVerts[i].y < minVector)
            {
                minVector = mVerts[i].y;
            }
            if (mVerts[i].y > maxVector)
            {
                maxVector = mVerts[i].y;
            }
        }
        return new Vector2(minVector, maxVector);
    }
   
}
