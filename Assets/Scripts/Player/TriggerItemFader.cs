using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerItemFader : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        IsFadeoutEffect(collision, true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        IsFadeoutEffect(collision, false);
    }

    private void IsFadeoutEffect(Collider2D collision, bool isfadeout)
    {
        ItemFader[] itemFader = collision.GetComponentsInChildren<ItemFader>();
        if (itemFader.Length > 0)
        {
            foreach (var item in itemFader)
            {
                if (isfadeout)
                    item.FadeOut();
                else
                    item.FadeIn();
            };
        }
    }
}
