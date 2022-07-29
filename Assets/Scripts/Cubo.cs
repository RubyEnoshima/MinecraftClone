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

    int nVisibles;
    void Awake() {
        resalte = Resources.Load<Material>("destacar");
        nVisibles = transform.childCount;
    }

    // Start is called before the first frame update
    void Start()
    {
        // NoResaltar();
        if(tipo == "aire"){
            Deshabilitar();
        }
        else{
            CambiaVisual();
        }
    }

    public void Deshabilitar(){
        for(int i=0;i<transform.childCount;i++){
            QuitarLado(i);
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
            if(_tipo=="aire") Deshabilitar();
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
        GameObject child = transform.GetChild(i).gameObject;
        if(nVisibles>0 && child.activeSelf){
            child.SetActive(false);
            child.GetComponent<MeshCollider>().enabled = false;
            nVisibles--;
            if(nVisibles==0) this.gameObject.SetActive(false);
        }
    }

    public void MostrarLado(int i){
        GameObject child = transform.GetChild(i).gameObject;
        if(!child.activeSelf){
            child.SetActive(true);
            child.GetComponent<MeshCollider>().enabled = true;
            nVisibles++;
            if(nVisibles>0) this.gameObject.SetActive(true);

        }
    }

    public void StartQuitarCaras(List<GameObject> cubos){
        for(int i=0;i<cubos.Count;i++){
            if(cubos[i]!=null && cubos[i].GetComponent<Cubo>().tipo != "aire"){
                // QuitarCara(cubos[i]);
                QuitarLado(i);
            }
        }
    }
    
    // La lista de cubos no deberia contener ningun hueco
    public void QuitarCaras(List<GameObject> cubos){
        for(int i=0;i<cubos.Count;i++){
            if(cubos[i]==null) QuitarLado(i);
            else if(cubos[i].GetComponent<Cubo>().tipo != "aire"){
                // QuitarLado(i);
                QuitarCara(cubos[i]);
            }
            else{
                // MostrarLado(i);
                MostrarCara(cubos[i]);
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

    public void QuitarCara(GameObject cubo){
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

    public void MostrarCara(GameObject cubo){
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
        if(caraQuitar.activeSelf){
            caraQuitar.SetActive(false);
            caraQuitar.GetComponent<MeshCollider>().enabled = false;
            nVisibles--;
            if(nVisibles==0) this.gameObject.SetActive(false);
        }
    }
}
