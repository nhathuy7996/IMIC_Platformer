using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPooling : ObjectPooling_base<BulletPooling>
{
    public List<GameObject> PoolBullet => Pool;
}
