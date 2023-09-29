using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerScript : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
