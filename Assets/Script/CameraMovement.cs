using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    private Quaternion initialRotation;
    private Quaternion gyroOffset;

    private SpawnObjectsInCircle spawnObjectsInCircle;

    private void Start()
    {
        if (AttitudeSensor.current != null)
            InputSystem.EnableDevice(AttitudeSensor.current);

        initialRotation = transform.rotation;
        gyroOffset = Quaternion.Euler(90f, 0f, 0f);
        spawnObjectsInCircle = FindFirstObjectByType<SpawnObjectsInCircle>();
    }

    private void Update()
    {

        if (AttitudeSensor.current == null) return;

        Quaternion deviceRotation = ConvertRotation(AttitudeSensor.current.attitude.ReadValue());
        transform.rotation = initialRotation * gyroOffset * deviceRotation;
        ShootRay();
    }

    private void ShootRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1000f))
        {
            Debug.Log("Hit: " + hit.collider.name);
            spawnObjectsInCircle.DestroyOnHit();

        }
    }

    private static Quaternion ConvertRotation(Quaternion rawRotation)
    {
        return new Quaternion(rawRotation.x, rawRotation.y, -rawRotation.z, -rawRotation.w);
    }
    private void OnDestroy()
    {
        if (AttitudeSensor.current != null)
            InputSystem.DisableDevice(AttitudeSensor.current);
    }

    
}
