using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
        public float hoverAmount;

    private SpriteRenderer rend;

    public Sprite HigthlitedTile;

    public Sprite DefaultTile;

    public Color highlitedColor;

    public bool isWalkable;

    public LayerMask obstacles;
    private AudioSource source;
    public AudioClip hoverSound;

    GameMaster gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        source = GetComponent<AudioSource>();
    }
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount;
         source.clip = hoverSound;
         source.Play();
    }

    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount;
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
            gm.selectedUnit.Move(this.transform);
        }
    }
}
