using System;
using Polyart;
using Unity.Cinemachine;
using UnityEngine;

public enum GuildPlayerState
{
    Default,
    MainTable,
    QuestTable,
    QuestResultTable,
    Shop,
    QuestTaking
}

public class GuildPlayerController : MonoBehaviour
{
    private static GuildPlayerController _instance;

    private CursorManager cursorManager;
    public static GuildPlayerController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<GuildPlayerController>();
            }
            return _instance;
        }
    }

    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private FirstPersonController_Dreamscape playerController;

    public Transform MainCameraTransform => mainCamera != null ? mainCamera.transform : null;

    [Header("Required")]
    [SerializeField] private CinemachineCamera questTableCamera;
    [SerializeField] private CinemachineCamera mainTableCamera;
    [SerializeField] private CinemachineCamera resultTableCamera;
    [SerializeField] private CinemachineCamera questTakingCamera;
    [SerializeField] private Canvas questTableCanvas;
    [SerializeField] private Canvas mainTableCanvas;
    [SerializeField] private Canvas resultTableCanvas;
    [SerializeField] private Canvas shopCanvas;
    [SerializeField]
    private Canvas questTakingCanvas;
    
    private GuildPlayerState currentState = GuildPlayerState.Default;



    [Header("Inventory")]
    [SerializeField] private CharacterInventory inventory;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        if (cursorManager == null)
        {
            cursorManager = GetComponent<CursorManager>();
        }
        if (playerController == null)
        {
            playerController = GetComponent<FirstPersonController_Dreamscape>();
        }

        if (mainCamera == null)
        {
            mainCamera = GetComponentInChildren<CinemachineCamera>();
        }
    }
    
    
    public void SwitchState(GuildPlayerState newState)
    {
        // Skip if already in the requested state
        if (currentState == newState)
            return;
            
        // Deactivate current state
        switch (currentState)
        {
            case GuildPlayerState.Default:
                if (mainCamera != null)
                    mainCamera.gameObject.SetActive(false);
                break;
            case GuildPlayerState.MainTable:
                if (mainTableCamera != null)
                    mainTableCamera.gameObject.SetActive(false);
                if (mainTableCanvas != null)
                    mainTableCanvas.gameObject.SetActive(false);
                break;
            case GuildPlayerState.QuestTable:
                if (questTableCamera != null)
                    questTableCamera.gameObject.SetActive(false);
                if (questTableCanvas != null)
                    questTableCanvas.gameObject.SetActive(false);
                break;
            case GuildPlayerState.QuestResultTable:
                if  (resultTableCamera != null)
                    resultTableCamera.gameObject.SetActive(false);
                if (resultTableCanvas != null)
                    resultTableCanvas.gameObject.SetActive(false);
                break;
            case GuildPlayerState.Shop:
                if (shopCanvas != null)
                {
                    shopCanvas.gameObject.SetActive(false);
                }
                break;
            case GuildPlayerState.QuestTaking:
                questTakingCamera.gameObject.SetActive(false);
                if (questTakingCanvas != null)
                {
                    questTakingCanvas.gameObject.SetActive(false);
                }
                break;
        }
        
        // Activate new state
        currentState = newState;
        switch (newState)
        {
            case GuildPlayerState.Default:
                if (mainCamera != null)
                    mainCamera.gameObject.SetActive(true);
                if (playerController != null)
                {
                    playerController.enabled = true;
                }
                break;
            case GuildPlayerState.MainTable:
                if (mainTableCamera != null)
                    mainTableCamera.gameObject.SetActive(true);
                if (mainTableCanvas != null)
                    mainTableCanvas.gameObject.SetActive(true);
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
                break;
            case GuildPlayerState.QuestTable:
                if (questTableCamera != null)
                    questTableCamera.gameObject.SetActive(true);
                if (questTableCanvas != null)
                    questTableCanvas.gameObject.SetActive(true);
                if (playerController != null)
                {
                    playerController.enabled = false;
                }
                break;
            case GuildPlayerState.QuestResultTable:
                resultTableCamera.gameObject.SetActive(true);
                resultTableCanvas.gameObject.SetActive(true);
                playerController.enabled = false;
                break;
            case GuildPlayerState.Shop:
                playerController.enabled = false;
                shopCanvas.gameObject.SetActive(true);
                break;
            case GuildPlayerState.QuestTaking:
                questTakingCamera.gameObject.SetActive(true);
                questTakingCanvas.gameObject.SetActive(true);
                playerController.enabled = false;
                break;
        }
        cursorManager.SwitchCursorState(newState);
    }
    
    public void SetActiveQuestCamera(bool active)
    {
        if (active)
        {
            SwitchState(GuildPlayerState.QuestTable);
        }
        else
        {
            SwitchState(GuildPlayerState.Default);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchState(GuildPlayerState.Default);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchState(GuildPlayerState.Shop);
        }
    }

    public void PutIntoInventory(GameObject item)
    {
        if (inventory != null)
        {
            inventory.TakeToInventory(item);
        }
        else
        {
            Debug.LogWarning("GuildPlayerController: Inventory is not assigned.");
        }
    }

    public GameObject GetFromInventory()
    {
        if (inventory != null)
        {
            return inventory.GetFromInventory();
        }
        else
        {
            Debug.LogWarning("GuildPlayerController: Inventory is not assigned.");
            return null;
        }
    }
    
    public bool CanTakeItem()
    {
        return inventory != null && inventory.CanTakeItem();
    }
    
    public bool IsItemQuest()
    {
        return inventory != null && inventory.IsItemQuest();
    }
}