using UnityEngine;

public class Beaker : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform[] placementPoints;

    private int currentCount = 0;

    public bool CanPlace()
    {
        return currentCount < placementPoints.Length;
    }

    public void Interact()
    {
        PlayerHoldSystem.instance.TryInteractBeaker(this);
    }

    public void PlaceTestTube(GameObject testTube)
    {
        if (!CanPlace()) return;

        Transform point = placementPoints[currentCount];

        testTube.transform.SetParent(point);
        testTube.transform.position = point.position;
        testTube.transform.rotation = point.rotation;

        currentCount++;
    }

    public GameObject RemoveTestTube()
    {
        if (currentCount <= 0) return null;

        currentCount--;

        Transform point = placementPoints[currentCount];

        if (point.childCount > 0)
        {
            Transform tube = point.GetChild(0);
            tube.SetParent(null);
            return tube.gameObject;
        }

        return null;
    }
}