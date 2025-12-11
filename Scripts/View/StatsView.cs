using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class StatsView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    [SerializeField] private Image attackIcon;
    [SerializeField] private Image defenseIcon;
    [SerializeField] private Image movementIcon;
    [SerializeField] private Image mageIcon;
    [SerializeField] private Image charismaIcon;
    public void UpdateView(Stats getPrediction)
    {
        rank.text = getPrediction.rank.ToString();
        attackIcon.enabled = getPrediction.skills.Contains(SkillType.Attack);
        defenseIcon.enabled = getPrediction.skills.Contains(SkillType.Defense);
        movementIcon.enabled = getPrediction.skills.Contains(SkillType.Mobility);
        mageIcon.enabled = getPrediction.skills.Contains(SkillType.Intelligence);
        charismaIcon.enabled = getPrediction.skills.Contains(SkillType.Charisma);
    }

    public void UpdateActiveView()
    {
        attackIcon.enabled = true;
        defenseIcon.enabled = true;
        movementIcon.enabled = true;
        mageIcon.enabled = true;
        charismaIcon.enabled = true;
    }
}