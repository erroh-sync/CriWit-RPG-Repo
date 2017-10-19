using UnityEngine;
using System.Collections;
 
[ExecuteInEditMode]
public class __Camera_Effects_Controller : MonoBehaviour
{
    public static __Camera_Effects_Controller Instance;

    [SerializeField]
    private Material motionBlurMat;

    [SerializeField]
    private float motionBlurPercentage = 0.1f;
    
    private RenderTexture[] screenBuffer = new RenderTexture[2];

    [SerializeField]
    private Animation anim;

    private void Start()
    {
        Instance = this;

        screenBuffer[0] = new RenderTexture(1280, 720, 16, RenderTextureFormat.ARGB32);
        screenBuffer[0].Create();
        screenBuffer[1] = new RenderTexture(1280, 720, 16, RenderTextureFormat.ARGB32);
        screenBuffer[1].Create();
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        motionBlurMat.SetTexture("_BufferTex", screenBuffer[0]);
        motionBlurMat.SetFloat("_BlurFactor", motionBlurPercentage);

        Graphics.Blit(source, screenBuffer[1], motionBlurMat);


        Graphics.Blit(screenBuffer[1], destination);
        Graphics.Blit(screenBuffer[1], screenBuffer[0]);
    }

    public void PlayerDeathFadeStep()
    {
        motionBlurPercentage = Mathf.Lerp(motionBlurPercentage, 1.0f, 0.1f * Time.deltaTime);
    }

    public void CamPlayAnim(string animName)
    {
        anim.Stop();
        anim.Play(animName);
    }
}