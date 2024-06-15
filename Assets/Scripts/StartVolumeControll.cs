using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartVolumeControll : MonoBehaviour
{

    public PostProcessVolume PostVolume;
    private Bloom bloom;
    public float bloomintensity = 3.54f;
    public float Alpha = 1f;
    public bool IsStart;
    public RawImage StartText;
    public RawImage LogoImg;

    private void Start()
    {
        IsStart = true;
    }

    private void Update()
    {
        PostVolume.profile.TryGetSettings(out bloom);
        {
            bloom.intensity.value = bloomintensity;
            StartText.color = new Color(1, 1, 1, Alpha);
            LogoImg.color = new Color(1, 1, 1, Alpha);
        }
        if (Input.anyKeyDown)
        {
            if (IsStart)
            {
                IsStart = false;
                DOTween.To(() => bloomintensity, x => bloomintensity = x, 70f, 3).SetEase(Ease.InQuad);
                DOTween.To(() => Alpha, x => Alpha = x, 0, 3).SetEase(Ease.InQuad);
                Invoke("NextScreen",3f);
            }
        }
    }

    public void NextScreen()
    {
        SceneManager.LoadScene("InGame");
    }
}
