using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyLevelUI : MonoBehaviour
{

    public TMP_Text concatenatedText;


    public void UpdateEnemyDisplay(int level, string enemyType)
    {
        concatenatedText.text = $"Level: {level} - {enemyType}";
    }
}
