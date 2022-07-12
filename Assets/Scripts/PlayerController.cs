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
    public float Gravedad = -9.81f;
    public float jumpHeight = 3f;

    Vector3 velocity; // gravedad

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
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Gravedad);
        }

        velocity.y += Gravedad * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        Moverse();

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f))
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.yellow);
            if(Input.GetButtonDown("Fire1")){
                GameObject cubo = hit.transform.gameObject;
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
        }
    }
}
