using UnityEngine;
using Architechture.Ritual.Data;

public class RitualPerformer : MonoBehaviour
{
    [Header("Core References")]
    public RitualCircle ritualCircle;
    public RitualSacrificeZone sacrificeZone;
    public RitualResolver resolver;
    public RitualEffectHandler effectHandler;
    public RitualInputUI inputUI;

    [Header("Settings")]
    public bool destroyItemsOnSuccess = true;
    public bool destroySacrificeOnUse = true;

    private int pendingSacrificeValue = 0;

    /// <summary>
    /// Вызывается кнопкой или триггером (например, алтарем), чтобы принести жертвы в зоне.
    /// </summary>
    public void PerformSacrifice()
    {
        if (sacrificeZone == null) 
        {
            Debug.LogWarning("[RitualPerformer] Алтарь подал сигнал на убийство, но поле SacrificeZone не назначено в инспекторе!");
            return;
        }
        
        Debug.Log("[RitualPerformer] Сигнал на жертвоприношение получен. Опрашиваем SacrificeZone...");
        int value = sacrificeZone.ConsumeSacrifices(destroySacrificeOnUse);
        
        if (value > 0)
        {
            pendingSacrificeValue += value;
            Debug.Log($"[RitualPerformer] Жертва принесена успешно! Накопленная ценность: {pendingSacrificeValue}");
            effectHandler.PlaySacrificeEffect(sacrificeZone.transform.position);
        }
        else
        {
            Debug.Log("[RitualPerformer] Попытка принести жертву, но в зоне никого нет.");
        }
    }

    /// <summary>
    /// Вызывается, когда игрок становится на точку ввода (InteractionPoint).
    /// Открывает UI для ввода слова.
    /// </summary>
    public void StartRitualInput()
    {
        if (inputUI == null) return;
        inputUI.Show(OnWordSubmitted);
    }

    private void OnWordSubmitted(string word)
    {
        ExecuteRitual(word);
    }

    private void ExecuteRitual(string typedWord)
    {
        RitualComponentId[] components = ritualCircle.GetComponentSequence();
        
        string currentCombo = "[ ";
        for(int i=0; i<components.Length; i++) {
            currentCombo += components[i].ToString() + " ";
        }
        currentCombo += "]";

        Debug.Log($"[Ritual] ЗАПУСК РИТУАЛА. Слоты: {currentCombo}. Ценность жертвы: {pendingSacrificeValue}. Введено слово: '{typedWord}'");

        // 1. Пробуем найти рецепт по предметам и жертвам
        RitualRecipe recipe = resolver.ResolveRecipe(components, pendingSacrificeValue);

        if (recipe == null)
        {
            // Ошибка: комбинация неверна или не хватает жертвы
            FailRitual($"Сборка провалилась. Текущие слоты: {currentCombo}. Жертва: {pendingSacrificeValue}");
            return;
        }

        // 2. Пробуем распознать слово
        RitualWordData wordData = resolver.ResolveWord(typedWord);
        if (wordData == null)
        {
            // Ошибка: слово введено неверно
            FailRitual($"Рецепт '{recipe.id}' подошел, но слово '{typedWord}' введено с ошибкой (или не найдено в базе)!");
            return;
        }

        // 3. Успех: Спавним мебель
        SucceedRitual(recipe, wordData);
    }

    private void FailRitual(string reason)
    {
        Debug.LogWarning("[Ritual] FAILED: " + reason);
        effectHandler.PlayFailureEffect(ritualCircle.summonPoint.position);
        
        // Сбрасываем накопленную жертву при провале
        pendingSacrificeValue = 0;
        
        // Здесь можно добавить наложение негативных баффов на игрока
    }

    private void SucceedRitual(RitualRecipe recipe, RitualWordData wordData)
    {
        Debug.Log($"[Ritual] SUCCESS! Summoning {recipe.resultFurniture.displayName} with color {wordData.resultColor}");
        
        effectHandler.PlaySuccessEffect(ritualCircle.summonPoint.position);

        if (recipe.resultFurniture.prefab != null)
        {
            GameObject furniture = Instantiate(recipe.resultFurniture.prefab, ritualCircle.summonPoint.position, ritualCircle.summonPoint.rotation);
            
            // Применяем цвет к материалу
            Renderer rend = furniture.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                rend.material.color = wordData.resultColor;
            }
        }

        // Очищаем круг
        ritualCircle.Clear(destroyItemsOnSuccess);
        pendingSacrificeValue = 0;
    }
}
