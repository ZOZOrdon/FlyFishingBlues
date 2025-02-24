using System.Collections;
using UnityEngine;

namespace DynamicMeshCutter
{
    public class PlaneBehaviour : CutterBehaviour
    {
        public float DebugPlaneLength = 2;

        public MeshTarget[] targetsToCut;
        public KnifeController KnifeController;

        public Vector3 targetScale = new Vector3(1.5f, 1.5f, 0.5f);
        public GameObject Bucket;
        public GameObject BloodEffectPrefab;
        private Transform bloodTransform;

        public void Start()
        {
            
        }
        public void Cut()
        {
            if (targetsToCut != null && targetsToCut.Length > 0)
            {
                foreach (var target in targetsToCut)
                {

                    Cut(target, transform.position, transform.forward, null, OnCreated);
                    Debug.Log("!!!!!!!");
                    Debug.Log($"Cutting target: {target.gameObject.name}");

                }
            }
            else
            {
                Debug.LogWarning("没有指定要被切割的目标对象！");
            }
        }


        void OnCreated(Info info, MeshCreationData cData)
        {
            // 指定位置和旋转
            Vector3 position = new Vector3(-0.05f, 0.55f, 0.05f);
            Quaternion rotation = Quaternion.Euler(-90f, 0f, 0f);

            // 实例化预制体
            GameObject instance = Instantiate(BloodEffectPrefab, position, rotation);

            // 手动设置缩放
            instance.transform.localScale = new Vector3(0.05f, 0.05f, 1f); 
            bloodTransform = instance.transform;
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
            // 启动协程，放大血迹效果
            StartCoroutine(ScaleBloodEffect());
            foreach (var target in cData.CreatedTargets)
            {
                if (target != null)
                {
                    // 添加 Grabbable 脚本
                    target.gameObject.AddComponent<Grabbable>();
                     
                }
            }
            Bucket.SetActive(true);
        }
        IEnumerator ScaleBloodEffect()
        {

            float duration = 3f; // 放大持续时间
            float elapsedTime = 0f;
            Vector3 initialScale = bloodTransform.localScale; // 初始大小为 0
                                                                  // 目标大小，根据需要调整

            // 将血迹效果的初始大小设置为 0
            bloodTransform.localScale = initialScale;

            // 放大血迹效果
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                bloodTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
                yield return null;
            }
            KnifeController.deleteTHeknife();
        }
    }
}
