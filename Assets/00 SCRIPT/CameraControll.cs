using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    [SerializeField] Vector2 LimitBorder;
    Vector2 FreeZone;
    [SerializeField] Transform _Target;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 Cam_size = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        FreeZone = LimitBorder - Cam_size;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _Target.transform.position;
        if (pos.x < -FreeZone.x )
        {
            pos.x = -FreeZone.x;
        }

        if (pos.x > FreeZone.x)
        {
            pos.x = FreeZone.x;
        }

        if (pos.y < -FreeZone.y)
        {
            pos.y = -FreeZone.y;
        }

        if (pos.y > FreeZone.y)
        {
            pos.y = FreeZone.y;
        }
        Debug.LogError(pos.x + "||" + FreeZone.x);
        pos.z = this.transform.position.z;
        this.transform.position = pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector2.zero, LimitBorder);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector2.zero, FreeZone);
    }
}
