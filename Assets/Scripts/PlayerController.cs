using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public World World; // Referencia al mundo (hacerlo estático...?)
    public CharacterController controller;

    // Variables para el salto
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool enSuelo;

    // Variables de personaje
    public float speed = 12f;
    public float Gravedad = -19.62f;
    public bool gravedadON = true;
    public float jumpHeight = 3f;
    Vector3 velocity; // gravedad
    public int vida = 10;
    public int vidaMax = 10;

    private Inventario inventario = new Inventario();

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

        // Input para cambiar el seleccionado en el inventario
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
                            cuboVecino.MostrarCara(cuboAct); // Queremos mostrar una unica cara

                        }
                    }
                    Destroy(cubo);
                }
                else if(Input.GetButtonDown("Fire2") && inventario.ItemActual().tipo=="cubo"){
                    GameObject lado = hit.transform.gameObject;
                    cuboAct = lado.transform.parent.GetComponent<Cubo>();

                    // Posiciones del cubo que estamos mirando
                    Vector3 pos = cuboAct.transform.position; // en el mundo
                    Vector3 posChunk = cuboAct.posChunk; // en el chunk

                    // Nuevas posiciones para el cubo nuevo
                    Vector3 posNueva = pos;
                    Vector3 posChunkNueva = posChunk;

                    switch(lado.name){
                        case "Arriba":
                            posChunkNueva = new Vector3(0,1,0)+ posChunk;
                            posNueva.y++;
                            break;
                        case "Abajo":
                            posChunkNueva = new Vector3(0,-1,0)+ posChunk;
                            posNueva.y--;
                            break;
                        case "Izquierda":
                            posChunkNueva = new Vector3(0,0,-1)+ pos;
                            posNueva.z--;
                            break;
                        case "Derecha":
                            posChunkNueva = new Vector3(0,0,1)+ pos;
                            posNueva.z++;
                            break;
                        case "Delante":
                            posChunkNueva = new Vector3(1,0,0)+ pos;
                            posNueva.x++;
                            break;
                        case "Detras":
                            posChunkNueva = new Vector3(-1,0,0)+ pos;
                            posNueva.x--;
                            break;
                    }
                    
                    Chunk chunkNuevo = cuboAct.chunk;

                    // Comprobamos si hay que cambiar el chunk en el que se encuentra
                    // Solo hay que mirar la x o la z, ya que la y siempre se encuentra en el mismo chunk
                    // NOTA: he puesto 16 pero debería poner width/profundidad para ser consistente!!!
                    if(posChunkNueva.x>=16){
                        Vector2 vecino = new Vector2(chunkNuevo.PosWorld.x+1,chunkNuevo.PosWorld.y);
                        if(World.ActiveChunks.ContainsKey(vecino))
                            chunkNuevo = World.Chunks[vecino];
                        posChunkNueva.x -= 16;
                    }
                    else if(posChunkNueva.x<0){
                        Vector2 vecino = new Vector2(chunkNuevo.PosWorld.x-1,chunkNuevo.PosWorld.y);
                        if(World.ActiveChunks.ContainsKey(vecino))
                            chunkNuevo = World.Chunks[vecino];
                        posChunkNueva.x += 16;

                    }
                       
                    
                    if(posChunkNueva.z>=16){
                        Vector2 vecino = new Vector2(chunkNuevo.PosWorld.x,chunkNuevo.PosWorld.y+1);
                        if(World.ActiveChunks.ContainsKey(vecino))
                            chunkNuevo = World.Chunks[vecino];
                        posChunkNueva.z -= 16;
                    }
                    else if(posChunkNueva.z<0){
                        Vector2 vecino = new Vector2(chunkNuevo.PosWorld.x,chunkNuevo.PosWorld.y-1);
                        if(World.ActiveChunks.ContainsKey(vecino))
                            chunkNuevo = World.Chunks[vecino];
                        posChunkNueva.z += 16;
                    }

                    int y = (int)posChunkNueva.y;
                    int z = (int)posChunkNueva.z;
                    int x = (int)posChunkNueva.x;

                    Debug.Log(posChunkNueva);

                    // Si el cubo no se encuentra donde el jugador, entonces lo ponemos
                    if(!CompruebaPos(posNueva)){
                        GameObject cuboIns = Instantiate(chunkNuevo.cubo,posNueva, Quaternion.identity);
                        cuboIns.transform.parent = chunkNuevo.transform;
                        cuboIns.name = "Cubo"+x.ToString()+y.ToString()+z.ToString();
                        Cubo cuboNuevo = cuboIns.GetComponent<Cubo>();
                        cuboNuevo.chunk = chunkNuevo;
                        cuboNuevo.posChunk = new Vector3(x,y,z);
                        cuboNuevo.CambiaTipo(inventario.ItemActual().tipoCubo);
                        chunkNuevo.chunk[x,y,z] = cuboIns;
                        
                        // Escondemos las caras no visibles
                        List<GameObject> vecinos = cuboAct.chunk.ObtenerVecinos(x,y,z);
                        cuboNuevo.StartQuitarCaras(vecinos);
                        foreach(var vecino in vecinos){
                            if(vecino!=null){
                                Cubo cuboVecino = vecino.GetComponent<Cubo>();
                                cuboVecino.QuitarCara(cuboNuevo); // Solo queremos quitar una cara
                            }
                        }
                        cuboAct.QuitarCaras(lado.name);

                        Debug.Log("Cubo colocado en "+posChunkNueva+" en el chunk "+chunkNuevo.PosWorld);

                        inventario.Usar();
                    }
                }
            }
        }
    }
}
