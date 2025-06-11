using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(BlockHP))]
[RequireComponent(typeof(BlockCollision))]
public class BlockController : MonoBehaviour
{
    [Header("Components")]
    public TextMeshPro hpText;

    private BlockHP blockHP;
    private BlockCollision blockCollision;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        blockHP = GetComponent<BlockHP>();
        blockCollision = GetComponent<BlockCollision>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (blockHP != null)
        {
            blockHP.SetController(this);
        }
    }

    private void Start()
    {
        UpdateHPDisplay();
        UpdateColorByHP();
    }

    private void Update()
    {
        if (blockHP != null && blockHP.GetHP() <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHPDisplay()
    {
        if (hpText != null && blockHP != null)
        {
            hpText.text = blockHP.GetHP().ToString();
        }
    }

    // BlockHP ‚©‚çŒÄ‚Ño‚³‚ê‚éiŒ¸­Žž‚Éj
    public void OnHPChanged()
    {
        UpdateHPDisplay();
        UpdateColorByHP();
    }

    private void UpdateColorByHP()
    {
        if(spriteRenderer == null || blockHP == null)
        {
            return;
        }

        int hp = blockHP.GetHP();
        float t = Mathf.Clamp01(hp / 50f);

        Color lowColor = Color.yellow;
        Color highColor = new Color(0.8f, 0f, 0f);

        spriteRenderer.color = Color.Lerp(lowColor, highColor, t);
    }
}
