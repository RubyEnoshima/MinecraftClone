using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public GameObject cubo;

    public int width = 5;
    public int height = 3;
    public int profundidad = 1;

    GameObject[,,] chunk;
    // List<List<List<GameObject>>> chunk;

    // Devuelve una lista de vecinos de la coordenada dada.
    // La lista no puede obtener huecos, y siempre tiene el mismo orden: arriba, abajo, izq, der, del, detr
    List<GameObject> ObtenerVecinos(int z, int y, int x){
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


    // Start is called before the first frame update
    void Start()
    {
        chunk = new GameObject[profundidad, height, width];
        for(int z=0;z<profundidad;z++){
            for(int y=0;y<height;y++){
                for(int x=0;x<width;x++){
                    GameObject cuboIns = Instantiate(cubo,new Vector3(x, z, y), Quaternion.identity);
                    cuboIns.transform.parent = this.transform;
                    cuboIns.name = "Cubo"+z.ToString()+y.ToString()+x.ToString();
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