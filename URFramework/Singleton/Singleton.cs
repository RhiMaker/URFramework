using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace URFramework
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance;

		public static T Instance
		{
			get => instance;
		}

		protected virtual void Awake()
		{
			instance = this as T;
		}
	}
}