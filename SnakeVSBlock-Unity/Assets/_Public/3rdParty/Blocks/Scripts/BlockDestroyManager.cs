using UnityEngine;

public class BlockDestroyManager : MonoBehaviour
{
    private int totalDestroyedHP = 0;

    public void AddDestroyedHP(int hp)
    {
        totalDestroyedHP += hp;
    }

    public int GetTotalDestroyedHP()
    {
        return totalDestroyedHP;
    }

    public void ResetScore()
    {
        totalDestroyedHP = 0;
    }

}
