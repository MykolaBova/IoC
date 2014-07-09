using System;
using System.Collections.Generic;
using System.Linq;

namespace Myotragus.IoC
{
	public class Service
	{
		#region IService Members

		public IServicesContainer Services
		{
			get;
			set;
		}

		#endregion
	}

	internal class ServiceDescriptor
	{
		public Type ServiceType { get; set; }
		public object Instance { get; set; }
	}
	/// <summary>
	/// IServiceContainer implementation using Windsor serviceContainer
	/// </summary>
	public class ServicesContainer : IServicesRegistrar
	{
		#region Fields

		private readonly IDictionary<Type, ServiceDescriptor> _services =
			new Dictionary<Type, ServiceDescriptor>();

		#endregion

		#region IServiceContainer Members

		/// <inheritdoc/>
		public TService Resolve<TService>()
		{
			return (TService)Resolve(typeof(TService));
		}

		/// <inheritdoc/>
		public object Resolve(Type tService)
		{
			var result = GetInstance(tService);

			return result;
		}

		private object GetInstance(Type tService)
		{
			if (_services.ContainsKey(tService))
				return GetInstance(_services[tService]);

			var genericDefinition = tService.GetGenericTypeDefinition();
			if (genericDefinition != null && _services.ContainsKey(genericDefinition))
				return GetGenericInstance(tService, _services[genericDefinition]
						.ServiceType);

			throw new Exception("Type not registered" + tService);
		}

		private object GetInstance(ServiceDescriptor serviceDescriptor)
		{
			return serviceDescriptor.Instance ?? (
					serviceDescriptor.Instance = CreateInstance(serviceDescriptor.ServiceType));
		}

		private object GetGenericInstance(Type tService, Type genericDefinition)
		{
			var genericArguments = tService.GetGenericArguments();
			var actualType = genericDefinition.MakeGenericType(genericArguments);
			var result = CreateInstance(actualType);

			_services[tService] = new ServiceDescriptor
			{
				ServiceType = actualType,
				Instance = result
			};

			return result;
		}

		private object CreateInstance(Type serviceType)
		{
			var ctor = serviceType.GetConstructors().First();
			var dependecies = ctor.GetParameters().Select(p => Resolve(p.ParameterType)).ToArray();

			return ctor.Invoke(dependecies);
		}

		#endregion

		#region IConfigurableServiceRepository Members

		public ITypeRegistrar RegisterForAll(params Type[] implementations)
		{
			return RegisterForAll((IEnumerable<Type>)implementations);
		}

		public ITypeRegistrar RegisterForAll(IEnumerable<Type> implementations)
		{
			foreach (var impl in implementations)
				RegisterFor(impl, impl.GetInterfaces());

			return this;
		}

		public ITypeRegistrar RegisterFor(Type implementation, params Type[] interfaces)
		{
			return RegisterFor(implementation, (IEnumerable<Type>)interfaces);
		}

		public ITypeRegistrar RegisterFor(Type implementation, IEnumerable<Type> interfaces)
		{
			foreach (var @interface in interfaces)
			{
				var descriptor = new ServiceDescriptor
				{
					ServiceType = implementation
				};
				_services[GetRegistrableType(@interface)] = descriptor;
			}

			return this;
		}

		private static Type GetRegistrableType(Type type)
		{
			return type.IsGenericType && type.ContainsGenericParameters 
				? type.GetGenericTypeDefinition() : type;
		}

		#endregion
	}
}
