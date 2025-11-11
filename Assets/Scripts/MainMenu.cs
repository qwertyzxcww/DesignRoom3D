using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider _sliderWidth;
    [SerializeField] private Slider _sliderHeight;
    [SerializeField] private TextMeshProUGUI _textWidth;
    [SerializeField] private TextMeshProUGUI _textHeight;
    [SerializeField] private TMP_InputField _inputProjectName;
    [SerializeField] private TextMeshProUGUI _rulesText;

    private void Start()
    {
        _sliderWidth.minValue = 0;
        _sliderWidth.maxValue = 10;
        _sliderWidth.wholeNumbers = true;

        _sliderHeight.minValue = 0;
        _sliderHeight.maxValue = 10;
        _sliderHeight.wholeNumbers = true;

        _sliderWidth.onValueChanged.AddListener(_ => UpdateLabels());
        _sliderHeight.onValueChanged.AddListener(_ => UpdateLabels());
        UpdateLabels();
    }

    /// <summary>Обновляет текст рядом со слайдерами.</summary>
    private void UpdateLabels()
    {
        if (_textWidth != null) _textWidth.text = $"Ширина: {_sliderWidth.value}";
        if (_textHeight != null) _textHeight.text = $"Длина: {_sliderHeight.value}";
    }

    /// <summary>Проверяет корректность параметров комнаты.</summary>
    private bool ValidateRoom()
    {
        var width = (int)_sliderWidth.value;
        var height = (int)_sliderHeight.value;
        string name = _inputProjectName != null ? _inputProjectName.text.Trim() : "";

        if (width < 3 || height < 3)
        {
            _rulesText.text = "Размер комнаты должен быть не меньше 3x3.";
            _rulesText.color = Color.red;
            return false;
        }

        if (name.Length < 3 || name.Length > 8)
        {
            _rulesText.text = "Название проекта: от 3 до 8 символов.";
            _rulesText.color = Color.red;
            return false;
        }

        _rulesText.text = "Все параметры корректны!";
        _rulesText.color = Color.green;
        return true;
    }

    /// <summary>Создаёт комнату, если данные корректны.</summary>
    public void StartProject()
    {
        if (!ValidateRoom()) return;

        var settings = RoomSettings.Instance;
        settings.ProjectName = _inputProjectName.text;
        settings.RoomWidth = (int)_sliderWidth.value;
        settings.RoomHeight = (int)_sliderHeight.value;

        SceneManager.LoadScene(1);
    }

    /// <summary>Закрывает приложение.</summary>
    public void Exit()
    {
        Application.Quit();
    }
}
