using System;
using Polyart;
using Unity.Cinemachine;
using UnityEngine;

public class GuildPlayerController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCamera;
    [SerializeField] private FirstPersonController_Dreamscape playerController;
    
    [Header("Required")]
    [SerializeField] private CinemachineCamera questTableCamera;

    [SerializeField] private Canvas questTableCanvas;

    void Start()
    {
        if (playerController == null)
        {
            playerController = GetComponent<FirstPersonController_Dreamscape>();
        }

        if (mainCamera == null)
        {
            mainCamera = GetComponentInChildren<CinemachineCamera>();
        }
    }
    
    
    public void SetActiveQuestCamera(bool active)
    {
        questTableCamera.gameObject.SetActive(active);
        questTableCanvas.gameObject.SetActive(active);
        if (active)
        {
            playerController.enabled = false;
            Cursor.visible = true;   
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            playerController.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetActiveQuestCamera(false);
            playerController.enabled = true;
        }
    }
}
