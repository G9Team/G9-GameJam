#define DEBUG_GUI //show debug GUI
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Events;

public class Unit : MonoBehaviour
{
    //CONFIG
    const float _startNoiseChance = 0.2f; //20% chance start noise per event tick
    const float _nearestNoiseChance = 0.05f; //5% per noising unit
    const float _nearestSilentChance = 0.025f; //2.5% per silent unit

    const float _unitDistance = 5f; //distance


    [SerializeField] private float _timeToFullNoise;
    [SerializeField] private UnitUI _unitUI;
    [SerializeField] private Animator _animator;
    public float noise = 0f;
    Player _player;
    GameController _controller;
    public PositionType _positionType;
    void Start()
    {
        GameController.onTick += OnTick;
        _player = FindObjectOfType<Player>();
        _controller = FindObjectOfType<GameController>();
        _animator.SetBool("IsSitting", _positionType == PositionType.Sit);
        Debug.Log(_positionType);
    }

    void OnTick()
    {
        if (noise > 0f)
            return;
        if (Vector3.Distance(this.transform.position, _player.transform.position) < Player.noiseConsilationDistance)
            return;
        float chance = _startNoiseChance;
        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            if (unit == this) continue;
            if (Vector3.Distance(unit.transform.position, this.transform.position) < _unitDistance)
                chance += unit.noise == 0f ? _nearestNoiseChance * unit.noise : _nearestSilentChance;
        }
        if (Random.Range(0.0f, 1.0f) < chance)
        {
            noise = 0.01f;
            _unitUI.ActivateNoiseImage();
            _animator.SetBool("IsNoising", true);
        }
    }

    public void SetPositionType(PositionType type){
        _positionType = type;
        
    }
    void Update()
    {
        if (noise > 0f)
        {
            noise = Mathf.MoveTowards(noise, 1f, Time.deltaTime * _timeToFullNoise);
            RaycastHit hit;
            if (!Physics.Raycast(this.transform.position, _player.transform.position - transform.position, out hit, Player.noiseConsilationDistance))
            {
                _unitUI.DeactivateButtonImage();
            }
            else
            {
                if (hit.transform.GetComponent<Player>() is null)
                {

                    _unitUI.DeactivateButtonImage();
                }
                else
                {
                    _unitUI.ActivateButtonImage();
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        _controller.AddToScore();
                        noise = 0f;
                        _animator.SetBool("IsNoising", false);
                        _unitUI.ActivateNoiseImage();
                        _unitUI.DeactivateButtonImage();
                    }
                }
            }
        }
    }

    #region DEBUG
#if DEBUG_GUI
    private void OnGUI()
    {
        Vector2 guiPoint = HandleUtility.WorldToGUIPoint(transform.position);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.alignment = TextAnchor.UpperCenter;
        if (noise == 0f)
            GUI.Label(new Rect(guiPoint.x - 50, guiPoint.y - 30, 100f, 30f), "Silent", style);
        else
            GUI.Label(new Rect(guiPoint.x - 50, guiPoint.y - 30, 100f, 30f), "Noising", style);
        GUI.HorizontalScrollbar(new Rect(guiPoint.x - 50, guiPoint.y - 45, 100f, 30f), noise, 0.01f, 0f, 1f);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, transform.up, _unitDistance);
    }
#endif
    #endregion
}
