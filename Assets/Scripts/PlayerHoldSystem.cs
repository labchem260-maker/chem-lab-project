using UnityEngine;

public class PlayerHoldSystem : MonoBehaviour
{
    public static PlayerHoldSystem instance;

    [SerializeField] private Transform holdPoint;

    private GameObject heldObject;

    private void Awake()
    {
        instance = this;
    }

    // 🧪 PICKUP
    public void TryPickUp(TestTube tube)
    {
        if (heldObject != null) return;
        if (tube.isPicked) return;

        GameObject obj = tube.gameObject;
        heldObject = obj;

        tube.isPicked = true;

        // Disable physics
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Disable ALL colliders
        foreach (Collider col in obj.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // Attach to hold point
        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }

    // 🧴 BEAKER INTERACTION
    public void TryInteractBeaker(Beaker beaker)
    {
        // PLACE
        if (heldObject != null && beaker.CanPlace())
        {
            Place(beaker);
            return;
        }

        // PICK FROM BEAKER
        if (heldObject == null)
        {
            GameObject tube = beaker.RemoveTestTube();
            if (tube != null)
            {
                TryPickUp(tube.GetComponent<TestTube>());
            }
        }
    }

    void Place(Beaker beaker)
    {
        TestTube tube = heldObject.GetComponent<TestTube>();

        // 🚫 Disable physics BEFORE placing
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        foreach (Collider col in heldObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }

        // ✅ Place at correct position (no physics interference)
        beaker.PlaceTestTube(heldObject);

        // ✅ Re-enable collider AFTER placement
        foreach (Collider col in heldObject.GetComponentsInChildren<Collider>())
        {
            col.enabled = true;
        }

        // ⚠️ KEEP KINEMATIC (VERY IMPORTANT)
        // DO NOT turn physics back on inside beaker

        tube.isPicked = false;
        heldObject = null;
    }
}