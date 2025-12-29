using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D customCursor; // Drag your texture here
    [SerializeField] private Vector2 hotSpot = Vector2.zero; // e.g., (16, 16) for center on 32x32
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto; // Auto: hardware if possible

    void Start()
    {
        // Set custom cursor (hides default automatically)
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
    }

    // Reset to default system cursor
    private void ResetCursor()
    {
    //    Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    private void SetCursor()
    {
        Cursor.SetCursor(customCursor, hotSpot, cursorMode);
    }

    public void SwitchCursorState(GuildPlayerState state)
    {
        switch (state)
        {
            case GuildPlayerState.Default:
                ResetCursor();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                break;
            case GuildPlayerState.MainTable:
                SetCursor();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;

                break;
            case GuildPlayerState.QuestTable:
                SetCursor();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GuildPlayerState.QuestResultTable:
                SetCursor();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case GuildPlayerState.Shop:
                SetCursor();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
}