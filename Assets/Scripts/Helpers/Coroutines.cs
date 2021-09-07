using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Coroutines
{
    public static IEnumerator Ghost(Collider2D collider, IReadOnlyCollection<SpriteRenderer> renderers, float duration, float ghostAlpha = 0.4f)
    {
        collider.enabled = false;

        foreach (var renderer in renderers)
            renderer.color = renderer.color.ChangeAlpha(ghostAlpha);

        yield return new WaitForSeconds(duration);

        collider.enabled = true;

        foreach (var renderer in renderers)
            renderer.color = renderer.color.ChangeAlpha(1f);
    }
}
