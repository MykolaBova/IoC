using System;

namespace Myotragus.IoC.Tests
{
	public interface IKing
	{
		IBoss<IGuard> Captain { get; }
		IBoss<IMaid> Mistress { get; }

		void RuleTheCastle();
	}

	public interface IServant
	{
		void Serve();
	}

	public interface IGuard : IServant
	{

	}

	public interface IMaid : IServant
	{

	}

	public interface IBoss<TServant>
			where TServant : IServant
	{
		TServant Servant { get; }
		void OrderServantToServe();
	}

	public class King : Service, IKing
	{
		public King(IBoss<IGuard> captain, IBoss<IMaid> mistress)
		{
			Captain = captain;
			Mistress = mistress;
		}

		public void RuleTheCastle()
		{
			Console.WriteLine("Rule!!");

			Captain.OrderServantToServe();
			Mistress.OrderServantToServe();
		}

		public IBoss<IGuard> Captain
		{
			get;
			private set;
		}

		public IBoss<IMaid> Mistress
		{
			get;
			private set;
		}
	}

	public class Boss<TServant> : Service, IBoss<TServant>
			where TServant : IServant
	{
		public Boss(TServant servant)
		{
			Servant = servant;
		}

		public TServant Servant
		{
			get;
			private set;
		}

		public void OrderServantToServe()
		{
			Servant.Serve();
		}
	}

	public class Guard : Service, IGuard
	{
		public void Serve()
		{
			Console.WriteLine("Watch!!");
		}
	}

	public class Maid : Service, IMaid
	{
		public void Serve()
		{
			Console.WriteLine("Clean!!");
		}
	}
}
