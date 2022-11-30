using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public GameObject victoryPanel;
    private AudioSource source;
    public AudioClip seletedSound;
    public AudioClip attackSound;
    public AudioClip deathSound;
    public string name;
    public bool isKing;

    public bool selected;

    GameMaster gm;

    public int tileSpeed;

    public bool hasMoved;

    public float moveSpeed;

    public int cunning;

    public int playerSide;

    public int attackRange;

    public int health;

    public int attackDamage;

    public int defenseDamage;

    public int armor;

    List<Unit> enemiesInRange = new List<Unit>();

    public bool hasAttacked;

    public GameObject attackSquare;

    private Animator camAnim;

    private Animator AttackAnim;
    public SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        ResetAttackSquares();
        camAnim = Camera.main.GetComponent<Animator>();
        AttackAnim = this.GetComponent<Animator>();
        rend = this.GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if(unit.playerSide != gm.playerTurn) {
                  unit.GetComponent<SpriteRenderer>().color = new Color(217, 217, 1f);
            }  else {
                unit.GetComponent<SpriteRenderer>().color = new Color(255, 255, 1f);
            }
        }
    }

    private void OnMouseEnter()
    {
        gm.hoverEnemyUnit = this;
    }

    private void OnMouseExit()
    {
        gm.hoverEnemyUnit = null;
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
                source.clip = seletedSound;
                source.Play();
                selected = true;
                gm.selectedUnit = this;
                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }
        }

        Collider2D col =
            Physics2D
                .OverlapCircle(Camera
                    .main
                    .ScreenToWorldPoint(Input.mousePosition),
                0.15f);
        Unit unit = col.GetComponent<Unit>();
        if (gm.selectedUnit != null)
        {
            if (
                gm.selectedUnit.enemiesInRange.Contains(unit) &&
                gm.selectedUnit.hasAttacked == false
            )
            {
                gm.selectedUnit.Attack (unit);
            }
        }
    }

    void Attack(Unit enemy)
    {
        AttackAnim.SetTrigger("attack");
        source.clip = attackSound;
        source.Play();
        camAnim.SetTrigger("New Trigger");
        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.defenseDamage - armor;

        if (enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
        }

        if (myDamage >= 1)
        {
            health -= myDamage;
        }

        if (enemy.health <= 0)
        {
            source.clip = deathSound;
            source.Play();
            if(enemy.isKing == true) {
                //game over
                victoryPanel.SetActive(true);
                if(enemy.playerSide == 1) {
                    gm.winner.text = "ACHILLES";
                    gm.looser.text = "HECTOR";
                } else {
                    gm.winner.text = "HECTOR";
                    gm.looser.text = "ACHILLES";
                }
            }
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if (health <= 0)
        {
            gm.ResetTiles();
            Destroy(this.gameObject);
        }
    }

    void GetWalkableTiles()
    {
        // Looks for the tiles the unit can walk on
        if (hasMoved == true)
        {
            return;
        }

        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in tiles)
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

    public void Move(Transform movePos)
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(movePos));
    }

    IEnumerator StartMovement(Transform movePos)
    {
        // Moves the character to his new position.
        while (transform.position.x != movePos.position.x)
        {
            // first aligns him with the new tile's x pos
            transform.position =
                Vector2
                    .MoveTowards(transform.position,
                    new Vector2(movePos.position.x, transform.position.y),
                    moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (transform.position.y != movePos.position.y // then y
        )
        {
            transform.position =
                Vector2
                    .MoveTowards(transform.position,
                    new Vector2(transform.position.x, movePos.position.y),
                    moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetAttackSquares();
        GetEnemies();
    }

    // IEnumerator StartMovement(Vector2 tilePos)
    // {
    //     while (transform.position.x != tilePos.x)
    //     {
    //         transform.position =
    //             Vector2
    //                 .MoveTowards(transform.position,
    //                 new Vector2(tilePos.x, transform.position.y),
    //                 moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    //     while (transform.position.y  < tilePos.y)
    //     {
    //         transform.position =
    //             Vector2
    //                 .MoveTowards(transform.position,
    //                 new Vector2(transform.position.x, tilePos.y ),
    //                 moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    //     hasMoved = true;
    //     ResetAttackSquares();
    //     GetEnemies();
    // }
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

    public void ResetAttackSquares()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.attackSquare.SetActive(false);
        }
    }
}
