using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    public string tipo = "cobble";
    public Chunk chunk;
    public Vector3 posChunk;
    public bool boxHit = false;

    // Start is called before the first frame update
    void Start()
    {
        if(tipo == "aire"){
            foreach(Transform cara in transform){
                cara.gameObject.SetActive(false);
            }
        }
        else{
            CambiaVisual();
        }
    }

    public void CambiaTipo(string _tipo){
        if(tipo!=_tipo){
            tipo = _tipo;
            CambiaVisual();

        }
    }

    // Cambia cómo se ve el bloque según el tipo asignado
    void CambiaVisual(){
        Material bloque = Resources.Load<Material>(tipo);
        foreach(Transform cara in transform){
            cara.gameObject.GetComponent<Renderer>().material = bloque;
        }
    }
    
    // La lista de cubos no deberia contener ningun hueco
    public void QuitarCaras(List<GameObject> cubos){ // arriba, abajo, izq, der, del, detr
        int n = 0;
        for(int i=0;i<cubos.Count;i++){
            if(cubos[i]!=null && cubos[i].GetComponent<Cubo>().tipo != "aire"){
                transform.GetChild(i).gameObject.SetActive(false);
                transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = false;
                n++;
            }
            else{
                transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = true;
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void QuitarCaras(string cara){
        GameObject caraQuitar = transform.Find(cara).gameObject;
        caraQuitar.SetActive(false);
        caraQuitar.GetComponent<MeshCollider>().enabled = false;

    }
}
