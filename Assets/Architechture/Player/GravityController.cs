using UnityEngine;
using UnityEngine.InputSystem;

public class GravityController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Input")]
    [SerializeField] private InputActionReference grabAction;
    [SerializeField] private InputActionReference throwAction;

    [Header("Settings")]
    [SerializeField] private LayerMask grabbableLayer;
    [SerializeField] private float grabDistance = 8f;
    [SerializeField] private float holdDistance = 3f;
    [SerializeField] private float moveForce = 80f;
    [SerializeField] private float damping = 8f;
    [SerializeField] private float throwForce = 18f;

    private Rigidbody heldRb;
    private bool isHolding;

    private void OnEnable()
    {
        grabAction.action.Enable();
        throwAction.action.Enable();

        grabAction.action.started += OnGrabStarted;
        grabAction.action.canceled += OnGrabCanceled;
        throwAction.action.started += OnThrowStarted;
    }

    private void OnDisable()
    {
        grabAction.action.started -= OnGrabStarted;
        grabAction.action.canceled -= OnGrabCanceled;
        throwAction.action.started -= OnThrowStarted;
    }

    private void FixedUpdate()
    {
        if (!isHolding || heldRb == null)
            return;

        Vector3 targetPosition = playerCamera.transform.position +
                                 playerCamera.transform.forward * holdDistance;

        Vector3 direction = targetPosition - heldRb.position;

        heldRb.linearVelocity = direction * moveForce * Time.fixedDeltaTime;
        heldRb.linearVelocity -= heldRb.linearVelocity * damping * Time.fixedDeltaTime;
    }

    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        if (heldRb != null)
            return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, grabDistance, grabbableLayer))
        {
            if (hit.rigidbody != null)
            {
                heldRb = hit.rigidbody;
                heldRb.useGravity = false;
                heldRb.linearDamping = 6f;
                isHolding = true;
            }
        }
    }

    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        DropObject();
    }

    private void OnThrowStarted(InputAction.CallbackContext context)
    {
        if (heldRb == null)
            return;

        Rigidbody rb = heldRb;
        DropObject();

        rb.AddForce(playerCamera.transform.forward * throwForce, ForceMode.Impulse);
    }

    private void DropObject()
    {
        if (heldRb == null)
            return;

        heldRb.useGravity = true;
        heldRb.linearDamping = 0f;

        heldRb = null;
        isHolding = false;
    }
}