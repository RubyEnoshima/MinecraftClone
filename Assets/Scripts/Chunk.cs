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
        chunk = new GameObject[maxProfundidad, maxHeight, maxWidth];
        chunk.Initialize();
    }

    // Devuelve una lista de vecinos de la coordenada dada.
    public List<GameObject> ObtenerVecinos(int x, int y, int z){
        List<GameObject> res = new List<GameObject>();
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
                    chunk[x,y,z].GetComponent<Cubo>().QuitarCaras(ObtenerVecinos(x,y,z));
                }
            }
        }
    }

    GameObject GenerarCubo(int x,int y,int z,int xIni,int yIni,int zIni){
        GameObject cuboIns = Instantiate(cubo,new Vector3(x+xIni, y+yIni, z+zIni), Quaternion.identity);
        cuboIns.transform.parent = this.transform;
        cuboIns.name = "Cubo"+x.ToString()+y.ToString()+z.ToString();
        cuboIns.GetComponent<Cubo>().chunk = this;
        cuboIns.GetComponent<Cubo>().posChunk = new Vector3(x,y,z);
        return cuboIns;
    }

    // Genera un chunk plano sin relieve ninguno, del height, width y profundidad del chunk
    public void ChunkLineal(){
        int zIni = (int)Position.z;
        int yIni = (int)Position.y;
        int xIni = (int)Position.x;
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    chunk[x,y,z] = GenerarCubo(x,y,z,xIni,yIni,zIni);;
                }
            }
        }
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
