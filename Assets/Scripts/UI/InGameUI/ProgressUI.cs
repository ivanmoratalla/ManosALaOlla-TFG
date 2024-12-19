using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressUI : MonoBehaviour
{
    [SerializeField] private Image progressBar;

    private Camera _camera;
    private KitchenAppliance _followingItem;

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


    // M�todo para configurar la barra de progreso con el electrodom�stico que le corresponda
    public void Set(KitchenAppliance appliance)
    {
        _followingItem = appliance;
        _followingItem.OnProgressChange += Item_OnItemProgressChange;
        _followingItem.OnProgressCanceled += OnProgressCanceled;
    }

    // M�todo para cambiar el avance de la barra de progreso de un electrodom�stico
    private void Item_OnItemProgressChange(object sender, float progressNormalized)
    {
        progressBar.fillAmount = progressNormalized;

        if (!(progressNormalized >= 0.9)) return;

        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEase(LeanTweenType.easeInBack);

        StartCoroutine(DestroyWithDelay(1f));
    }

    // M�todo para cancelar la barra de progreso de un electrodom�stico
    private void OnProgressCanceled(object sender, EventArgs e)
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInBack);

        StartCoroutine(DestroyWithDelay(0.3f));
    }

    // N�todo para destruir la barra de progreso
    private IEnumerator DestroyWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_followingItem)
        {
            _followingItem.OnProgressChange -= Item_OnItemProgressChange;
            _followingItem.OnProgressCanceled -= OnProgressCanceled;
        }

        Destroy(gameObject);
    }
}