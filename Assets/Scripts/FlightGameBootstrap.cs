using UnityEngine;

/// <summary>Бир компонент билан демо саҳнани: самолёт, аэродром, нишон ва HUD'ни яратади.</summary>
public class FlightGameBootstrap : MonoBehaviour
{
    private Material MakeMaterial(Color color)
    {
        Material m = new Material(Shader.Find("Standard"));
        m.color = color;
        return m;
    }

    private GameObject Box(string name, Vector3 position, Vector3 scale, Material material, Transform parent = null)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.SetParent(parent);
        obj.transform.position = position;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().material = material;
        return obj;
    }

    private void Start()
    {
        Material planeMat = MakeMaterial(new Color(0.25f, 0.32f, 0.38f));
        Material runwayMat = MakeMaterial(new Color(0.18f, 0.2f, 0.22f));
        Material targetMat = MakeMaterial(new Color(0.8f, 0.2f, 0.15f));
        Material groundMat = MakeMaterial(new Color(0.22f, 0.38f, 0.2f));
        Box("Ground", new Vector3(0, -1.2f, 80), new Vector3(1600, 1, 1800), groundMat);
        Box("Runway", new Vector3(0, -0.55f, 20), new Vector3(32, 0.25f, 650), runwayMat);

        GameObject plane = new GameObject("Training fighter");
        plane.transform.position = new Vector3(0, 1f, -220f);
        Box("Fuselage", plane.transform.position, new Vector3(2.2f, 1.8f, 12f), planeMat, plane.transform);
        Box("Wings", plane.transform.position, new Vector3(22f, 0.45f, 3.5f), planeMat, plane.transform);
        Box("Tail", plane.transform.position + new Vector3(0, 3.1f, -4.7f), new Vector3(0.45f, 5f, 2.5f), planeMat, plane.transform);
        Box("Horizontal stabilizer", plane.transform.position + new Vector3(0, 0.3f, -5f), new Vector3(8f, 0.3f, 2f), planeMat, plane.transform);
        GameObject nose = new GameObject("Gun muzzle");
        nose.transform.SetParent(plane.transform);
        nose.transform.localPosition = new Vector3(0, 0, 7f);
        nose.transform.localRotation = Quaternion.identity;

        Rigidbody rb = plane.AddComponent<Rigidbody>();
        AircraftFlightController flight = plane.AddComponent<AircraftFlightController>();
        AircraftWeapons weapons = plane.AddComponent<AircraftWeapons>();
        weapons.muzzle = nose.transform;
        BoxCollider collider = plane.AddComponent<BoxCollider>();
        collider.center = new Vector3(0, 0, 0);
        collider.size = new Vector3(22, 5, 12);

        for (int i = 0; i < 5; i++)
        {
            GameObject target = Box("Practice target", new Vector3((i - 2) * 32f, 30f + (i % 2) * 15f, 180f + i * 95f), new Vector3(8f, 8f, 8f), targetMat);
            target.AddComponent<TargetDummy>();
        }

        GameObject cameraObject = new GameObject("Flight camera");
        Camera cam = cameraObject.AddComponent<Camera>();
        cameraObject.AddComponent<AudioListener>();
        AircraftCamera follow = cameraObject.AddComponent<AircraftCamera>();
        follow.aircraft = plane.transform;
        follow.viewCamera = cam;
        cameraObject.transform.position = plane.transform.position - plane.transform.forward * 18f + plane.transform.up * 6f;

        GameObject uiObject = new GameObject("Mobile flight controls");
        MobileFlightUI ui = uiObject.AddComponent<MobileFlightUI>();
        ui.aircraft = flight;
        ui.aircraftCamera = follow;
        ui.weapons = weapons;

        RenderSettings.ambientLight = new Color(0.65f, 0.68f, 0.72f);
        if (Camera.main != null && Camera.main != cam) Destroy(Camera.main.gameObject);
    }
}
