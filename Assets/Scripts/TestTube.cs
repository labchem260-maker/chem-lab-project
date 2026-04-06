using UnityEngine;

public class TestTube : MonoBehaviour, IInteractable
{
    public bool isPicked = false;

    public void Interact()
    {
        PlayerHoldSystem.instance.TryPickUp(this);
    }
}