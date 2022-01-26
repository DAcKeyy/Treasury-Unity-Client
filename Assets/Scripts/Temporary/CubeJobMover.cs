using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Temporary
{
    public struct CubeJobMover : IJobParallelForTransform
    {
        [ReadOnly] public NativeArray<CubeJobData> CubeJobDatas;

        public void Execute(int index, TransformAccess transform)
        {
            var _random = new Unity.Mathematics.Random(228);
            transform.position = _random.NextFloat3(-10, 10);
        }
    }
}