
using UnityEngine;

public class CameraFlow : MonoBehaviour
{
    [Header("Personaje a seguir")]
    public Transform target;

    [Header("Movimiento suave")]
    public float smoothSpeed = 2f;

    [Header("Posición de cámara")]
    public Vector3 offset = new Vector3(0f, 2f, -10f);

    void LateUpdate()
    {
        // Verificamos que exista un personaje asignado
        if (target == null)
        {
            return;
        }

        // Calculamos la posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Movemos suavemente la cámara
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
