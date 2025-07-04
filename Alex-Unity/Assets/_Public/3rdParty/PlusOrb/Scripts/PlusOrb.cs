using UnityEngine;
using TMPro;

public class PlusOrb : MonoBehaviour
{
    public int addAmount = 1;
    public TextMeshPro textDisplay;

    private void Start()
    {
        //1〜5の乱数を決定
        addAmount = Random.Range(1, 6);

        //上に表示
        if(textDisplay != null)
        {
            textDisplay.text = "" + addAmount.ToString();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "HeadSprite")
        {
            SnakeFollowMouse snake = other.GetComponentInParent<SnakeFollowMouse>();
            if (snake == null)
            {
                return;
            }

            for (int i = 0; i < addAmount; i++)
            {
                snake.AddTail();
            }

            Destroy(gameObject);
        }
    }
}
