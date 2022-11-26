using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;

    public Sprite HigthlitedTile;

    public Sprite DefaultTile;

    public Color highlitedColor;

    public bool isWalkable;

    public LayerMask obstacles;

    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseEnter()
    {
        rend = GetComponent<SpriteRenderer>();
        // rend.sprite = HigthlitedTile;
    }

    private void OnMouseExit()
    {
        rend = GetComponent<SpriteRenderer>();
        // rend.sprite = DefaultTile;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool
    isClear() // does this tile have an obstacle on it. Yes or No?
    {
        Collider2D col =
            Physics2D.OverlapCircle(transform.position, 0.2f, obstacles);
        if (col == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Highlight()
    {
        // rend.color = highlitedColor;
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = HigthlitedTile;
        isWalkable = true;
    }

    public void Reset()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = DefaultTile;
        isWalkable = false;
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform.position);
        }
    }
}
