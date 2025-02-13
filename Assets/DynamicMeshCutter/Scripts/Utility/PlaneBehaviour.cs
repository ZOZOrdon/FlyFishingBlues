using System.Collections;
using UnityEngine;

namespace DynamicMeshCutter
{
    public class PlaneBehaviour : CutterBehaviour
    {
        public float DebugPlaneLength = 2;

        public MeshTarget[] targetsToCut;
        public KnifeController KnifeController;
        public GameObject bloodEffectPrefab;
        public Vector3 targetScale = new Vector3(10f, 10f, 10f);
        public GameObject Bucket;

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
            MeshCreation.TranslateCreatedObjects(info, cData.CreatedObjects, cData.CreatedTargets, Separation);
            
            Vector3 spawnPosition = info.MeshTarget.transform.position;
            Quaternion spawnRotation = Quaternion.identity; // 根据需要调整旋转
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, spawnPosition, spawnRotation);

            // 启动协程，放大血迹效果
            StartCoroutine(ScaleBloodEffect(bloodEffect.transform));
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
        IEnumerator ScaleBloodEffect(Transform bloodTransform)
        {
            float duration = 1f; // 放大持续时间
            float elapsedTime = 0f;
            Vector3 initialScale = Vector3.zero; // 初始大小为 0
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
