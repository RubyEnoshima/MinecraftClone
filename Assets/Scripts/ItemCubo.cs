using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCubo : Item
{

    public ItemCubo(string _tipo){
        tipo = "cubo";
        infoAdicional = _tipo;
    }
}
