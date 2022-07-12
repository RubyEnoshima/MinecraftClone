using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject cubo;

    public int maxWidth = 16;
    public int maxHeight = 16;
    public int maxProfundidad = 125;
    public int width = 5;
    public int height = 3;
    public int profundidad = 1;

    public GameObject[,,] chunk;
    // List<List<List<GameObject>>> chunk;

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


    // Start is called before the first frame update
    void Start()
    {
        // if(width>maxWidth || height>maxHeight || profundidad>maxProfundidad) throw new Exception("No puede tener mas que el maximo");
        chunk = new GameObject[maxProfundidad, maxHeight, maxWidth];
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    GameObject cuboIns = Instantiate(cubo,new Vector3(x, z, y), Quaternion.identity);
                    cuboIns.transform.parent = this.transform;
                    cuboIns.name = "Cubo"+z.ToString()+y.ToString()+x.ToString();
                    cuboIns.GetComponent<Cubo>().chunk = this;
                    cuboIns.GetComponent<Cubo>().posChunk = new Vector3(z,y,x);
                    chunk[z,y,x] = cuboIns;
                }
            }
        }
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    chunk[z,y,x].GetComponent<Cubo>().QuitarCaras(ObtenerVecinos(z,y,x));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
