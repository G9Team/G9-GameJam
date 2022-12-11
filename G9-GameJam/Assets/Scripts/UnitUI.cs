using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUI : MonoBehaviour
{
    [SerializeField] private GameObject _noiseImage, _buttonImage;
    private GameObject _mainCameraLandmark;
    private bool _isMakingNoise = false;
    private void Start() {
        _mainCameraLandmark = GameObject.FindGameObjectWithTag("CameraLandmark");
        FaceToCamera();
    }
    private void FaceToCamera() =>
            transform.rotation = Quaternion.LookRotation(_mainCameraLandmark.transform.position);
    public void ActivateNoiseImage(){
        _noiseImage.SetActive(!_noiseImage.activeSelf);
        _isMakingNoise = !_isMakingNoise;
    } 
    public void ActivateButtonImage(){
        if (_isMakingNoise) _buttonImage.SetActive(true);
    }
    public void DeactivateButtonImage() => _buttonImage.SetActive(false);

}
