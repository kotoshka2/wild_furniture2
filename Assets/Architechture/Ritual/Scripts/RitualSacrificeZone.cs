using UnityEngine;
using Architechture.Ritual.Data;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class RitualSacrificeZone : MonoBehaviour
{
    private List<RitualSacrificeItem> currentSacrifices = new List<RitualSacrificeItem>();

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        RitualSacrificeItem sacrifice = other.GetComponentInParent<RitualSacrificeItem>();
        if (sacrifice != null && !currentSacrifices.Contains(sacrifice))
        {
            currentSacrifices.Add(sacrifice);
            Debug.Log($"[RitualSacrificeZone] Обнаружена жертва в зоне: {other.gameObject.name} (Ценность: {sacrifice.Data?.value})");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        RitualSacrificeItem sacrifice = other.GetComponentInParent<RitualSacrificeItem>();
        if (sacrifice != null && currentSacrifices.Contains(sacrifice))
        {
            currentSacrifices.Remove(sacrifice);
            Debug.Log($"[RitualSacrificeZone] Жертва покинула зону: {other.gameObject.name}");
        }
    }

    /// <summary>
    /// Приносит все объекты в зоне в жертву, возвращая суммарную ценность.
    /// </summary>
    public int ConsumeSacrifices(bool destroyObjects)
    {
        int totalValue = 0;
        int count = 0;
        foreach (var sacrifice in currentSacrifices)
        {
            if (sacrifice != null && sacrifice.Data != null)
            {
                totalValue += sacrifice.Data.value;
                count++;
                if (destroyObjects)
                {
                    Destroy(sacrifice.gameObject);
                }
            }
        }
        currentSacrifices.Clear();
        Debug.Log($"[RitualSacrificeZone] Сожрано {count} жертв. Общая полученная ценность: {totalValue}");
        return totalValue;
    }
}
