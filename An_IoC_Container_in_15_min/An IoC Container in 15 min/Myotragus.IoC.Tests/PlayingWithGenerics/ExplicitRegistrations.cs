
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace Myotragus.IoC.Tests.PlayingWithGenerics
{
	public interface IExplicit<TType>
	{
		void Print();
	}

	public class StringExplicit : IExplicit<string>
	{
		public void Print()
		{
			Console.WriteLine("System.String");
		}
	}

	public class IntExplicit : IExplicit<int>
	{
		public void Print()
		{
			Console.WriteLine("System.Int32");
		}
	}

	public class TypeExplicit : IExplicit<Type>
	{
		public void Print()
		{
			Console.WriteLine("System.Type");
		}
	}

	[TestFixture]
	[TestClass]
	public class ExplicitRegistrations
	{
		[Test]
		[TestMethod]
		public void Test()
		{
			var services = new ServicesContainer();
			services.RegisterForAll(typeof(StringExplicit), 
				typeof(IntExplicit), typeof(TypeExplicit));

			services.Resolve<IExplicit<int>>().Print();
			services.Resolve<IExplicit<string>>().Print();
			services.Resolve<IExplicit<Type>>().Print();
		}
	}
}