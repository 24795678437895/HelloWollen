using UnityEngine;

/// <summary>Телефонда OnGUI орқали сенсорли бошқарув ва маълумот панели.</summary>
public class MobileFlightUI : MonoBehaviour
{
    public AircraftFlightController aircraft;
    public AircraftCamera aircraftCamera;
    public AircraftWeapons weapons;
    private bool firing;
    private int joystickFinger = -1;
    private Vector2 joystick;

    private void Update()
    {
        firing = false;
        if (Input.touchCount == 0) { joystickFinger = -1; joystick = Vector2.zero; return; }
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            Vector2 gui = new Vector2(t.position.x, Screen.height - t.position.y);
            if (gui.x < Screen.width * 0.48f && gui.y > Screen.height * 0.48f)
            {
                joystickFinger = t.fingerId;
                Vector2 center = new Vector2(Screen.width * 0.2f, Screen.height * 0.78f);
                joystick = Vector2.ClampMagnitude((gui - center) / (Screen.height * 0.13f), 1f);
            }
            if (gui.x > Screen.width * 0.68f && gui.y > Screen.height * 0.65f) firing = true;
        }
        if (aircraft != null) aircraft.SetControls(joystick.y, joystick.x);
    }

    private void OnGUI()
    {
        if (aircraft == null) return;
        float scale = Mathf.Clamp(Screen.width / 900f, 0.7f, 1.5f);
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(scale, scale, 1f));
        GUI.skin.label.fontSize = 20;
        GUI.skin.button.fontSize = 18;
        GUI.Label(new Rect(18, 15, 310, 35), $"ТЕЗЛИК  {aircraft.SpeedKmh:0} км/соат");
        GUI.Label(new Rect(18, 48, 260, 35), $"БАЛАНДЛИК  {aircraft.Altitude:0} м");
        GUI.Label(new Rect(18, 81, 260, 35), $"ЙЎНАЛИШ  {aircraft.Heading:000}°");
        GUI.Label(new Rect(18, 114, 260, 35), $"ГАЗ  {aircraft.Throttle * 100:0}%   ҲИСОБ  {TargetDummy.Score}");

        float w = Screen.width / scale;
        float h = Screen.height / scale;
        if (GUI.Button(new Rect(w - 180, h - 190, 155, 54), "ЎҚ УЗИШ")) weapons?.Fire();
        if (GUI.Button(new Rect(w - 180, h - 126, 72, 48), "ГАЗ +")) aircraft.ChangeThrottle(0.08f);
        if (GUI.Button(new Rect(w - 100, h - 126, 75, 48), "ГАЗ −")) aircraft.ChangeThrottle(-0.08f);
        if (GUI.Button(new Rect(w - 180, h - 70, 72, 48), aircraft.GearDown ? "ШАССИ ↑" : "ШАССИ ↓")) aircraft.ToggleGear();
        if (GUI.Button(new Rect(w - 100, h - 70, 75, 48), "КАМЕРА")) aircraftCamera?.ToggleView();
        if (GUI.Button(new Rect(18, h - 70, 115, 48), "ТОРМОЗ")) aircraft.SetBrakes(true);
        if (Event.current.type == EventType.MouseUp) aircraft.SetBrakes(false);
        GUI.Label(new Rect(w * 0.12f, h * 0.67f, 220, 32), "СЕНСОРЛИ ДЖОЙСТИК");
    }
}
