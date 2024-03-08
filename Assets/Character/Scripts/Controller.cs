using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Knossos.Character
{

[RequireComponent(typeof(CharacterController), typeof(AnimationController))]
[RequireComponent(typeof(DodgeController), typeof(AttackController))]
public class Controller : MonoBehaviour
{
    public Config config;

    private CharacterController characterController;
    private AnimationController animationController;
    private DodgeController dodgeController;
    private AttackController attackController;
    private Camera mainCam;

    // Player stats
    public Vector3 movement;
    private Quaternion targetRotation;
    private float _speed = 10f;
    private float targetSpeed;
    public float speed
    {
        get => _speed;
        set => targetSpeed = value;
    }

    private float startHeight;

    // Movement Logic

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animationController = GetComponent<AnimationController>();
        dodgeController = GetComponent<DodgeController>();
        attackController = GetComponent<AttackController>();
        mainCam = Camera.main;
        startHeight = transform.position.y;
    }

    private void Start()
    {
        speed = config.speed;
        targetRotation = Quaternion.LookRotation(transform.forward);
        InputManager.onClearedInput += OnBufferClear;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, startHeight, transform.position.z);
        _speed = Mathf.Lerp(speed, targetSpeed, .9f);

        if (!dodgeController.dashing)
        {
            movement = new Vector3(InputManager.inputs.horizontal, 0, InputManager.inputs.vertical);
            movement = Quaternion.Euler(0, mainCam.transform.rotation.eulerAngles.y, 0) * movement;
            movement = Vector3.ClampMagnitude(movement, 1);
            characterController.Move(movement * Time.deltaTime * speed * attackController.attMoveSpeedMult);
        }
        else
            characterController.Move(movement.normalized * Time.deltaTime * speed);

        if (movement.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(movement, Vector3.up);
        }
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, config.rotationSpeed * attackController.attRotSpeedMult * Time.deltaTime);

        dodgeController.UpdateDodge();
        attackController.UpdateAttack();

    }

    private void OnBufferClear(BufferedInput input)
    {
        if (input == BufferedInput.Dodge)
            animationController.ResetDodge();
        else if (input == BufferedInput.Attack)
            animationController.ResetAttack();
    }
}

}