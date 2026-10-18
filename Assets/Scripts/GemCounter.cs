using TMPro;
using UnityEngine;

public class GemCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    private int gems;

    private void Start()
    {
        UpdateText();
    }

    public void AddGem()
    {
        gems++;
        UpdateText();
    }

    private void UpdateText()
    {
        counterText.text = "Gemas: " + gems;
    }
}
