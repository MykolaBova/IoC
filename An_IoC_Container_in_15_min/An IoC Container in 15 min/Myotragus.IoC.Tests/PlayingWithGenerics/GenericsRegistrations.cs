using System;
using NUnit.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assert = NUnit.Framework.Assert;

namespace Myotragus.IoC.Tests.PlayingWithGenerics
{
	public interface ITypeNamePrinter<TType>
	{
		void Print();
	}

	public class TypeNamePrinter<TType> : ITypeNamePrinter<TType>
	{
		public void Print()
		{
			Console.WriteLine(typeof(TType).FullName);
		}
	}

	[TestFixture]
	[TestClass]
	public class GenericRegistrations
	{
		[Test]
		[TestMethod]
		public void ExplicitRegistration()
		{
			var services = new ServicesContainer();

			services.RegisterForAll(typeof(TypeNamePrinter<int>),
				typeof(TypeNamePrinter<string>), typeof(TypeNamePrinter<Type>));

			services.Resolve<ITypeNamePrinter<int>>().Print();
			services.Resolve<ITypeNamePrinter<string>>().Print();
			services.Resolve<ITypeNamePrinter<Type>>().Print();

			Assert.Throws(typeof(Exception), 
				() => services.Resolve<ITypeNamePrinter<float>>().Print());
		}

		[Test]
		[TestMethod]
		public void ImplicitRegistration()
		{
			var services = new ServicesContainer();

			services.RegisterForAll(typeof(TypeNamePrinter<>));

			services.Resolve<ITypeNamePrinter<int>>().Print();
			services.Resolve<ITypeNamePrinter<string>>().Print();
			services.Resolve<ITypeNamePrinter<Type>>().Print();

			Assert.DoesNotThrow(
				() => services.Resolve<ITypeNamePrinter<float>>().Print());
		}
	}
}
