using UnityEngine;
using UnityEngine.Tilemaps;

public class HealingItem : MonoBehaviour
{
    private Tilemap tilemap;
    private Collider2D col;

    private Color activeColor = Color.white;
    private Color inactiveColor = Color.black;

    private bool used = false;

    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        col = GetComponent<Collider2D>();
        ResetItem();
    }

    public bool CanHeal() => !used;

    public void Use()
    {
        used = true;
        if (tilemap != null) tilemap.color = inactiveColor;
        if (col != null) col.enabled = false;
    }

    public void ResetItem()
    {
        used = false;
        if (tilemap != null) tilemap.color = activeColor;
        if (col != null) col.enabled = true;
    }
}
 