using UnityEngine;
using static UnityEditor.Progress;

public class Character : Entity
{
    [Header("Interact Settings")]
    [SerializeField] float detectionRadius = 3f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask obstructionLayer; // vai ser a default old
    [SerializeField] KeyCode input = KeyCode.E;
    GameObject currentTarget; // alvo visivel mais proximo

    [Header("Visual Settings")]
    [SerializeField] GameObject interactFeedback;
    [SerializeField] Material outlineMaterial; // shader

    private void Start() { if (interactFeedback != null) { interactFeedback.SetActive(false); } }

    private void Update()
    {
        DetectObjects();
        if (UnityEngine.Input.GetKeyDown(input)) { Interact(); }
    }

    public override void Movement()
    {
        throw new System.NotImplementedException();
    }

    #region Interact
    private void DetectObjects()
    {
        // basicamente detecto o gameObject por collider
        // depois mando um raycast no divo

        // criando a deteccao em formato de esfera
        // e retornando os divos que estao na layerMask q eu escolhi
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, interactableLayer);

        // inicializando p encontrar o menor gameObject
        // tem que ser infinito para todas as outras distancias serem menores
        GameObject closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in hitColliders)
        {
            Vector3 direction = (collider.transform.position - transform.position).normalized; // distancia normalizada entre gameObject e player
            float distance = Vector3.Distance(transform.position, collider.transform.position); // distancia real :P

            // verificando se tem algo atrapalhando a visao do player ate o gameObject
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distance, interactableLayer | obstructionLayer))
            {
                if (hit.collider.gameObject == collider.gameObject)
                {
                    Debug.Log("Interactive object detected: " + collider.name);
                    if (distance < closestDistance) // se tiver mais de varios objetos determiando o mais perto
                    {
                        closest = collider.gameObject;
                        closestDistance = distance;

                        if (interactFeedback != null)
                        {
                            interactFeedback.SetActive(true);
                            interactFeedback.transform.position = closest.transform.position + new Vector3(0, 2f, 0);
                        }
                    }
                }
                else { Debug.Log(hit.collider.gameObject.name + " is blocking the view to " + collider.name); }

                Debug.DrawRay(transform.position, direction * distance, Color.red); // desenhando o raycast
            }
        }

        // se nada foi encontrado entao so desativar o feedback
        if (closest == null && interactFeedback != null) { interactFeedback.SetActive(false); }

        HighlightTarget(closest);
    }

    private void Interact()
    {
        if (currentTarget != null)
        {
            Debug.Log("Interacting with: " + currentTarget.name);
            if (currentTarget.GetComponent<Car>() == true)
            {
                if (interactFeedback != null) { interactFeedback.SetActive(false); }
                Car target = currentTarget.GetComponent<Car>();
                GetInCar(); // chamando metodo de entrar no carro >.<
            }
        }
    }

    private void GetInCar()
    {
        
    }

    private void HighlightTarget(GameObject newTarget)
    {
        // removendo highlight do antigo alvo :S
        if (currentTarget != null)
        {
            var renderer = currentTarget.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (var mat in renderer.materials)
                {
                    if (mat.shader.name == "Shader Graphs/OutlineShader")
                    {
                        Debug.Log("ok!");
                        mat.SetFloat("_Scale", 0f);
                    }
                    Debug.Log("mat.shader.name: " + mat.shader.name);
                }
            }
        }

        // adicionando highlight no novo alvo
        currentTarget = newTarget;
        if (currentTarget != null)
        {
            var renderer = currentTarget.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (var mat in renderer.materials)
                {
                    if (mat.shader.name == "Shader Graphs/OutlineShader")
                    {
                        Debug.Log("ok 2!");
                        mat.SetFloat("_Scale", 1.2f);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // para ver no editor ne
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
    #endregion

}
