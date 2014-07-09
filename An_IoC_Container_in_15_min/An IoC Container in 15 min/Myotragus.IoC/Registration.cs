using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Myotragus.IoC
{
	public interface ITypeCollection : IEnumerable<Type>
	{

	}

	internal class TypeCollection : ITypeCollection
	{
		internal IEnumerable<Type> Actual { get; set; }

		#region IEnumerable<Type> Members

		public IEnumerator<Type> GetEnumerator()
		{
			return Actual.GetEnumerator();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return Actual.GetEnumerator();
		}

		#endregion
	}

	public interface IServicesImplementationCollection : ITypeCollection
	{
	}

	internal class ServicesImplementationCollection : TypeCollection, IServicesImplementationCollection
	{ }

	public static class ServicesImplementation
	{
		public static IServicesImplementationCollection FromAssembly(Assembly asm)
		{
			return FromThese(asm.GetTypes());
		}

		public static IServicesImplementationCollection FromAssemblyContaining(Type type)
		{
			return FromAssembly(type.Assembly);
		}

		public static IServicesImplementationCollection FromAssemblyContaining<TType>()
		{
			return FromAssembly(typeof(TType).Assembly);
		}

		public static IServicesImplementationCollection FromThese(IEnumerable<Type> types)
		{
			return new ServicesImplementationCollection
			{
				Actual = types.Where(x => x.IsClass && !x.IsAbstract)
			};
		}

		public static IServicesImplementationCollection FromThese(params Type[] types)
		{
			return FromThese((IEnumerable<Type>)types);
		}
	}

	public static class ServicesFilters
	{
		public static IServicesImplementationCollection Except(this IServicesImplementationCollection @this, IEnumerable<Type> except)
		{
			return new ServicesImplementationCollection { Actual = @this.Except(except) };
		}

		public static IServicesImplementationCollection Except(this IServicesImplementationCollection @this, params Type[] except)
		{
			return @this.Except((IEnumerable<Type>)except);
		}

		public static IServicesImplementationCollection NotImplementing<TService>(this IServicesImplementationCollection @this)
		{
			return @this.NotImplementing(typeof(TService));
		}

		public static IServicesImplementationCollection NotImplementing(this IServicesImplementationCollection @this, Type tService)
		{
			return new ServicesImplementationCollection
			{
				Actual = @this.Where(s => !tService.IsAssignableFrom(s))
			};
		}

		public static IServicesImplementationCollection Implementing<TService>(this IServicesImplementationCollection @this)
		{
			return @this.Implementing(typeof(TService));
		}

		public static IServicesImplementationCollection Implementing(this IServicesImplementationCollection @this, Type tService)
		{
			return new ServicesImplementationCollection
			{
				Actual = @this.Where(s => tService.IsAssignableFrom(s))
			};
		}
	}
}
