using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float xySpeed = 10f;
    public float rotationSpeed = 100f;
    public float tiltLimit = 15f;

    [SerializeField]
    InputActionReference moveAction;

    public GameObject aimObject;
    public Transform model;

    private void Update()
    {
        Vector2 xyVector = moveAction.action.ReadValue<Vector2>();

        LocalMove(xyVector.x, xyVector.y, xySpeed);
        ClampPosition();
        RotationLook(xyVector.x, xyVector.y, rotationSpeed);
        HorizontalTilt(model, xyVector.x, tiltLimit, .1f);
    }

    void LocalMove(float x, float y, float speed)
    {
        transform.localPosition += new Vector3(x, y, 0) * speed * Time.deltaTime;
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);

    }

    void RotationLook(float h, float v, float speed)
    {
        aimObject.transform.localPosition = new Vector3(h, v, 1);
        gameObject.transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(aimObject.transform.position),
            Mathf.Deg2Rad * speed * Time.deltaTime);

    }

    void HorizontalTilt(Transform target, float axis, float tiltLimit, float lerpTime)
    {
        Vector3 targetEurlerAngles = target.localEulerAngles;
        target.localEulerAngles = new Vector3(
            targetEurlerAngles.x,
            targetEurlerAngles.y,
            Mathf.LerpAngle(targetEurlerAngles.z, -axis * tiltLimit, lerpTime));
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(aimObject.transform.position, .5f);
        Gizmos.DrawSphere(aimObject.transform.position, .15f);
    }
}
