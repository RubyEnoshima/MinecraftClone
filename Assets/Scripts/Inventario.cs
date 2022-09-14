using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario
{
    const int MAX = 64;
    private List<Item> Objetos;
    private int seleccionado = 0;

    public Inventario() {
        Objetos = new List<Item>(7);
    }
    
    public void DebugInventario(){
        ItemCubo cobble = new ItemCubo("cobble");
        cobble.nombre = "Cobblestone";
        cobble.cantidad = MAX;
        Objetos.Add(cobble);
        
        ItemCubo tierra = new ItemCubo("tierra");
        tierra.nombre = "Tierra";
        tierra.cantidad = MAX;
        Objetos.Add(tierra);
        Debug.Log(Objetos.Count);
    }

    public Item ItemActual(){
        return Objetos[seleccionado];
    }

    public bool CambiarSeleccionado(int n){
        if(n>=0 && n<Objetos.Count){
            seleccionado = n;
            return true;
        }
        return false;
    }

    public void Usar(){
        if(!Objetos[seleccionado].Vacio()){
            Objetos[seleccionado].cantidad--;
            if(Objetos[seleccionado].cantidad==0){
                Objetos[seleccionado] = new Item();
            }

        }
    }

    public void AddItem(Item item){
        
    }
}
