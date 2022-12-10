using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private GameObject _noiseImage, _buttonImage;
    private Camera _mainCamera;
    private bool _isMakingNoise = false;
    private void Awake() {
        _mainCamera = FindObjectOfType<Camera>();
        FaceToCamera();
    }
    private void FaceToCamera() =>
            transform.localRotation = Quaternion.LookRotation(_mainCamera.transform.position);
    public void ActivateNoiseImage(){
        _noiseImage.SetActive(!_noiseImage.activeSelf);
        _isMakingNoise = !_isMakingNoise;
    } 
    public void ActivateButtonImage(){
        if (_isMakingNoise) _buttonImage.SetActive(!_buttonImage.activeSelf);
    }
}
