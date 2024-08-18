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
            Vector3 mouse = Input.mousePosition; // �������� ������� ����
            Ray castPoint = Camera.main.ScreenPointToRay(mouse); // ��� ���� ��������� ����
            RaycastHit hit; // ��������� �������, � ������� ������ ����

            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, _hitLayers))
            {
                this.transform.position = hit.point;
            }
        }
    }

    private void OnMouseDown() // ���������� ����� � �����, �� ��������� ��������� ������� �� ������ �� ������� ��������� ������
    {
        _isPlayer = true;
    }

    private void OnMouseUp()
    {
        _isPlayer = false;
    }

}
