using System.Collections.Generic;
using UnityEngine;

namespace TDC.CameraEngine.Effect
{
    [AddComponentMenu("TDC/Camera/Effect/Ripple")]
    public class RippleEffect : MonoBehaviour
    {
        #region Unity

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, material);
        }

        #endregion

        #region Data

        public int maxCount = 1;

        public AnimationCurve waveform = new AnimationCurve(
            new Keyframe(0.00f, 0.50f, 0, 0),
            new Keyframe(0.05f, 1.00f, 0, 0),
            new Keyframe(0.15f, 0.10f, 0, 0),
            new Keyframe(0.25f, 0.80f, 0, 0),
            new Keyframe(0.35f, 0.30f, 0, 0),
            new Keyframe(0.45f, 0.60f, 0, 0),
            new Keyframe(0.55f, 0.40f, 0, 0),
            new Keyframe(0.65f, 0.55f, 0, 0),
            new Keyframe(0.75f, 0.46f, 0, 0),
            new Keyframe(0.85f, 0.52f, 0, 0),
            new Keyframe(0.99f, 0.50f, 0, 0)
        );

        [Range(0.01f, 1.0f)] public float refractionStrength = 0.5f;

        public Color reflectionColor = Color.gray;

        [Range(0.01f, 1.0f)] public float reflectionStrength = 0.7f;

        [Range(1.0f, 3.0f)] public float waveSpeed = 1.25f;

        [Range(0.0f, 2.0f)] public float dropInterval = 0.5f;

        [SerializeField] [HideInInspector] private Shader shader;

        private class Droplet
        {
            private Vector2 position;
            private bool state;
            private float time;

            public Droplet()
            {
                time = 1000;
            }

            public void CoreReset()
            {
                position = new Vector2(Random.value, Random.value);
                time = 0;
            }

            public void Target(Vector2 worldPosition)
            {
                position.x = worldPosition.x * 100 / Screen.width / 100;
                position.y = worldPosition.y * 100 / Screen.height / 100;

                time = 0;
                state = true;
            }

            public void CoreUpdate()
            {
                if (state)
                {
                    time += Time.deltaTime;

                    if (time > 3f)
                    {
                        state = false;
                    }
                }
            }

            public Vector4 MakeShaderParameter(float aspect)
            {
                return new Vector4(position.x * aspect, position.y, time, 0);
            }
        }

        private readonly List<Droplet> droplets = new List<Droplet>();
        private Texture2D gradTexture;
        private Material material;
        private float timer;
        private int dropCount;

        #endregion

        #region Core

        public void Initialization()
        {
            for (var i = 0; i < maxCount; i++) droplets.Add(new Droplet());

            gradTexture = new Texture2D(128, 1, TextureFormat.Alpha8, false);
            gradTexture.wrapMode = TextureWrapMode.Clamp;
            gradTexture.filterMode = FilterMode.Bilinear;
            for (var i = 0; i < gradTexture.width; i++)
            {
                var x = 1.0f / gradTexture.width * i;
                var a = waveform.Evaluate(x);
                gradTexture.SetPixel(i, 0, new Color(a, a, a, a));
            }

            gradTexture.Apply();

            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            material.SetTexture("_GradTex", gradTexture);

            UpdateShaderParameters();
        }

        public void CoreUpdate()
        {
            /*if (dropInterval > 0)
            {
                timer += Time.deltaTime;
                while (timer > dropInterval)
                {
                    Emit();
                    timer -= dropInterval;
                }
            }*/

            /*foreach (var d in droplets) d.Update();

            UpdateShaderParameters();*/
        }

        private void UpdateShaderParameters()
        {
            var c = GetComponent<Camera>();

            for (var i = 0; i < droplets.Count; i++)
                material.SetVector("_Drop1", droplets[i].MakeShaderParameter(c.aspect));

            material.SetColor("_Reflection", reflectionColor);
            material.SetVector("_Params1", new Vector4(c.aspect, 1, 1 / waveSpeed, 0));
            material.SetVector("_Params2", new Vector4(1, 1 / c.aspect, refractionStrength, reflectionStrength));
        }

        public void Emit()
        {
            droplets[dropCount++ % droplets.Count].CoreReset();
        }

        public void TargetEmit(Vector2 worldPosition)
        {
            droplets[dropCount++ % droplets.Count].Target(Camera.main.WorldToScreenPoint(worldPosition));
        }

        #endregion
    }
}