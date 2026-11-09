using UnityEngine;

/// <summary>Энг содда кузатувчи камера; C тугмаси ташқи ва кокпит кўринишини алмаштиради.</summary>
public class AircraftCamera : MonoBehaviour
{
    public Transform aircraft;
    public Camera viewCamera;
    private bool cockpit;

    private void LateUpdate()
    {
        if (aircraft == null || viewCamera == null) return;
        Vector3 targetPosition = cockpit
            ? aircraft.position + aircraft.TransformVector(new Vector3(0f, 1.15f, 1.4f))
            : aircraft.position - aircraft.forward * 18f + aircraft.up * 6f;
        Quaternion targetRotation = cockpit
            ? aircraft.rotation
            : Quaternion.LookRotation(aircraft.position + aircraft.forward * 8f - targetPosition, aircraft.up);
        viewCamera.transform.position = Vector3.Lerp(viewCamera.transform.position, targetPosition, Time.deltaTime * 5f);
        viewCamera.transform.rotation = Quaternion.Slerp(viewCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    public void ToggleView() => cockpit = !cockpit;
}
