using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario
{
    const int MAX = 64;
    public class Item{
        public string nombre = "";
        public int cantidad = 0;
        public string tipo = "";
        public string tipoCubo = "";

        public bool Vacio(){
            return nombre=="";
        }
    }
    private List<Item> Objetos;
    private int seleccionado = 0;
    public Inventario() {
        Objetos = new List<Item>(7);
    }
    public void DebugInventario(){
        Item cobble = new Item();
        cobble.nombre = "Cobblestone";
        cobble.cantidad = 1;
        cobble.tipo = "cubo";
        cobble.tipoCubo = "cobble";
        Objetos.Add(cobble);
        Item tierra = new Item();
        tierra.nombre = "Tierra";
        tierra.cantidad = MAX;
        tierra.tipo = "cubo";
        tierra.tipoCubo = "tierra";
        Objetos.Add(tierra);
    }

    public Item ItemActual(){
        return Objetos[seleccionado];
    }

    public void CambiarSeleccionado(int n){
        if(n>=0 && n<Objetos.Count)
            seleccionado = n;
    }

    public void Usar(){
        if(!Objetos[seleccionado].Vacio()){
            Objetos[seleccionado].cantidad--;
            if(Objetos[seleccionado].cantidad==0){
                Objetos[seleccionado] = new Item();
            }

        }
    }
}
