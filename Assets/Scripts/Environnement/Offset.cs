using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offset : MonoBehaviour
{
    [SerializeField] protected int offset;
    private SpriteRenderer sprite;
    [SerializeField] protected Transform Object;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite && Object)
            sprite.sortingOrder = 10000 - (int) (Object.position.y * 100) + offset;
    }
}
