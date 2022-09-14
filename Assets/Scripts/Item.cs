using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    public string nombre = "";
    public int cantidad = 0;
    public string tipo = "";
    public string infoAdicional = "";


    public bool Vacio(){
        return nombre=="" || cantidad==0 || tipo=="";
    }
}
