using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected;

    GameMaster gm;

    public int tileSpeed;

    public bool hasMoved;

    public float moveSpeed;

    public int playerSide;

    public int attackRange;

    List<Unit> enemiesInRange = new List<Unit>();

    public bool hasAttacked;
    public GameObject attackSquare; 

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnMouseDown()
    {
        ResetAttackSquares();
        if (selected == true)
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else
        {
            if (playerSide == gm.playerTurn)
            {
                if (gm.selectedUnit != null)
                {
                    gm.selectedUnit.selected = false;
                }
                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
        }
    }

    void GetWalkableTiles()
    {
        if (hasMoved == true)
        {
            return;
        }
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (
                Mathf.Abs(transform.position.x - tile.transform.position.x) +
                Mathf.Abs(transform.position.y - tile.transform.position.y) <=
                tileSpeed
            )
            {
                // how far he can move
                if (tile.isClear() == true)
                {
                    // is the tile clear from any obstacles
                    tile.Highlight();
                }
            }
        }
    }

    public void Move(Vector2 tilePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {
            transform.position =
                Vector2
                    .MoveTowards(transform.position,
                    new Vector2(tilePos.x, transform.position.y),
                    moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != tilePos.y)
        {
            transform.position =
                Vector2
                    .MoveTowards(transform.position,
                    new Vector2(transform.position.x, tilePos.y),
                    moveSpeed * Time.deltaTime);
            yield return null;
        }
        hasMoved = true;
        ResetAttackSquares();
        GetEnemies();
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (
                Mathf.Abs(transform.position.x - unit.transform.position.x) +
                Mathf.Abs(transform.position.y - unit.transform.position.y) <=
                attackRange
            )
            {
                if (unit.playerSide != gm.playerTurn && hasAttacked == false)
                {
                    enemiesInRange.Add (unit);
                    unit.attackSquare.SetActive(true);
                }
            }
        }
    }

    public void ResetAttackSquares() {
        foreach(Unit unit in FindObjectsOfType<Unit>()) {
            unit.attackSquare.SetActive(false);
        }
    }
}
