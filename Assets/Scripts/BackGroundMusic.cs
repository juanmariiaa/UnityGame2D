using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    private static BackGroundMusic instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Mantiene la música en todas las escenas
        }
        else
        {
            Destroy(gameObject); // Destruye duplicados si ya existe una instancia
        }
    }
}