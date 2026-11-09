using UnityEngine;

/// <summary>Бошланғич учиш модели: кўтариш кучи, тортиш, аэродинамик қаршилик ва бошқарув.</summary>
[RequireComponent(typeof(Rigidbody))]
public class AircraftFlightController : MonoBehaviour
{
    [Header("Учиш параметрлари")]
    [SerializeField] private float maxThrust = 62000f;
    [SerializeField] private float wingArea = 28f;
    [SerializeField] private float liftCoefficient = 0.22f;
    [SerializeField] private float controlTorque = 9500f;
    [SerializeField] private float maxSpeed = 220f;

    private Rigidbody body;
    private float throttle;
    private float pitchInput;
    private float rollInput;
    private float yawInput;
    private bool brakes;

    public float Throttle => throttle;
    public float SpeedKmh => body == null ? 0f : body.velocity.magnitude * 3.6f;
    public float Altitude => transform.position.y;
    public float Heading => transform.eulerAngles.y;
    public bool GearDown { get; private set; } = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.mass = 8500f;
        body.useGravity = true;
        body.drag = 0.015f;
        body.angularDrag = 1.8f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.centerOfMass = new Vector3(0f, -0.25f, 0f);
    }

    private void Update()
    {
        // Компьютерда прототипни текшириш учун клавиатура бошқаруви.
        pitchInput = Mathf.Clamp(Input.GetAxisRaw("Vertical") + (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f), -1f, 1f);
        rollInput = Mathf.Clamp(Input.GetAxisRaw("Horizontal") + (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f), -1f, 1f);
        yawInput = (Input.GetKey(KeyCode.E) ? 1f : 0f) - (Input.GetKey(KeyCode.Q) ? 1f : 0f);
        if (Input.GetKey(KeyCode.LeftShift)) SetThrottle(throttle + 0.35f * Time.deltaTime);
        if (Input.GetKey(KeyCode.LeftControl)) SetThrottle(throttle - 0.35f * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.G)) ToggleGear();
        brakes = Input.GetKey(KeyCode.B);
    }

    private void FixedUpdate()
    {
        Vector3 velocity = body.velocity;
        float speed = velocity.magnitude;
        float speedMs = Mathf.Min(speed, maxSpeed);
        float density = Mathf.Clamp(1f - Mathf.Max(0f, transform.position.y) / 16000f, 0.25f, 1f);
        float dynamicPressure = 0.5f * 1.225f * density * speedMs * speedMs;

        body.AddForce(transform.forward * (maxThrust * throttle), ForceMode.Force);
        float cl = Mathf.Clamp(liftCoefficient + pitchInput * 0.48f, -0.15f, 1.35f);
        body.AddForce(transform.up * (dynamicPressure * wingArea * cl), ForceMode.Force);
        if (speed > 1f)
        {
            Vector3 drag = -velocity.normalized * (dynamicPressure * wingArea * 0.025f);
            body.AddForce(drag, ForceMode.Force);
        }

        Vector3 localAngular = new Vector3(pitchInput, yawInput, -rollInput) * controlTorque;
        body.AddRelativeTorque(localAngular, ForceMode.Force);
        if (brakes && GearDown && transform.position.y < 3f)
            body.AddForce(-Vector3.ProjectOnPlane(velocity, Vector3.up) * 2.5f, ForceMode.Acceleration);
    }

    public void SetControls(float pitch, float roll, float yaw = 0f)
    {
        pitchInput = Mathf.Clamp(pitch, -1f, 1f);
        rollInput = Mathf.Clamp(roll, -1f, 1f);
        yawInput = Mathf.Clamp(yaw, -1f, 1f);
    }

    public void SetThrottle(float value) => throttle = Mathf.Clamp01(value);
    public void ChangeThrottle(float amount) => SetThrottle(throttle + amount);
    public void SetBrakes(bool value) => brakes = value;
    public void ToggleGear() => GearDown = !GearDown;
}
