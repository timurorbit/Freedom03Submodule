using TMPro;
using UnityEngine;
using UnityEngine.UI;

internal class QuestPredictionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rank;
    
    [Header("Only For Quest Inspecting")]
    [SerializeField] private Image attackIcon;
    [SerializeField] private Image defenseIcon;
    [SerializeField] private Image movementIcon;
    [SerializeField] private Image mageIcon;
    [SerializeField] private Image charismaIcon;
    public void UpdateView(Stats getPrediction)
    {
        UpdateViewRankOnly(getPrediction);
        attackIcon.enabled = getPrediction.GetStatAmount(SkillType.Attack) > 0;
        defenseIcon.enabled = getPrediction.GetStatAmount(SkillType.Defense) > 0;
        movementIcon.enabled = getPrediction.GetStatAmount(SkillType.Mobility) > 0;
        mageIcon.enabled = getPrediction.GetStatAmount(SkillType.Intelligence) > 0;
        charismaIcon.enabled = getPrediction.GetStatAmount(SkillType.Charisma) > 0;
    }

    public void UpdateViewRankOnly(Stats getPrediction)
    {
        if (getPrediction.rank == Rank.None)
        {
            return;
        }
        rank.text = getPrediction.rank.ToString();
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