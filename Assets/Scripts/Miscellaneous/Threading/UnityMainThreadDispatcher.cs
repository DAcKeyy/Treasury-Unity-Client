using System;
using System.Collections.Generic;
using Treasury.Miscellaneous.Optimization.System;

namespace Treasury.Miscellaneous.Threading
{
	public class UnityMainThreadDispatcher : Singleton<UnityMainThreadDispatcher>
	{
		private const int QUEUE_CAPACITY = 10000;
		
		public Queue<Action> AwakeQueue = new Queue<Action>(QUEUE_CAPACITY);
		public Queue<Action> StartQueue = new Queue<Action>(QUEUE_CAPACITY);
		public Queue<Action> UpdateQueue = new Queue<Action>(QUEUE_CAPACITY);
		public Queue<Action> LateUpdateQueue = new Queue<Action>(QUEUE_CAPACITY);
		public Queue<Action> FixedUpdateQueue = new Queue<Action>(QUEUE_CAPACITY);
		
		private void Awake()
		{
			while (AwakeQueue.Count > 0) AwakeQueue.Dequeue().Invoke();
		}
		
		private void Start()
		{
			while (StartQueue.Count > 0) StartQueue.Dequeue().Invoke();
		}

		protected override void CachedUpdate()
		{
			while (UpdateQueue.Count > 0) UpdateQueue.Dequeue().Invoke();
		}
		
		protected override void CachedLateUpdate()
		{
			while (LateUpdateQueue.Count > 0) LateUpdateQueue.Dequeue().Invoke();
		}
		
		protected override void CachedFixedUpdate()
		{
			while (FixedUpdateQueue.Count > 0) FixedUpdateQueue.Dequeue().Invoke();
		}
	}
}