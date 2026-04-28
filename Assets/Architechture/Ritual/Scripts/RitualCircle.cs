using UnityEngine;
using Architechture.Ritual.Data;
public class RitualCircle : MonoBehaviour
{
    [Header("Slots")]
    public RitualSlot[] slots = new RitualSlot[5];

    [Header("Summon")]
    public Transform summonPoint;

    public int CurrentSacrificeValue { get; private set; }

    public void AddSacrifice(SacrificeData sacrifice)
    {
        if (sacrifice == null)
            return;

        CurrentSacrificeValue += sacrifice.value;
    }

    public RitualComponentId[] GetComponentSequence()
    {
        RitualComponentId[] sequence = new RitualComponentId[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                sequence[i] = RitualComponentId.None;
            else
                sequence[i] = slots[i].CurrentComponentId;
        }

        return sequence;
    }

    public void Clear(bool destroyComponents)
    {
        CurrentSacrificeValue = 0;

        foreach (RitualSlot slot in slots)
        {
            if (slot != null)
                slot.Clear(destroyComponents);
        }
    }
}