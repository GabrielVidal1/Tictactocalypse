using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ButtonEvent : MonoBehaviour
{
    [SerializeField] private RawImage eventTexture;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI keyText;

    [SerializeField] private Animator changeAnimator;

    private RawImage changeAnimatorImage;
    private Animator animator;

    public void Initialize(string keyString)
    {
        keyText.text = keyString;
        animator = GetComponent<Animator>();
        changeAnimatorImage = changeAnimator.GetComponent<RawImage>();
    }

    public void SetTexture(Texture image)
    {
        TriggerChangeEvent(image);
    }

    public IEnumerator StartCooldown(float duration)
    {
        float start = Time.time;
        cooldownImage.fillAmount = 1f;
        while (Time.time < start + duration)
        {
            cooldownImage.fillAmount = 1f - (Time.time - start) / duration;
            yield return 0;
        }
        cooldownImage.fillAmount = 0f;
    }

    public void PressButton()
    {

    }

    public void TriggerDisabled()
    {
        //animator.SetTrigger("Disabled");
    }

    public void TriggerPress()
    {
        animator.SetTrigger("Press");
    }

    private Texture temp;

    public void TriggerChangeEvent(Texture texture)
    {
        temp = texture;
        changeAnimatorImage.texture = texture;
        changeAnimator.SetTrigger("Change");
        Invoke("UpdateTexture", 0.5f);
    }

    public void UpdateTexture()
    {
        eventTexture.texture = temp;
    }

}
