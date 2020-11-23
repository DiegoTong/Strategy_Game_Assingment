using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Button spawnbutton;
    public Button actionButton;
    public Button skipButton;
    public Button attacButton;
    public Text actionButtonText;
    public Text spawnbuttonText;
    public Slider healthBar;
    public Text unitName;
    public Button attacButtonTop;
    public Button attacButtonBottom;
    public Button attacButtonLeft;
    public Button attacButtonRight;
    // Start is called before the first frame update
    void Start()
    {
        spawnbutton.gameObject.SetActive(false);
        attacButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        attacButtonTop.gameObject.SetActive(false);
        attacButtonBottom.gameObject.SetActive(false);
        attacButtonLeft.gameObject.SetActive(false);
        attacButtonRight.gameObject.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setStats()
    {
        healthBar.GetComponent<Slider>().value = 100;
        unitName.text = "";
    }
    public void setStats(UnitController unit)
    {
        healthBar.GetComponent<Slider>().value = unit.health;
        unitName.text = unit.name;
    }
}
