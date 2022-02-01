using UnityEngine;

namespace Technical.Scripts
{
    /// <summary>
    /// Resize camera rect to fit desired aspect ratio
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class CameraResolutionHandler : MonoBehaviour
    {
        #region Variables

        [Header("Aspect ratio settings")]
        [SerializeField] 
        private int aspectRatioWidth = 9;
        [SerializeField] 
        private int aspectRatioHeight = 16;
        
        [Header("Graphic settings")]
        [SerializeField] 
        private Color clearRectColor = Color.black;
        
        private int _screenSizeX = 0;
        private int _screenSizeY = 0;
        private Camera _cam;

        #endregion

        #region Unity Lifecycle Methods

        public void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        public void OnPreCull()
        {
            if (Application.isEditor) {
            
                return;
            
            }
            
            var wp = _cam.rect;
            
            _cam.rect = new Rect(0, 0, 1, 1);
            
            GL.Clear(true, true, clearRectColor);

            _cam.rect = wp;
        
        }

        // Use this for initialization
        public void Start()
        {
            RescaleCamera();
        }
        
        /*
        Enable this method to handle screen resize (different device orientations)
        public void Update()
        {
            RescaleCamera();
        }
        */
        #endregion

        #region Class Methods
        
        /// <summary>
        /// Calculates current camera rect width and height to match desired aspect ratio
        /// </summary>
        private void RescaleCamera()
        {
            if (Screen.width == _screenSizeX && Screen.height == _screenSizeY) return;

            var aspect = (float)aspectRatioWidth / (float)aspectRatioHeight;
            var windowAspect = (float)Screen.width / (float)Screen.height;
            var scaleHeight = windowAspect / aspect;

            if (scaleHeight < 1.0f)
            {
                var rect = _cam.rect;

                rect.width = 1.0f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1.0f - scaleHeight) / 2.0f; // position rect in center

                _cam.rect = rect;
            }
            else // add pillarbox
            {
                var scaleWidth = 1.0f / scaleHeight;

                var rect = _cam.rect;

                rect.width = scaleWidth;
                rect.height = 1.0f;
                rect.x = (1.0f - scaleWidth) / 2.0f;
                rect.y = 0;

                _cam.rect = rect;
            }

            _screenSizeX = Screen.width;
            _screenSizeY = Screen.height;
        }
        #endregion
    }
}
