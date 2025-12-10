namespace _Game.Scripts.Behaviours
{
    public interface IInteractable
    {
        bool canInteract { get; }
        void Interact();
    }
}
