using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using URFramework;

namespace TestScripts
{
	public class Test : Singleton<Test>
	{
		public void Testa()
		{
			
		}
	}

	public class Test2
	{
		public void Testa()
		{
			Test.Instance.Testa();
		}
	}
}

