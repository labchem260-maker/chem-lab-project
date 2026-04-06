using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactorSource; // Camera
    [SerializeField] private float interactRange = 5f;

    [SerializeField] private GameObject crosshair; // Only ONE crosshair

    private IInteractable currentTarget;

    private void Update()
    {
        Ray ray = new Ray(interactorSource.position, interactorSource.forward);

        Debug.DrawRay(ray.origin, ray.direction * interactRange, Color.red, 0.1f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange))
        {
            // Check parent for interactable
            IInteractable interactObj = hitInfo.collider.GetComponentInParent<IInteractable>();

            if (interactObj != null)
            {
                currentTarget = interactObj;
                return;
            }
        }

        currentTarget = null;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (currentTarget != null)
        {
            currentTarget.Interact();
        }
    }
}