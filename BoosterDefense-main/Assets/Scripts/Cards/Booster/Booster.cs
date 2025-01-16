using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
    public BoosterStats boosterStats = null;
    public Image image;
    public TMP_Text Name;
    public TMP_Text Price;
    public Button button;
    public int price;

    public void SetStats(BoosterStats booster, bool free = false)
    {
        boosterStats = booster;
        image.sprite = booster.icon;
        Name.text = booster.nameBooster;
        if (free) {
            price = 0;
            Price.enabled = true;
            Price.text = "Choose";
        } else {
            price = booster.priceGold;
            Price.enabled = true;
            Price.text = price.ToString();
        }
    }

    void Update()
    {
        if (boosterStats && !BoosterDrawCardUI.instance.isDrawing){
            button.interactable = RessourceManager.instance.GetRessourceAmount(RessourceType.gold) >= price;
        } else {
            button.interactable = false;
        }
    }
}
