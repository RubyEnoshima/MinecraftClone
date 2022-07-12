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

    void Start() {
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
    bool CompruebaPos(Vector3 posCubo){
        return !((posCubo.x - 0.5 >= transform.position.x || posCubo.x + 0.5 <= transform.position.x) &&
         //(posCubo.y - 0.5 >= transform.position.y || posCubo.y + 0.5 <= transform.position.y + 2) &&
         (posCubo.z - 0.5 >= transform.position.z || posCubo.z + 0.5 <= transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        Moverse();

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 7f))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);
            if(Input.GetButtonDown("Fire1")){
                GameObject cubo = hit.transform.parent.gameObject;
                Cubo cuboAct = cubo.GetComponent<Cubo>();
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
            else if(Input.GetButtonDown("Fire2")){
                GameObject lado = hit.transform.gameObject;
                Cubo cuboAct = lado.transform.parent.GetComponent<Cubo>();
                Vector3 pos = cuboAct.posChunk;
                Debug.Log(lado);
                Vector3 posNueva;
                Cubo cuboNuevo;
                switch(lado.name){
                    case "Arriba":
                        posNueva = new Vector3(1,0,0)+ pos;
                        break;
                    case "Abajo":
                        posNueva = new Vector3(-1,0,0)+ pos;
                        break;
                    case "Izquierda":
                        posNueva = new Vector3(0,-1,0)+ pos;
                        break;
                    case "Derecha":
                        posNueva = new Vector3(0,1,0)+ pos;
                        break;
                    case "Delante":
                        posNueva = new Vector3(0,0,1)+ pos;
                        break;
                    case "Detras":
                        posNueva = new Vector3(0,0,-1)+ pos;
                        break;
                    default:
                        posNueva = new Vector3(-1,-1,-1) + pos;
                        break;
                }
                if(!CompruebaPos(posNueva)){
                    Chunk chunkNuevo = cuboAct.chunk;
                    int z = (int)posNueva.x;
                    int y = (int)posNueva.y;
                    int x = (int)posNueva.z;
                    GameObject cuboIns = Instantiate(chunkNuevo.cubo,new Vector3(x, z, y), Quaternion.identity);
                    cuboIns.transform.parent = chunkNuevo.transform;
                    cuboIns.name = "Cubo"+z.ToString()+y.ToString()+x.ToString();
                    cuboIns.GetComponent<Cubo>().chunk = chunkNuevo;
                    cuboIns.GetComponent<Cubo>().posChunk = posNueva;
                    chunkNuevo.chunk[z,y,x] = cuboIns;

                }
            }
        }
    }
}
