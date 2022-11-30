using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit;
    public Unit hoverEnemyUnit;

    public int playerTurn = 1;
    public int turnnumber = 1;
    public GameObject selectedUnitSquare;
    public TextMeshProUGUI PlayerHealth;
    public TextMeshProUGUI PlayerSpeed;
    public TextMeshProUGUI PlayerCunning;
    public TextMeshProUGUI PlayerStrength;
    public TextMeshProUGUI PlayerGuard;
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI Turn;

    public TextMeshProUGUI HoverEnemyPlayerHealth;
    public TextMeshProUGUI HoverEnemyPlayerSpeed;
    public TextMeshProUGUI HoverEnemyPlayerCunning;
    public TextMeshProUGUI HoverEnemyPlayerStrength;
    public TextMeshProUGUI HoverEnemyPlayerGuard;
    public TextMeshProUGUI HoverEnemyPlayerName;

    public TextMeshProUGUI winner;
    public TextMeshProUGUI looser;



    // Start is called before the first frame update
    void Start()
    {
            PlayerHealth.text = "";
            PlayerSpeed.text = "";
            PlayerCunning.text = "";
            PlayerStrength.text = "";
            PlayerGuard.text = "";
            PlayerName.text = "";
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(playerTurn == 2 ) {
                turnnumber += 1;
                Turn.text = turnnumber.ToString();
            }
            EndTurn();
        }

        if(hoverEnemyUnit != null && hoverEnemyUnit.playerSide == 2) {
            HoverEnemyPlayerHealth.text = hoverEnemyUnit.health.ToString();
            HoverEnemyPlayerSpeed.text = hoverEnemyUnit.moveSpeed.ToString();
            HoverEnemyPlayerCunning.text = hoverEnemyUnit.cunning.ToString();
            HoverEnemyPlayerStrength.text = hoverEnemyUnit.attackDamage.ToString();
            HoverEnemyPlayerGuard.text = hoverEnemyUnit.armor.ToString();
            HoverEnemyPlayerName.text = hoverEnemyUnit.name.ToString();
        }
        
        if(hoverEnemyUnit != null && hoverEnemyUnit.playerSide == 1) {
            PlayerHealth.text = hoverEnemyUnit.health.ToString();
            PlayerSpeed.text = hoverEnemyUnit.moveSpeed.ToString();
            PlayerCunning.text = hoverEnemyUnit.cunning.ToString();
            PlayerStrength.text = hoverEnemyUnit.attackDamage.ToString();
            PlayerGuard.text = hoverEnemyUnit.armor.ToString();
            PlayerName.text = hoverEnemyUnit.name.ToString();
        }

        if(hoverEnemyUnit == null ) {
            HoverEnemyPlayerHealth.text = "";
            HoverEnemyPlayerSpeed.text =  "";
            HoverEnemyPlayerCunning.text =  "";
            HoverEnemyPlayerStrength.text =  "";
            HoverEnemyPlayerGuard.text =  "";
            HoverEnemyPlayerName.text =  "Hover over Enemy";

            PlayerHealth.text = "";
            PlayerSpeed.text = "";
            PlayerCunning.text = "";
            PlayerStrength.text = "";
            PlayerGuard.text = "";
            PlayerName.text = "Hover over Fighter";
        }

        if(selectedUnit != null) {
            PlayerHealth.text = selectedUnit.health.ToString();
            PlayerSpeed.text = selectedUnit.moveSpeed.ToString();
            PlayerCunning.text = selectedUnit.cunning.ToString();
            PlayerStrength.text = selectedUnit.attackDamage.ToString();
            PlayerGuard.text = selectedUnit.armor.ToString();
            PlayerName.text = selectedUnit.name.ToString();
           
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = selectedUnit.transform.position;
        } else {
            selectedUnitSquare.SetActive(false);
        }
    }

    void EndTurn()
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }
        else if (playerTurn == 2)
        {
            playerTurn = 1;
        }

        if (selectedUnit != null)
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.attackSquare.SetActive(false);
            unit.hasAttacked = false;
        }
    }


}
