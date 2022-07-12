using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubo : MonoBehaviour
{
    public string tipo = "tierra";
    // Start is called before the first frame update
    void Start()
    {
        if(tipo == "aire"){
            foreach(Transform cara in transform){
                cara.gameObject.SetActive(false);
            }
        }
    }

    // La lista de cubos no deberia contener ningun hueco
    public void QuitarCaras(List<GameObject> cubos){ // arriba, abajo, izq, der, del, detr
        int n = 0;
        for(int i=0;i<cubos.Count;i++){
            if(cubos[i]!=null && cubos[i].GetComponent<Cubo>().tipo != "aire"){
                transform.GetChild(i).gameObject.SetActive(false);
                n++;
            }
        }
        if(n==cubos.Count) GetComponent<BoxCollider>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
