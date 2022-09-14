using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public World World;
    public GameObject cubo;
    public Vector3 Position;
    public Vector2 PosWorld;

    public int maxWidth = 16;
    public int maxHeight = 125;
    public int maxProfundidad = 16;
    const int width = 16;
    const int height = 2;
    const int profundidad = 16;

    public GameObject[,,] chunk;

    void Awake(){
        chunk = new GameObject[maxWidth, maxHeight, maxProfundidad];
        chunk.Initialize();
    }

    // Devuelve una lista de vecinos de la coordenada dada.
    public List<GameObject> ObtenerVecinos(int x, int y, int z){
        List<GameObject> res = new List<GameObject>();
        // GameObject cuboAire = new GameObject("temp");
        // cuboAire.AddComponent<Cubo>();
        // cuboAire.GetComponent<Cubo>().tipo = "aire";
        for(int i=-1;i<=1;i+=2){
            if(x+i>=0 && x+i<width)res.Add(chunk[x+i,y,z]);
            else{
                if(x+i<0){ // Si es una x negativa, es que estamos en el chunk anterior
                    // Por tanto añadimos el cubo anterior, que es el del chunk vecino, el ultimo de todos (15, width-1)
                    Vector2 vecino = new Vector2(PosWorld.x-1,PosWorld.y);
                    if(World.ActiveChunks.ContainsKey(vecino))
                        res.Add(World.ActiveChunks[vecino].chunk[width-1,y,z]);
                    else res.Add(null);
                }else{
                    Vector2 vecino = new Vector2(PosWorld.x+1,PosWorld.y);
                    if(World.ActiveChunks.ContainsKey(vecino))
                        res.Add(World.ActiveChunks[vecino].chunk[0,y,z]);
                    else res.Add(null);
                }
            }

            if(y+i>=0 && y+i<maxHeight)res.Add(chunk[x,y+i,z]);
            else res.Add(null);

            if(z+i>=0 && z+i<profundidad)res.Add(chunk[x,y,z+i]);
            else {
                if(z+i<0){

                    Vector2 vecino = new Vector2(PosWorld.x,PosWorld.y-1);
                    if(World.ActiveChunks.ContainsKey(vecino))
                        res.Add(World.ActiveChunks[vecino].chunk[x,y,profundidad-1]);
                    else res.Add(null); 
                }else{
                    Vector2 vecino = new Vector2(PosWorld.x,PosWorld.y+1);
                    if(World.ActiveChunks.ContainsKey(vecino))
                        res.Add(World.ActiveChunks[vecino].chunk[x,y,0]);
                    else res.Add(null);
                }
            }
        }
        return res;
    }

    public List<GameObject> ObtenerVecinos(Vector3 pos){
        int z = (int)pos.z;
        int y = (int)pos.y;
        int x = (int)pos.x;
        return ObtenerVecinos(x,y,z);
    }

    public void QuitarCaras(){
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    chunk[x,y,z].GetComponent<Cubo>().StartQuitarCaras(ObtenerVecinos(x,y,z));
                }
            }
        }
    }

    public GameObject GenerarCubo(int x,int y,int z,string tipo = "cobble"){
        GameObject cuboIns = Instantiate(cubo,new Vector3(x+(int)Position.x, y+(int)Position.y, z+(int)Position.z), Quaternion.identity);
        cuboIns.transform.parent = this.transform;
        Cubo cuboComp = cuboIns.GetComponent<Cubo>();
        cuboComp.chunk = this;
        cuboComp.posChunk = new Vector3(x,y,z);
        cuboIns.name = "Cubo"+cuboComp.posChunk.ToString();
        cuboComp.CambiaTipo(tipo);
        return cuboIns;
    }

    public GameObject GenerarCubo(int x,int y,int z,Vector3 posChunkNueva,string tipo = "cobble"){
        GameObject cuboIns = Instantiate(cubo,new Vector3(x+(int)Position.x, y+(int)Position.y, z+(int)Position.z), Quaternion.identity);
        cuboIns.transform.parent = this.transform;
        Cubo cuboComp = cuboIns.GetComponent<Cubo>();
        cuboComp.chunk = this;
        cuboComp.posChunk = posChunkNueva;
        cuboIns.name = "Cubo"+cuboComp.posChunk.ToString();
        cuboComp.CambiaTipo(tipo);
        chunk[(int)posChunkNueva.x,(int)posChunkNueva.y,(int)posChunkNueva.z] = cuboIns;
        return cuboIns;
    }

    // Genera un chunk plano sin relieve ninguno, del height, width y profundidad del chunk
    public void ChunkLineal(){
        
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    chunk[x,y,z] = GenerarCubo(x,y,z,"grass");
                }
            }
        }
        // for(int z=0;z<profundidad;z++){
        //     for(int y=height;y<maxHeight;y++){
        //         for(int x=0;x<width;x++){
        //             chunk[x,y,z] = GenerarCubo(x,y,z,xIni,yIni,zIni,"aire");
        //         }
        //     }
        // }
    }

    public void ChunkNoise(GameObject Cubo){
        
    }

    public bool PosValida(Vector3 pos){
        return pos.x>=0 && pos.x<width && pos.y>=0 && pos.y<height && pos.z>=0 && pos.z<profundidad;
    }

    // Start is called before the first frame update
    void Start()
    {
        Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
