using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected; 
    GameMaster gm; 
    public int tileSpeed;
    public bool hasMoved;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown(){
        if(selected == true) {
            gm.selectedUnit = null;
        } else {
            if(gm.selectedUnit != null) {
                gm.selectedUnit.selected = false;
            }
            selected = true;
            gm.selectedUnit = this;
            GetWalkableTiles();
        }
    }

    void GetWalkableTiles() {
        if(hasMoved == true) {
            return;
        }
        foreach (Tile tile in FindObjectsOfType<Tile>()) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            { // how far he can move
                if (tile.isClear() == true)
                { // is the tile clear from any obstacles
                    tile.Highlight();
                }

            }          
        }
    }



}
