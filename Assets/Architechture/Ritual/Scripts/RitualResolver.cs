using UnityEngine;
using Architechture.Ritual.Data;
using System.Collections.Generic;
using System.Linq;

public class RitualResolver : MonoBehaviour
{
    [Header("Recipes Database")]
    public List<RitualRecipe> recipes;

    [Header("Words Database")]
    public List<RitualWordData> words;

    /// <summary>
    /// Пытается найти подходящий рецепт на основе предметов в круге и ценности жертвы.
    /// </summary>
    public RitualRecipe ResolveRecipe(RitualComponentId[] currentComponents, int currentSacrificeValue)
    {
        foreach (var recipe in recipes)
        {
            if (recipe.minSacrificeValue > currentSacrificeValue)
            {
                Debug.Log($"[RitualResolver] Рецепт '{recipe.id}' отклонен: не хватает жертвы (нужно {recipe.minSacrificeValue}, есть {currentSacrificeValue})");
                continue;
            }

            if (IsSequenceMatch(currentComponents, recipe.requiredComponents))
            {
                Debug.Log($"[RitualResolver] Рецепт '{recipe.id}' успешно подошел!");
                return recipe;
            }
            else
            {
                Debug.Log($"[RitualResolver] Рецепт '{recipe.id}' отклонен: комбинация предметов не совпала.");
            }
        }
        Debug.Log("[RitualResolver] Ни один рецепт из базы не подошел под текущие условия круга.");
        return null; // Ничего не подошло
    }

    /// <summary>
    /// Ищет слово в базе данных для определения цвета/модификатора.
    /// </summary>
    public RitualWordData ResolveWord(string inputWord)
    {
        string lowerInput = inputWord.Trim().ToLower();
        return words.FirstOrDefault(w => w.word.ToLower() == lowerInput);
    }

    private bool IsSequenceMatch(RitualComponentId[] current, RitualComponentId[] required)
    {
        if (current.Length != required.Length) return false;

        for (int i = 0; i < current.Length; i++)
        {
            if (current[i] != required[i])
                return false;
        }

        return true;
    }
}
