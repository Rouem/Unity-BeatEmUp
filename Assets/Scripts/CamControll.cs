using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControll : MonoBehaviour
{

    public float distance, angle;// distancia e angulo da camera.
    public Transform player;//alvo para onde a camera vai olhar.
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //o código abaixo faz a camera seguir o alvo(player), ajustando angulo e posição
        //ajustantando a rotação da camera
        transform.rotation = Quaternion.Euler(angle,transform.eulerAngles.y,transform.eulerAngles.z);
        //ajustando a posição
        transform.position = player.position - transform.forward*distance;
    }
}
