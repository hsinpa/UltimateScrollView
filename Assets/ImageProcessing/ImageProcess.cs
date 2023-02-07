using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Hsinpa {
    public class ImageProcess : MonoBehaviour
    {

        [SerializeField]
        private Camera camera_main;

        [SerializeField]
        private Camera camera_ui;

        [SerializeField]
        private RenderTexture final_renderRT;

        [SerializeField]
        private Material p_mat;

        public RenderTexture cMainTex;
        public RenderTexture cUITex;

        private const string MaterialNameUITex = "_UITex";
        private const string MaterialNameMainTex = "_MainTex";

        // Start is called before the first frame update
        void Start()
        {
            cMainTex = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            cUITex = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);

            p_mat.SetTexture(MaterialNameUITex, cUITex);
        }

        private void LateUpdate()
        {
            GetRTFromCamera(camera_main, cMainTex);
            GetRTFromCamera(camera_ui, cUITex);

            ExecuteImageSynthesis(cMainTex, cUITex, final_renderRT);
        }

        private void GetRTFromCamera(Camera target_camera, RenderTexture target_rt)
        {
            target_camera.targetTexture = target_rt;
            target_camera.Render();

            RenderTexture.active = target_rt;

            target_camera.targetTexture = null;
            RenderTexture.active = null;
        }

        private void ExecuteImageSynthesis(RenderTexture cMainTex, RenderTexture cUITex, RenderTexture destination) {
            Graphics.Blit(cMainTex, destination, p_mat);
        }

    }
}