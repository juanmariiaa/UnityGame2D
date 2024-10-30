using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        // Actualiza la posición de la cámara solo en el eje X, siguiendo al jugador
        transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
    }
}