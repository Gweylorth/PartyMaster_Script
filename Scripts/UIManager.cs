using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text PartyDudesText;
    public Text TimeLeftText;
    public Text SelectionTimeLeftText;
    public Text GameOverText;
    public Text YeahText;
    public Text OopsText;

    public Color Color1;
    public Color Color2;

    private Color currentColor;

    public bool Yeah { get; set; }
    public bool Oops { get; set; }

    public void Update()
    {
        YeahText.gameObject.SetActive(Yeah);
        OopsText.gameObject.SetActive(Oops);
    }

    public void SwapColors()
    {
        if (currentColor == Color1)
        {
            currentColor = Color2;
        }
        else
        {
            currentColor = Color1;
        }

        SelectionTimeLeftText.color = currentColor;
    }

    public void GameOver()
    {
        GameOverText.gameObject.SetActive(true);
        GameOverText.text = (GameManager.Instance.PartyGuests < 50) ? "Caretaker Wins!" : "PartyMaster Wins!";
        SelectionTimeLeftText.gameObject.SetActive(false);
    }
}
