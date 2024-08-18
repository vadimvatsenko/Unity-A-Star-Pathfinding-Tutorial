using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTargetBlock : MonoBehaviour
{
    [SerializeField] LayerMask _hitLayers;
    private bool _isPlayer;

    private void Start()
    {
        _isPlayer = true;
    }

    private void Update()
    {
        if (_isPlayer)
        {
            Vector3 mouse = Input.mousePosition; // получить позицию мыши
            Ray castPoint = Camera.main.ScreenPointToRay(mouse); // луч куда указывает мышь
            RaycastHit hit; // сохраняет позицию, в которую попала мышь

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, _hitLayers))
            {
                this.transform.position = hit.point;
            }
        }
    }

    private void OnMouseDown() // встроенный метод в Юнити, он позволяет отследить нажатие на объект на котором находится скрипт
    {
        _isPlayer = true;
    }

    private void OnMouseUp()
    {
        _isPlayer = false;
    }

}
