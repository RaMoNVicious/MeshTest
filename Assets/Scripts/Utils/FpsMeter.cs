using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [RequireComponent(typeof(Text))]
    public class FpsMeter : MonoBehaviour
    {
        [SerializeField]
        [Range(0.1f, 10f)]
        private float updateInterval = 0.5f;
        
        private Text _txtValue;

        private int _frameCount = 0;

        private float _measureTime = 0f;

        void Awake()
        {
            _txtValue = GetComponent<Text>();
        }

        void Update()
        {
            _frameCount++;
            _measureTime += Time.unscaledDeltaTime;

            if (_measureTime > updateInterval)
            {
                var fps = _frameCount / _measureTime;
                _txtValue.text = $"FPS: {fps:0000}";

                _frameCount = 0;
                _measureTime = 0f;
            }
        }
    }
}