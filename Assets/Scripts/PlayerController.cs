using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float InteractRange; 
    public float MoveSpeed;
    public GameManager gameManager;
    public Rigidbody rb;
    [Header("Info")]
    [SerializeField] public List<IInteractibles> interactibles;
    public Transform visualRef;
    private void Awake() {
        interactibles = new();
    }
    private void Start() {
        
        rb = GetComponent<Rigidbody>();
        gameManager.inputActions.PlayerControls.Movement.performed += ctx => {
            Move(ctx.ReadValue<Vector2>());    
        };
        gameManager.inputActions.PlayerControls.Movement.canceled += ctx => {
            Move(Vector2.zero);    
        };
        gameManager.inputActions.PlayerControls.Interact.performed += _ => {
            Interact();
        };
    }

    private void OnEnable() {
        gameManager = GameManager.Instance;
        gameManager.inputActions.PlayerControls.Enable();
    }
    private void OnDisable() {
        gameManager.inputActions.PlayerControls.Disable();
    }

    public void Move(Vector2 dir){
        Vector3 move = new Vector3(-dir.x, 0, -dir.y) * MoveSpeed;
        rb.velocity = move;
        visualRef.LookAt(transform.position + move);
    }
    public void Interact(){
        Debug.Log("Trying to Interact");
        Debug.Log(interactibles.Count);
        float shortestDistance = float.MaxValue;
        IInteractibles closestInteractible = interactibles[0];

        for (int i = 0; i < interactibles.Count; i++){
            float dist = Vector3.Distance(transform.position, ((MonoBehaviour)interactibles[i]).transform.position);
            if(dist < shortestDistance){
                closestInteractible = interactibles[i];
                shortestDistance = dist;
            }
        }

        
        if(shortestDistance < InteractRange) 
            closestInteractible.Interact();
    }



}
