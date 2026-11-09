using UnityEngine;

/// <summary>Саҳнадаги содда нишонлар учун йўқолиш ва очко.</summary>
public class TargetDummy : MonoBehaviour
{
    public static int Score { get; private set; }

    public void Hit()
    {
        Score += 100;
        Destroy(gameObject);
    }
}
