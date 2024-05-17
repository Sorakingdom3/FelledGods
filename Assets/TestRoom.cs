using UnityEngine;
using UnityEngine.EventSystems;

public class TestRoom : MonoBehaviour, IPointerDownHandler
{
    private bool seleccionada;
    private Vector2 posicionInicial;

    public void OnPointerDown(PointerEventData eventData)
    {
        seleccionada = true;
        posicionInicial = transform.position;
    }

    private void Update()
    {
        if (seleccionada)
        {
            Vector2 posicionMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(posicionMouse.x, posicionMouse.y, transform.position.z);
            if (seleccionada)
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log(posicionMouse);
                    Debug.Log(transform.position);

                    GameObject objetoColisionado = DetectarObjetivo();
                    if (objetoColisionado != null)
                    {
                        Debug.Log("La carta se soltó sobre: " + objetoColisionado.name);
                        // Haz aquí lo que necesites con el objeto colisionado
                    }

                }
        }
    }

    private GameObject DetectarObjetivo()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        Debug.Log(hit.collider != null);
        if (hit.collider != null && hit.collider != this)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
