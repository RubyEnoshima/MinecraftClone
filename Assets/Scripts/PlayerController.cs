using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool enSuelo;

    public float speed = 12f;
    public float Gravedad = -19.62f;
    public bool gravedadON = true;
    public float jumpHeight = 3f;

    Vector3 velocity; // gravedad

    private Inventario inventario = new Inventario();
    public int vida = 10;
    public int vidaMax = 10;

    Cubo ultimoCubo = null;

    void Start() {
        inventario.DebugInventario();
        vida = vidaMax;
        if(!gravedadON) Gravedad = 0;
    }

    void Moverse(){
        enSuelo = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(enSuelo && velocity.y<0){
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && enSuelo){
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * Gravedad);
        }

        velocity.y += Gravedad * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // True si está en el mismo sitio que el cubo
    // HAY QUE COMPROBAR MOBS!!!
    bool CompruebaPos(Vector3 posCubo){
        Vector3 pos = posCubo - transform.position;
        bool res = pos.x >= -0.5f && pos.x <= 0.5f &&
        pos.y >= -1.5f && pos.y <= 0.1f &&
        pos.z >= -0.5f && pos.z <= 0.5f;
        return res;
    }

    // Update is called once per frame
    void Update()
    {
        Moverse();

        RaycastHit hit;

        bool numPresionado = false;
        for(int i=1;i<=7;i++){
            if(Input.GetKeyDown(i.ToString())){
                inventario.CambiarSeleccionado(i-1);
                Debug.Log("Cambiado");
                numPresionado = true;
                break;
            }
        }

        if (!numPresionado && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 7f))
        {
            if(hit.transform==null){
                ultimoCubo.NoResaltar();
                ultimoCubo = null;
            }
            else if(hit.transform.gameObject.name!="Player"){

                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);
                
                GameObject cubo = hit.transform.parent.gameObject;
                Cubo cuboAct = cubo.GetComponent<Cubo>();


                if(ultimoCubo==null || cuboAct!=ultimoCubo){
                    cuboAct.Resaltar();
                    if(ultimoCubo!=null) ultimoCubo.NoResaltar();
                    ultimoCubo = cuboAct;

                }
                
                if(Input.GetButtonDown("Fire1")){
                    Vector3 pos = cuboAct.posChunk;
                    List<GameObject> vecinos = cuboAct.chunk.ObtenerVecinos(pos);
                    cuboAct.chunk.chunk[(int)pos.x,(int)pos.y,(int)pos.z] = null;
                    foreach(var vecino in vecinos){
                        if(vecino!=null){
                            Cubo cuboVecino = vecino.GetComponent<Cubo>();
                            cuboVecino.QuitarCaras(cuboVecino.chunk.ObtenerVecinos(cuboVecino.posChunk));

                        }
                    }
                    Destroy(cubo);
                }
                else if(Input.GetButtonDown("Fire2") && inventario.ItemActual().tipo=="cubo"){
                    GameObject lado = hit.transform.gameObject;
                    cuboAct = lado.transform.parent.GetComponent<Cubo>();
                    Vector3 pos = cuboAct.transform.position;
                    Vector3 posChunk = cuboAct.posChunk;
                    Vector3 posNueva = new Vector3(Mathf.RoundToInt(hit.point.x),hit.point.y,Mathf.RoundToInt(hit.point.z));
                    Vector3 posChunkNueva;
                    Debug.Log(posNueva+", el cubo clicado está en "+posChunk);

                    // bool comprobar = true;

                    switch(lado.name){
                        case "Arriba":
                            posChunkNueva = new Vector3(0,1,0)+ posChunk;
                            // comprobar = false;
                            break;
                        case "Abajo":
                            posChunkNueva = new Vector3(0,-1,0)+ posChunk;
                            // comprobar = false;
                            break;
                        // case "Izquierda":
                        //     posNueva = new Vector3(0,-1,0)+ pos;
                        //     break;
                        // case "Derecha":
                        //     posNueva = new Vector3(0,1,0)+ pos;
                        //     break;
                        // case "Delante":
                        //     posNueva = new Vector3(0,0,1)+ pos;
                        //     break;
                        // case "Detras":
                        //     posNueva = new Vector3(0,0,-1)+ pos;
                        //     break;
                        default:
                            posChunkNueva = new Vector3(-1,-1,-1);
                            break;
                    }

                    // posNueva.x %= 16;
                    // posNueva.z %= 16;

                    Chunk chunkNuevo = cuboAct.chunk;
                    // Si no hay que comprobar nada (arriba/abajo) o la posicion esta guay
                    // if(cuboAct.chunk.PosValida(posNueva)){
                    //     chunkNuevo = cuboAct.chunk;
                    // }else{
                    //     chunkNuevo = null; // el nuevo chunk, hay que calcularlo somehow
                    //     posNueva = posNueva; // nueva posicion dentro del nuevo chunk
                    //     throw new System.Exception();
                    // }

                    int z = (int)posChunkNueva.x;
                    int y = (int)posChunkNueva.y;
                    int x = (int)posChunkNueva.z;

                    // Si el cubo no se encuentra donde el jugador, entonces lo ponemos
                    if(!CompruebaPos(posNueva)){
                        GameObject cuboIns = Instantiate(chunkNuevo.cubo,posNueva, Quaternion.identity);
                        cuboIns.transform.parent = chunkNuevo.transform;
                        cuboIns.name = "Cubo"+z.ToString()+y.ToString()+x.ToString();
                        Cubo cuboNuevo = cuboIns.GetComponent<Cubo>();
                        cuboNuevo.chunk = chunkNuevo;
                        cuboNuevo.posChunk = new Vector3(z,y,x);
                        cuboNuevo.CambiaTipo(inventario.ItemActual().tipoCubo);
                        chunkNuevo.chunk[z,y,x] = cuboIns;
                        
                        // Escondemos las caras no visibles
                        List<GameObject> vecinos = cuboAct.chunk.ObtenerVecinos(z,y,x);
                        cuboNuevo.QuitarCaras(vecinos);
                        foreach(var vecino in vecinos){
                            if(vecino!=null){
                                Cubo cuboVecino = vecino.GetComponent<Cubo>();
                                cuboVecino.QuitarCaras(cuboVecino.chunk.ObtenerVecinos(cuboVecino.posChunk));
                            }
                        }
                        cuboAct.QuitarCaras(lado.name);

                        inventario.Usar();
                    }
                }
            }
        }
    }
}
