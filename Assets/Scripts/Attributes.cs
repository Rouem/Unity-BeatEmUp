using UnityEngine;

public class Attributes : MonoBehaviour
{

    /* Aqui estão alguns atributos básicos que todo personagem precisa ter, como hp(vida), 
    hp máximo(vida inicial e total), ataque, velocidade de movimento... 

    hitCounts é a quantidade de golpes que o persoangem levou e CountOfHits
    representa quantos golpes ele precisa levar para ficar atordoado/zonzo/tonto.

    invecibleOnStun já diz, o personagem fica invencível enquanto esta tonto. 
    */
    public int hp, maxHp = 1, atk = 1, spd = 1, hitsCount,CountOfHitsToStun = 0;
    public bool invencibleOnStun = false;
    void Start()
    {
        hp = maxHp;
    }
    /// <summary>
    /// Esta função verifica se personagem levou uma quantida de golpes suficientes para ficar tonto.
    /// </summary>
    /// <param>Retorna um valor booleano</param>
    public bool IsStunned(){
        return hitsCount >= CountOfHitsToStun;
    }
    
}
