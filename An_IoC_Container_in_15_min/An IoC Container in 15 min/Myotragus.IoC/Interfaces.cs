using System;
using System.Collections.Generic;

namespace Myotragus.IoC
{
	/// <summary>
	/// Holds a singleton instance for every registered service
	/// </summary>
	public interface IServicesContainer
	{
		/// <summary>
		/// Resolves a registered service, provided an interface. Services are register as singleton
		/// </summary>
		/// <seealso cref="Resolve(Type)"/>
		/// <typeparam name="TService">Interface service type</typeparam>
		/// <returns>Service instance</returns>
		TService Resolve<TService>();

		/// <summary>
		/// Resolves a registered service, provided an interface. Services are register as singleton
		/// </summary>
		/// <seealso cref="Resolve{TService}()"/>
		/// <param name="tService">Interface service type</param>
		/// <returns>Service instance</returns>
		object Resolve(Type tService);
	}

	public interface IServicesRegistrar : IServicesContainer, ITypeRegistrar
	{ }

	public interface ITypeRegistrar
	{
		ITypeRegistrar RegisterFor(Type implementation, IEnumerable<Type> interfaces);
		ITypeRegistrar RegisterFor(Type implementation, params Type[] interfaces);

		ITypeRegistrar RegisterForAll(IEnumerable<Type> implementations);
		ITypeRegistrar RegisterForAll(params Type[] implementations);
	}
}
