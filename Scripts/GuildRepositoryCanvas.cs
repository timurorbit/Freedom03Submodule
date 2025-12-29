using TMPro;
using UnityEngine;

public class GuildRepositoryCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI guildGoldText;

    [SerializeField] private TextMeshProUGUI guildReputationText;


    private GuildRepository repository;

    private void Start()
    {
        repository = GuildRepository.Instance;
    }
    
    
    private void Update()
    {
        if (repository == null) return;

        if (guildGoldText != null)
        {
            guildGoldText.text = repository.Gold.ToString();
        }

        if (guildReputationText != null)
        {
            guildReputationText.text = repository.Reputation.ToString();
        }
    }
}
