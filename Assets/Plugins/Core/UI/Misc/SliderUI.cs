using UnityEngine;
using UnityEngine.UI;

#if TMP
using TMPro;
#endif

namespace TDC.UI
{
    public class SliderUI : CoreUI
    {
        #region Data

        public enum TTypeValue
        {
            Value,
            Percent
        }

        [Header("Data")] public TTypeValue typeValue;
        [Range(0f, 1f)] public float lerp;
        public bool showValue;

        [Header("DataUI")]
#if TMP
        public TextMeshProUGUI txtValue;
        #else
        public Text txtValue;
#endif

        [Header("LocalData")] private Slider _Slider;

        #endregion

        #region Core

        public void Initialization(float CurValue, float MaxValue)
        {
            _Slider = GetComponent<Slider>();

            if (_Slider)
            {
                _Slider.maxValue = MaxValue;

                SetValue(CurValue);
            }
        }

        public void SetValue(float _Value)
        {
            if (lerp > 0)
            {
                SetSlider(_Slider, Mathf.Lerp(_Slider.value, _Value, lerp));
            }
            else
            {
                SetSlider(_Slider, _Value);
            }

            SetTxtValue();
        }

        private void SetTxtValue()
        {
            if (showValue)
            {
                SetActive(txtValue.gameObject, true);

                switch (typeValue)
                {
                    case TTypeValue.Value:
                        SetString(txtValue.gameObject, _Slider.value + "%");
                        break;

                    case TTypeValue.Percent:
                        SetString(txtValue.gameObject, _Slider.value * 100f / _Slider.maxValue + "%");
                        break;
                }
            }
            else
            {
                SetActive(txtValue.gameObject, false);
            }
        }

        #endregion
    }
}