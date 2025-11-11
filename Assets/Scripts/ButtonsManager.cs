using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsManager : MonoBehaviour
{
    [Header("Core")]
    [SerializeField] private RoomEditController _edit;

    [Header("Panels")]
    [SerializeField] private GameObject _panelChoiceMaterial;
    [SerializeField] private GameObject _panelChoiceCategories;
    [SerializeField] private GameObject _panelRest;
    [SerializeField] private GameObject _panelWorkplace;
    [SerializeField] private GameObject _panelDecor;

    /// <summary>Закрывает все панели и сбрасывает режим при запуске.</summary>
    private void Awake()
    {
        CloseAllPanels();
        if (_edit != null) _edit.SetMode(EditMode.None);
    }

    /// <summary>Полный выход из сцены: очистка и возврат в меню.</summary>
    public void OnExit()
    {
        CloseAllPanels();
        if (_edit != null) _edit.SetMode(EditMode.None);

        // Очистка всех объектов
        foreach (var obj in GameObject.FindGameObjectsWithTag("Object"))
            Destroy(obj);

        foreach (var wall in GameObject.FindGameObjectsWithTag("Wall"))
            Destroy(wall);

        foreach (var floor in GameObject.FindGameObjectsWithTag("Floor"))
            Destroy(floor);

        // Сброс настроек комнаты
        if (RoomSettings.Instance != null)
        {
            Destroy(RoomSettings.Instance.gameObject);
            RoomSettings.Instance = null;
        }

        // Возврат в главное меню
        SceneManager.LoadScene(0);
    }


    /// <summary>Переключает в режим поворота.</summary>
    public void OnRotate()
    {
        CloseAllPanels();
        if (_edit != null) _edit.SetMode(EditMode.Rotate);
    }

    /// <summary>Переключает в режим удаления.</summary>
    public void OnDelete()
    {
        CloseAllPanels();
        if (_edit != null) _edit.SetMode(EditMode.Delete);
    }

    /// <summary>Открывает панель выбора материала.</summary>
    public void OnSelectMaterialOpen()
    {
        CloseAllPanels();
        _panelChoiceMaterial.SetActive(true);
    }

    /// <summary>Открывает панель категорий объектов.</summary>
    public void OnSelectObjectOpen()
    {
        CloseAllPanels();
        _panelChoiceCategories.SetActive(true);
    }

    /// <summary>Открывает панель категории Rest.</summary>
    public void OnCategoryRest()
    {
        _panelChoiceCategories.SetActive(false);
        _panelRest.SetActive(true);
    }

    /// <summary>Открывает панель категории Workplace.</summary>
    public void OnCategoryWorkplace()
    {
        _panelChoiceCategories.SetActive(false);
        _panelWorkplace.SetActive(true);
    }

    /// <summary>Открывает панель категории Decor.</summary>
    public void OnCategoryDecor()
    {
        _panelChoiceCategories.SetActive(false);
        _panelDecor.SetActive(true);
    }

    /// <summary>Возвращает к списку категорий.</summary>
    public void OnBackToCategories()
    {
        _panelRest.SetActive(false);
        _panelWorkplace.SetActive(false);
        _panelDecor.SetActive(false);
        _panelChoiceCategories.SetActive(true);
    }

    /// <summary>Закрывает все панели.</summary>
    public void CloseAllPanels()
    {
        _panelChoiceMaterial.SetActive(false);
        _panelChoiceCategories.SetActive(false);
        _panelRest.SetActive(false);
        _panelWorkplace.SetActive(false);
        _panelDecor.SetActive(false);
    }
}
