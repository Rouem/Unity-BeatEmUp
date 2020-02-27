using UnityEngine.UI;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    //Classe responsável pela exibição da vida do player na tela.
    public Text hpUi;//texto da vida
    public Image hpBar;//barra de vida

    public GameObject SideBar, Reload;

    Attributes attributes;//atributos do player
    void Start()
    {
        attributes = GetComponent<Attributes>();
    }

    public void ReloadUI()
    {
        float value = CalculateBar(attributes.hp,attributes.maxHp);
        hpUi.text = "Vida ("+(value*100).ToString("F0")+"%)";
        hpBar.rectTransform.localScale = new Vector3(value,hpBar.rectTransform.localScale.y,0);
    }

    float CalculateBar(int hp, int hpMax){
       return (float)(hp*100/hpMax)/100;
    }
}
