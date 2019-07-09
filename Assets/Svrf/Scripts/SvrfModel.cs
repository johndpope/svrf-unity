﻿using Svrf.Models.Media;
using System.Threading.Tasks;
using Svrf.Unity.Models;
using Svrf.Unity.Utilities;
using UnityEngine;

namespace Svrf.Unity
{
    public class SvrfModel : MonoBehaviour
    {
        public string SvrfModelId;

        public bool WithOccluder = DefaultOptions.WithOccluder;
        public Shader ShaderOverride = DefaultOptions.ShaderOverride;

        private static SvrfApi _svrf;
        private static readonly SvrfModelOptions DefaultOptions = new SvrfModelOptions
        {
            ShaderOverride = null,
            WithOccluder = true,
        };

        public bool IsLoading { get; set; } = true;

        public async void Start()
        {
            IsLoading = true;
            CreateSvrfInstance();

            var model = (await _svrf.Media.GetByIdAsync(SvrfModelId)).Media;
            var options = new SvrfModelOptions
            {
                ShaderOverride = ShaderOverride,
                WithOccluder = WithOccluder
            };

            await SvrfModelUtility.AddSvrfModel(gameObject, model, options);

            IsLoading = false;
        }

        public static async Task<GameObject> GetSvrfModelAsync(MediaModel model, SvrfModelOptions options = null, GameObject gameObject = null)
        {
            // It's impossible to use null coalescing operator with Unity objects.
            gameObject = gameObject == null ? new GameObject("Svrf Model") : gameObject;
            options = options ?? DefaultOptions;

            await SvrfModelUtility.AddSvrfModel(gameObject, model, options);

            return gameObject;
        }

        private static void CreateSvrfInstance()
        {
            if (_svrf == null)
            {
                _svrf = new SvrfApi();
            }
        }
    }
}
