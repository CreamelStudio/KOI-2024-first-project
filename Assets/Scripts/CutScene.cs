using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
public class CutScene : MonoBehaviour
{

    public PostProcessVolume PostVolume;
    private Bloom bloom;
    public float bloomintensity = 70f;

    void Start()
    {
        DOTween.To(() => bloomintensity, x => bloomintensity = x, 3.54f, 2).SetEase(Ease.InQuad);
    }

    private void Update()
    {
        PostVolume.profile.TryGetSettings(out bloom);
        {
            bloom.intensity.value = bloomintensity;
        }
    }
}
