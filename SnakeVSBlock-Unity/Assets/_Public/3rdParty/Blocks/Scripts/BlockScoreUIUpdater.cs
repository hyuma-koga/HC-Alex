using UnityEngine;
using TMPro;

public class BlockScoreUIUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text score2Text;
    [SerializeField] private BlockDestroyManager destroyManager;
    [SerializeField] private GameObject startScreenUI; // �� �X�^�[�g��ʎQ��

    private void Update()
    {
        if (destroyManager == null) return;

        int currentScore = destroyManager.GetTotalDestroyedHP();

        // scoreText �̓X�^�[�g��ʒ�������\��
        if (scoreText != null)
        {
            bool isStartScreenActive = startScreenUI != null && startScreenUI.activeSelf;
            scoreText.gameObject.SetActive(!isStartScreenActive);
            scoreText.text = $"{currentScore}";
        }

        // score2Text �͏�ɕ\��
        if (score2Text != null)
        {
            score2Text.text = $"({currentScore}pt)";
        }
    }
}
