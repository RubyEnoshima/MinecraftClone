using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class World : MonoBehaviour
{
    public GameObject ChunkPrefab;
    public GameObject Cubo;
    public float DistanciaRenderizado = 12f;
    // Pseudo Random Number Generator
    private System.Random pseudoRandom;

    // Chunks guardados
    public Dictionary<Vector2, Chunk> Chunks = new Dictionary<Vector2, Chunk>();
    public List<Chunk> ActiveChunks = new List<Chunk>();


    //============================================================
    // Set Warm-Up Data
    //============================================================
    private void Awake() {
        // Get/Create Seed
        // if (Seed == ""){
        //     Seed = GenerateRandomSeed();
        // }
        // // Get Random Number Generator
        // pseudoRandom = new System.Random(Seed.GetHashCode());
        //  // Set Offsets from seed (new world each time)
        // xOffset = pseudoRandom.Next(-10000, 10000);
        // yOffset = pseudoRandom.Next(-10000, 10000);

        // // Using to Clear while Making Test Adjustments
        // chunks.Clear();
        // ActiveChunks.Clear();
        // // Generate Starting Chunk
        // for (int x = -1; x <= 1; x++)
        // {
        //     for (int y = -1; y <= 1; y++)
        //     {
        //         // Draw Test Chunks
        //         GenerateChunk(x, y);
        //         Chunk c = GetChunk(x,y);
        //         // Enable the Chunk and Add it To The Active List
        //         c.enabled = true;
        //         c.cubo = Cubo;
        //         c.ChunkNoise(Cubo);
        //         ActiveChunks.Add(c);
        //     }
        // }
        const int height = 16;
        const int width = 16;
        
        for (int x = -1*width; x <= 1*width; x+=width)
        {
            for (int y = -1*height; y <= 1*height; y+=height)
            {
                GameObject nuevoChunk = Instantiate(ChunkPrefab,new Vector3(x,0,y),Quaternion.identity);
                Chunk c = nuevoChunk.GetComponent<Chunk>();
                c.ChunkLineal();
                c.PosWorld = new Vector2(x/width,y/height);
                nuevoChunk.transform.position = Vector3.zero;
                ActiveChunks.Add(c);
                Chunks.Add(c.PosWorld,c);
            }
        }
    }

    private void Update() {
        // Vector2 chunkPos = new Vector2(0, 0);
        // // Get X/Y Offsets for camera to correctly load new chunks (this will be updated to work with camera bounds later instead this is just temporary)
        // if (Camera.main.transform.position.x >= 0){
        //     chunkPos.x = (int)((Camera.main.transform.position.x+ChunkSize/2) / ChunkSize);
        // } else {
        //     chunkPos.x = (int)((Camera.main.transform.position.x-ChunkSize/2) / ChunkSize);
        // }

        // if (Camera.main.transform.position.y >= 0){
        //     chunkPos.y = (int)((Camera.main.transform.position.y+ChunkSize/2) / ChunkSize);
        // } else {
        //     chunkPos.y = (int)((Camera.main.transform.position.y-ChunkSize/2) / ChunkSize);
        // }

        // // Generate new Chunks if they don't exist & Enable Active Chunks 
        // EnableChunks(chunkPos);
    }

    // ===
    //  Generate Random Seed of Length
    // ===
    private string GenerateRandomSeed(int maxCharAmount = 10, int minCharAmount = 10){
        //Set Characters To Pick from
        const string glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";
        //Set Length from min to max
        int charAmount = UnityEngine.Random.Range(minCharAmount, maxCharAmount);
        // Set output Variable
        string output = "";
        // Do Random Addition
        for(int i=0; i<charAmount; i++)
        {
            output += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
        }
        // Output New Random String
        return output;
    }
        
}
