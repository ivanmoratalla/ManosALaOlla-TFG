using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    // Serialized
    [SerializeField] private Image progressBar;

    // Private *****
    private Camera _camera;
    private KitchenAppliance _followingItem;

    // MonoBehavior Methods
    private void Awake()
    {
        _camera = Camera.main;
        transform.localScale = Vector3.zero;
    }

    private void OnEnable()
    {
        LeanTween.scale(gameObject, Vector3.one, 0.5f).setEase(LeanTweenType.easeOutBack);
    }

    void Update()
    {
        if (!_followingItem) return;
        Vector3 progressUiPosition = _camera.WorldToScreenPoint(_followingItem.transform.position);
        progressUiPosition = new Vector3(progressUiPosition.x, progressUiPosition.y - 45, progressUiPosition.z);
        transform.position = progressUiPosition;
    }



    // Public Methods
    public void Set(KitchenAppliance item)
    {
        _followingItem = item;
        _followingItem.OnProgressChange += Item_OnItemProgressChange;
    }

    // Private Methods
    private void Item_OnItemProgressChange(object sender, float progressNormalized)
    {
        progressBar.fillAmount = progressNormalized;

        if (!(progressNormalized >= 0.9)) return;

        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack);

        StartCoroutine(DestroyWithDelay(1f));
    }

    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_followingItem) _followingItem.OnProgressChange -= Item_OnItemProgressChange;

        Destroy(gameObject);
    }
}