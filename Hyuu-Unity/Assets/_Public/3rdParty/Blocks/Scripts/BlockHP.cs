using UnityEngine;

public class BlockHP : MonoBehaviour
{
    public int hp = 10;

    private BlockController controller;

    private void Awake()
    {
        controller = GetComponent<BlockController>();
    }

    public void SetController(BlockController controller)
    {
        this.controller = controller;
    }

    public int GetHP()
    {
        return hp;
    }

    public void ReduceHP(int amount)
    {
        hp -= amount;
        if (hp < 0)
        {
            hp = 0;
        }

        if (controller != null)
        {
            controller.OnHPChanged();
        }
    }

    public bool IsDepleted()
    {
        return hp <= 0;
    }
}
