using UnityEngine;
using Architechture.Ritual.Data;


[RequireComponent(typeof(Collider))]
public class RitualSlot : MonoBehaviour
{
    [Header("Snap")]
    [SerializeField] private Transform snapPoint;
    [SerializeField] private float snapForce = 25f;
    [SerializeField] private float dampingForce = 4f;
    [SerializeField] private float releaseDistance = 1.25f;

    [Header("Rotation")]
    [SerializeField] private bool alignRotation = true;
    [SerializeField] private float rotationSpeed = 8f;

    private RitualComponentItem currentItem;
    private Rigidbody currentRigidbody;

    public bool IsOccupied => currentItem != null;

    public RitualComponentId CurrentComponentId
    {
        get
        {
            if (currentItem == null || currentItem.Data == null)
                return RitualComponentId.None;

            return currentItem.Data.id;
        }
    }

    public GameObject CurrentObject
    {
        get
        {
            if (currentItem == null)
                return null;

            return currentItem.gameObject;
        }
    }

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsOccupied)
            return;

        RitualComponentItem item = other.GetComponentInParent<RitualComponentItem>();

        if (item == null || item.Data == null)
            return;

        Rigidbody rb = item.GetComponent<Rigidbody>();

        if (rb == null)
            return;

        currentItem = item;
        currentRigidbody = rb;
        Debug.Log($"[RitualSlot] В слот {gameObject.name} помещен предмет: {item.Data.displayName} (ID: {item.Data.id})");
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentItem == null)
            return;

        RitualComponentItem item = other.GetComponentInParent<RitualComponentItem>();

        if (item != currentItem)
            return;

        Clear(false);
    }

    private void FixedUpdate()
    {
        if (currentItem == null || currentRigidbody == null)
            return;

        Vector3 targetPosition = snapPoint != null ? snapPoint.position : transform.position;
        Vector3 toTarget = targetPosition - currentRigidbody.position;

        if (toTarget.magnitude > releaseDistance)
        {
            Clear(false);
            return;
        }

        Vector3 force = toTarget * snapForce;
        Vector3 damping = -currentRigidbody.linearVelocity * dampingForce;

        currentRigidbody.AddForce(force + damping, ForceMode.Acceleration);

        if (alignRotation)
        {
            Quaternion targetRotation = snapPoint != null ? snapPoint.rotation : transform.rotation;

            currentRigidbody.MoveRotation(
                Quaternion.Slerp(
                    currentRigidbody.rotation,
                    targetRotation,
                    rotationSpeed * Time.fixedDeltaTime
                )
            );
        }
    }

    public void Clear(bool destroyObject)
    {
        if (destroyObject && currentItem != null)
            Destroy(currentItem.gameObject);

        currentItem = null;
        currentRigidbody = null;
    }
}