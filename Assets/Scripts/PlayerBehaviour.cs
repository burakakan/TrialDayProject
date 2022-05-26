using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    ControlActions actions;
    
    [SerializeField]
    float speed = 5f, shootingRange = 10f, shootingSpeed = 20f;
    [SerializeField]
    GameObject ball;

    Coroutine move;
    Vector2 inputVector;
    Vector3 direction;

    private void Awake()
    {
        actions = new ControlActions();
        actions.Player.Move.performed += StartMoving;
        actions.Player.Move.canceled += StopMoving;
        SubscribeShoot();
    }

    private void OnEnable() => actions.Player.Enable();
    private void OnDisable() => actions.Player.Disable();

    private void Shoot(InputAction.CallbackContext context)
    {
        actions.Player.Shoot.performed -= Shoot;
        ball.transform.DOPunchPosition(Vector3.forward * shootingRange, shootingRange / shootingSpeed, 0, 0).OnComplete(SubscribeShoot);
    }
    private void SubscribeShoot() => actions.Player.Shoot.performed += Shoot;
    
    private void StartMoving(InputAction.CallbackContext context)
    {
        if (move != null)
            StopCoroutine(move);
        move = StartCoroutine(Move(context));
    }
    private void StopMoving(InputAction.CallbackContext context)
    {
        StopCoroutine(move);
    }

    private IEnumerator Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        direction = new Vector3(inputVector.x, 0f, inputVector.y);

        transform.DORotate(Quaternion.LookRotation(direction, Vector3.up).eulerAngles, 0.25f);

        while (true)
        {
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            yield return null;
        }
    }


}
