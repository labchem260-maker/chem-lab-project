using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;

    private GameObject heldObject = null;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactDistance, Color.red);
            Interact();
        }
    }

    void Interact()
    {
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out TestTube tube))
            {
                if (heldObject == null && !tube.isPicked)
                {
                    PickUp(tube.gameObject);
                }
            }

            if (hit.transform.TryGetComponent(out Beaker beaker))
            {

                if (heldObject != null && beaker.CanPlace())
                {
                    Place(beaker);
                    return;
                }

                if (heldObject == null)
                {
                    GameObject tubeFromBeaker = beaker.RemoveTestTube();
                    if (tubeFromBeaker != null)
                    {
                        PickUp(tubeFromBeaker);
                    }
                }
            }
        }
    }

    void PickUp(GameObject obj)
    {
        heldObject = obj;

        TestTube tube = obj.GetComponent<TestTube>();
        tube.isPicked = true;

        //Disable physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        //Disable collider
        Collider col = obj.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        // Attach to camera
        obj.transform.SetParent(cameraTransform);
        obj.transform.localPosition = new Vector3(0, -0.2f, 1f);
        obj.transform.localRotation = Quaternion.identity;
    }

    void Place(Beaker beaker)
    {
        TestTube tube = heldObject.GetComponent<TestTube>();
        tube.isPicked = false;

        // Re-enable physics
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Re-enable collider
        Collider col = heldObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        beaker.PlaceTestTube(heldObject);

        heldObject = null;
    }
}