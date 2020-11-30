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
    public Text unitHealth;
    public Text unitSpeed;
    public Text unitAttack;
    public Text turnCount;
    public Text numGold;
    public Text numResources;
    public Text unitCap;
    public Button BuildHQButton;
    public Button BuildTurretButton;
    public Button BuildWallButton;
    public Button SpawnSoldierButton;
    public Button SpawnUnit;
    // Start is called before the first frame update
    void Start()
    {
        spawnbutton.gameObject.SetActive(false);
        attacButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        BuildHQButton.gameObject.SetActive(false);
        BuildTurretButton.gameObject.SetActive(false);
        BuildWallButton.gameObject.SetActive(false);
        SpawnSoldierButton.gameObject.SetActive(false);
        SpawnUnit.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
     public void updateUI(int numresources, int numturns, int numgold, int numUnits, int numCap)
    {
        turnCount.text = "Turn " + numturns;
        numGold.text = "Alien Spawns: " + numgold;
        numResources.text = "Resources: " + numresources;
        unitCap.text = "Units " + numUnits + "/" + numCap;
    }
    public void setStats()
    {
        healthBar.GetComponent<Slider>().maxValue = 100;
        healthBar.GetComponent<Slider>().value = 100;
        unitName.text = "";
        unitHealth.text = "";
        unitSpeed.text = "";
        unitAttack.text = "";
    }
    public void setStats(UnitController unit)
    {
        healthBar.GetComponent<Slider>().maxValue = unit.maxHealth;
        healthBar.GetComponent<Slider>().value = unit.health;
        unitName.text = unit.name;
        unitHealth.text = unit.health + " / "+ unit.maxHealth;
        unitSpeed.text = "Unit speed " +unit.movement;
        unitAttack.text = "Unit Attack " + unit.attack;
    }
    public void EnableSpawnButtons()
    {
        SpawnSoldierButton.gameObject.SetActive(true);
        SpawnUnit.gameObject.SetActive(true);
    }
    public void enableBuildButtons()
    {
        BuildHQButton.gameObject.SetActive(true);
        BuildTurretButton.gameObject.SetActive(true);
        BuildWallButton.gameObject.SetActive(true);
    }
    public void disableUIBUttons()
    {
        SpawnSoldierButton.gameObject.SetActive(false);
        SpawnUnit.gameObject.SetActive(false);
        BuildHQButton.gameObject.SetActive(false);
        BuildTurretButton.gameObject.SetActive(false);
        BuildWallButton.gameObject.SetActive(false);
    }
    
}
