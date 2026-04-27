using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;

public class PS1PostProcessFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings {
        public Material material;
        // Эффект накладывается после основной обработки, чтобы не ломать свет
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    }

    public Settings settings = new Settings();
    PS1RenderPass m_ScriptablePass;

    public override void Create() {
        m_ScriptablePass = new PS1RenderPass(settings.material);
        m_ScriptablePass.renderPassEvent = settings.renderPassEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) {
        // Проверка: не запускаем, если нет материала или мы в режиме превью
        if (settings.material != null && renderingData.cameraData.cameraType == CameraType.Game || renderingData.cameraData.cameraType == CameraType.SceneView) {
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }

    class PS1RenderPass : ScriptableRenderPass {
        private Material mat;
        public PS1RenderPass(Material mat) => this.mat = mat;

        private class PassData {
            public TextureHandle src;
            public Material material;
        }

        public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData) {
            UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
            UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

            // Получаем основную текстуру камеры
            TextureHandle cameraColor = resourceData.activeColorTexture;
            if (!cameraColor.IsValid()) return;

            // 1. НАСТРОЙКА ВРЕМЕННОЙ ТЕКСТУРЫ (Чтобы избежать ошибок формата)
            var cameraDesc = cameraData.cameraTargetDescriptor;
            TextureDesc desc = new TextureDesc(cameraDesc.width, cameraDesc.height);
            desc.name = "PS1_TemporaryBuffer";
            desc.colorFormat = cameraDesc.graphicsFormat; // Важно: берем формат цвета у камеры
            desc.depthBufferBits = 0; // Нам не нужна глубина для пост-эффекта
            desc.msaaSamples = MSAASamples.None;
            
            TextureHandle tempTexture = renderGraph.CreateTexture(desc);

            // 2. ПЕРВЫЙ ПРОХОД: Накладываем шейдер (Camera -> Temp)
            using (var builder = renderGraph.AddRasterRenderPass<PassData>("PS1_ApplyDithering", out var passData)) {
                passData.src = cameraColor;
                passData.material = mat;

                builder.UseTexture(cameraColor, AccessFlags.Read);
                builder.SetRenderAttachment(tempTexture, 0, AccessFlags.Write);
                builder.AllowPassCulling(false);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) => {
                    // Используем системный Blitter (стандарт Unity 6)
                    Blitter.BlitTexture(context.cmd, data.src, new Vector4(1, 1, 0, 0), data.material, 0);
                });
            }

            // 3. ВТОРОЙ ПРОХОД: Копируем результат назад (Temp -> Camera)
            using (var builder = renderGraph.AddRasterRenderPass<PassData>("PS1_OutputToScreen", out var passData)) {
                passData.src = tempTexture;

                builder.UseTexture(tempTexture, AccessFlags.Read);
                builder.SetRenderAttachment(cameraColor, 0, AccessFlags.Write);
                builder.AllowPassCulling(false);

                builder.SetRenderFunc((PassData data, RasterGraphContext context) => {
                    // Простое копирование без шейдера (параметр material = 0)
                    Blitter.BlitTexture(context.cmd, data.src, new Vector4(1, 1, 0, 0), 0, false);
                });
            }
        }
    }
}