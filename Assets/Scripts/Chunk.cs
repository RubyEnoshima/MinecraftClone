using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject cubo;
    public Vector3 Position;
    public Vector2 PosWorld;

    public int maxWidth = 16;
    public int maxHeight = 16;
    public int maxProfundidad = 125;
    const int width = 16;
    const int height = 16;
    public int profundidad = 1;
    public float[,] Sample;

    public GameObject[,,] chunk;
    // List<List<List<GameObject>>> chunk;

    void Awake(){
        chunk = new GameObject[maxProfundidad, maxHeight, maxWidth];
        Sample = new float[maxHeight+1, maxHeight+1];
    }

    // Devuelve una lista de vecinos de la coordenada dada.
    // La lista no puede obtener huecos, y siempre tiene el mismo orden: arriba, abajo, izq, der, del, detr
    public List<GameObject> ObtenerVecinos(int z, int y, int x){
        List<GameObject> res = new List<GameObject>();
        if(z+1<profundidad) res.Add(chunk[z+1,y,x]);
        else res.Add(null);
        if(z-1>=0) res.Add(chunk[z-1,y,x]);
        else res.Add(null);
        if(y-1>=0) res.Add(chunk[z,y-1,x]);
        else res.Add(null);
        if(y+1<height) res.Add(chunk[z,y+1,x]);
        else res.Add(null);
        if(x+1<width) res.Add(chunk[z,y,x+1]);
        else res.Add(null);
        if(x-1>=0) res.Add(chunk[z,y,x-1]);
        else res.Add(null);
        return res;
    }

    public List<GameObject> ObtenerVecinos(Vector3 pos){
        int z = (int)pos.x;
        int y = (int)pos.y;
        int x = (int)pos.z;
        return ObtenerVecinos(z,y,x);
    }

    void QuitarCaras(){
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    chunk[z,y,x].GetComponent<Cubo>().QuitarCaras(ObtenerVecinos(z,y,x));
                }
            }
        }
    }

    // Genera un chunk plano sin relieve ninguno, del height, width y profundidad del chunk
    public void ChunkLineal(){
        int zIni = (int)Position.z;
        int yIni = (int)Position.y;
        int xIni = (int)Position.x;
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    GameObject cuboIns = Instantiate(cubo,new Vector3(x+xIni, z+zIni, y+yIni), Quaternion.identity);
                    cuboIns.transform.parent = this.transform;
                    cuboIns.name = "Cubo"+z.ToString()+y.ToString()+x.ToString();
                    cuboIns.GetComponent<Cubo>().chunk = this;
                    cuboIns.GetComponent<Cubo>().posChunk = new Vector3(z,y,x);
                    cuboIns.GetComponent<Cubo>().QuitarCaras(ObtenerVecinos(z,y,x));
                    chunk[z,y,x] = cuboIns;
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
        // ChunkLineal();
        //ChunkNoise();
        // QuitarCaras();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
