using TMPro;
using UnityEngine;

public class DigimonStatusUI : MonoBehaviour
{
    public GameObject panel;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;

    public DigimonFollow digimon;

    void Start()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Toggle();
    }

    void Toggle()
    {
        bool active = !panel.activeSelf;
        panel.SetActive(active);

        if (active)
            UpdateUI();
    }

    void UpdateUI()
    {
        nameText.text = digimon.Name;
        levelText.text = "Level: " + digimon.Level;
        hpText.text = "HP: " + digimon.stats.Hp;
        attackText.text = "Attack: " + digimon.stats.MagicAttack;
        defenseText.text = "Defense: " + digimon.stats.MagicDefense;
    }
}
