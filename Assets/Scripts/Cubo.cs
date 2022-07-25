using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    public string tipo = "cobble";
    public Chunk chunk;
    public Vector3 posChunk;
    static Material resalte;

    bool resaltado = false;

    void Awake() {
        resalte = Resources.Load<Material>("destacar");
    }

    // Start is called before the first frame update
    void Start()
    {
        // NoResaltar();
        if(tipo == "aire"){
            foreach(Transform cara in transform){
                cara.gameObject.SetActive(false);
            }
        }
        else{
            CambiaVisual();
        }
    }

    public void Resaltar(){
        if(!resaltado){
            foreach(Transform cara in transform){
                Material[] matArray = cara.gameObject.GetComponent<Renderer>().materials;
                matArray[1] = resalte;
                cara.gameObject.GetComponent<Renderer>().materials = matArray;
            }
            resaltado = true;
        }
    }

    public void NoResaltar(){
        if(resaltado){
            resaltado = false;
            foreach(Transform cara in transform){
                Material[] matArray = cara.gameObject.GetComponent<Renderer>().materials;
                matArray[1] = null;
                cara.gameObject.GetComponent<Renderer>().materials = matArray;
            }
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

    // Detras x-, Abajo y-, Izq z-

    public void QuitarLado(int i){
        transform.GetChild(i).gameObject.SetActive(false);
        transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = false;
    }

    public void MostrarLado(int i){
        transform.GetChild(i).gameObject.SetActive(true);
        transform.GetChild(i).gameObject.GetComponent<MeshCollider>().enabled = true;
    }
    
    // La lista de cubos no deberia contener ningun hueco
    public void QuitarCaras(List<GameObject> cubos){
        for(int i=0;i<cubos.Count;i++){
            if(cubos[i]!=null && cubos[i].GetComponent<Cubo>().tipo != "aire"){
                QuitarLado(i);
            }
            else{
                MostrarLado(i);
            }
        }
    }

    // Usamos estas funciones para mostrar o quitar una unica cara dependiendo de la posicion del cubo
    public void QuitarCara(Cubo cubo){
        if(cubo.transform.position.x < transform.position.x){
            QuitarLado(0);
        }else if(cubo.transform.position.y < transform.position.y){
            QuitarLado(1);
        }else if(cubo.transform.position.z < transform.position.z){
            QuitarLado(2);
        }else if(cubo.transform.position.x > transform.position.x){
            QuitarLado(3);
        }else if(cubo.transform.position.y > transform.position.y){
            QuitarLado(4);
        }else if(cubo.transform.position.z > transform.position.z){
            QuitarLado(5);
        }
    }

    public void MostrarCara(Cubo cubo){
        if(cubo.transform.position.x < transform.position.x){
            MostrarLado(0);
        }else if(cubo.transform.position.y < transform.position.y){
            MostrarLado(1);
        }else if(cubo.transform.position.z < transform.position.z){
            MostrarLado(2);
        }else if(cubo.transform.position.x > transform.position.x){
            MostrarLado(3);
        }else if(cubo.transform.position.y > transform.position.y){
            MostrarLado(4);
        }else if(cubo.transform.position.z > transform.position.z){
            MostrarLado(5);
        }
    }

    // Para cuando queremos quitar una cara sabiendo el nombre de la cara que hay que quitar
    public void QuitarCaras(string cara){
        GameObject caraQuitar = transform.Find(cara).gameObject;
        caraQuitar.SetActive(false);
        caraQuitar.GetComponent<MeshCollider>().enabled = false;

    }
}
