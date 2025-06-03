using System.Collections;
using UnityEngine;

public class Character : Entity
{
    [SerializeField] bool isInCar = false;
    Car currentCar;
    [SerializeField] GameObject playerModel;

    [Header("Interact Settings")]
    [SerializeField] float detectionRadius = 3f;
    [SerializeField] LayerMask interactableLayer;
    [SerializeField] LayerMask obstructionLayer; // vai ser a default old
    GameObject currentTarget; // alvo visivel mais proximo

    [Header("Visual Settings")]
    [SerializeField] GameObject interactFeedback;
    [SerializeField] Material outlineMaterial; // shader

    [Header("Move Settings")]
    CharacterController characterController;
    float moveValue = 0f;
    [SerializeField] Animator animatorController;
    KeyCode inputRunning = KeyCode.LeftShift, inputBoxing = KeyCode.Space, input = KeyCode.E;
    float boxingWeight = 0f;
    int armsLayerIndex;
    [SerializeField] bool isBoxing = false;

    private void Start()
    {
        if (interactFeedback != null) { interactFeedback.SetActive(false); }

        armsLayerIndex = animatorController.GetLayerIndex("Arms");
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isInCar == false)
        {
            HandleMovement();
            HandleBoxing();
        }

        DetectObjects();
        if (Input.GetKeyDown(input)) { Interact(); }
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
        if (isInCar == false && currentTarget != null && currentTarget.GetComponent<Car>())
        {
            Debug.Log("Interacting with: " + currentTarget.name);

            if (interactFeedback != null) { interactFeedback.SetActive(false); }
            Car target = currentTarget.GetComponent<Car>();
            GetInCar(); // chamando metodo de entrar no carro >.<

        }
        else if (isInCar == true) { ExitCar(); }
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

    #region Car
    private void GetInCar()
    {
        if (isInCar == true) return;
        currentCar = currentTarget.GetComponent<Car>();

        if (currentCar != null)
        {
            isInCar = true;

            // desativando controle do personagem e ativando do carro
            SetRagdollState(false);
            if (playerModel != null) { playerModel.SetActive(false); }
            currentCar.getCanMove = true;
        }
    }

    private void ExitCar()
    {
        if (isInCar != true || currentCar == null) return;

        isInCar = false;
        currentCar.getCanMove = false;
        transform.SetParent(null);

        // reposicionando personagem proximo ao carro
        transform.position = currentCar.getExitPoint.position;
        if (playerModel != null) { playerModel.SetActive(true); }

        SetRagdollState(false);
        currentCar = null;
    }
    #endregion

    #region Movement
    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");

        if (Input.GetButton("Horizontal"))
        {
            Vector3 move = transform.right * x + transform.forward;
            characterController.Move(move * speed * Time.deltaTime);

            // controlando valor de animacao da corrida
            if (Input.GetKey(inputRunning)) { moveValue = Mathf.MoveTowards(moveValue, 1f, Time.deltaTime); }
            else { moveValue = Mathf.MoveTowards(moveValue, 0.25f, Time.deltaTime); }

            animatorController.SetFloat("Move", moveValue);
        }
        else
        {
            moveValue = Mathf.MoveTowards(moveValue, 0f, Time.deltaTime * 5f);
            animatorController.SetFloat("Move", moveValue);
        }
    }

    void HandleBoxing()
    {
        if (Input.GetKeyDown(inputBoxing) && !isBoxing)
        {
            isBoxing = true;
            animatorController.Play("Boxing", armsLayerIndex, 0f);
            StartCoroutine(BoxingLayerWeightRoutine());
        }

        // transicao suave visualmente entre layers
        float targetWeight = isBoxing ? 1f : 0f;
        boxingWeight = Mathf.Lerp(boxingWeight, targetWeight, Time.deltaTime * 5f);
        animatorController.SetLayerWeight(armsLayerIndex, boxingWeight);
    }

    IEnumerator BoxingLayerWeightRoutine()
    {
        // esperando o tempo da animacao
        AnimatorClipInfo[] clips = animatorController.GetCurrentAnimatorClipInfo(armsLayerIndex);
        if (clips.Length > 0) { yield return new WaitForSeconds(clips[0].clip.length); }
        isBoxing = false;

        int currentAnimationBaseLayer = animatorController.GetCurrentAnimatorClipInfoCount(0);
        animatorController.CrossFade(currentAnimationBaseLayer, 0.1f, 0);
    }
    #endregion

    private void SetRagdollState(bool enabled)
    {
        Rigidbody[] rigidBodies = GetComponentsInChildren<Rigidbody>();
        foreach (var rb in rigidBodies)
        {
            rb.isKinematic = !enabled;
            rb.detectCollisions = enabled;
        }

        Animator animator = GetComponent<Animator>();
        if (animator != null) { animator.enabled = !enabled; }
    }
}