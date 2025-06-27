using UnityEngine;
using TMPro;

public class BlockTextUpdater : MonoBehaviour
{
    public TextMeshPro hpText;
    private BlockHP blockHP;

    private void Start()
    {
        blockHP = GetComponent<BlockHP>();
    }

    private void Update()
    {
        if(blockHP != null && hpText != null)
        {
            hpText.text = blockHP.hp.ToString();
        }
    }
}
