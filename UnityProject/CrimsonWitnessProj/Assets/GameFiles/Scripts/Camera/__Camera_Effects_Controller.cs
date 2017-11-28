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

    [SerializeField]
    private Vector2 UVOffset = new Vector2(0,0);

    [SerializeField]
    private RenderTexture[] screenBuffer = new RenderTexture[2];

    [SerializeField]
    private Animation anim;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Create our secondary buffer to be blitted to the screen
        //screenBuffer[1] = new RenderTexture(1280, 720, 16, RenderTextureFormat.ARGB32);
        //screenBuffer[1].Create();
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

    public void EnterCombatFade()
    {
        StartCoroutine("C_EnterCombatFade");
    }

    IEnumerator C_EnterCombatFade()
    {
        for (var i = 0; i < 500; i++)
        {
            motionBlurPercentage = Mathf.Lerp(motionBlurPercentage, 0.95f, 0.08f);
            yield return null;
        }
    }

    public void BeginCrossFade()
    {
        StopAllCoroutines();
        StartCoroutine("C_CrossFade");
    }

    IEnumerator C_CrossFade()
    {
        float targ = motionBlurPercentage;
        motionBlurPercentage = 1.0f;
        for (var i = 0; i < 100; i++)
        {
            motionBlurPercentage = Mathf.Lerp(motionBlurPercentage, targ, 0.1f);
            yield return null;
        }
        motionBlurPercentage = targ;
    }

    public void CamPlayAnim(string animName)
    {
        BeginCrossFade();
        if (anim == null)
            anim = this.GetComponent<Animation>();

        anim.Stop();
        anim.Play(animName);
    }
}