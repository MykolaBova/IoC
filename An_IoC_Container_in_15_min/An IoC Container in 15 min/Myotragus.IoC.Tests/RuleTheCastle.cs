using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace Myotragus.IoC.Tests
{
	[TestClass]
	[TestFixture]
	public class RuleTheCastle
	{
		public static IServicesContainer Services
		{
			get;
			private set;
		}

		public TestContext TestContext
		{
			get;
			set;
		}

		[ClassInitialize]
		public static void Init(TestContext context)
		{
			var services = new ServicesContainer();

			services.RegisterForAll(ServicesImplementation
				.FromAssemblyContaining<IKing>());

			Services = services;
		}

		[SetUp]
		public void SetUp()
		{
			Init(null);
		}

		[Test]
		[TestMethod]
		public void RuleTheCastleTest()
		{
			var king = Services.Resolve<IKing>();

			king.RuleTheCastle();
		}
	}
}
