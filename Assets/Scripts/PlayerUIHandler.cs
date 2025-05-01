using UnityEngine;
using TMPro;
public class PlayerUIHandler : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI promptText;

    // Update is called once per frame
    public void UpdateText(string promptMessage){
        promptText.text = promptMessage;
    }
}
