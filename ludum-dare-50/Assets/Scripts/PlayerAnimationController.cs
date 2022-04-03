using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using Zenject;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _aimingCamera;
    [Inject] Player _player;


    void Update()
    {
        _aimingCamera.Priority = _player.isHolding ? 20 : 0;
    }
}