namespace VersioningTestApp.Areas.HelpPage
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Reflection;
	using System.Text.RegularExpressions;
	using System.Web.Http;
	using System.Web.Http.Controllers;
	using System.Web.Http.Description;
	using System.Xml.XPath;

	using VersioningTestApp.Areas.HelpPage.ModelDescriptions;

	/// <summary>
	/// A custom <see cref="IDocumentationProvider"/> that reads the API documentation from an XML documentation file.
	/// </summary>
	public class MultipleXmlDocumentationProvider : IDocumentationProvider, IModelDocumentationProvider
	{
		private readonly IEnumerable<XPathNavigator> documentNavigators;

		private const string TypeExpression = "/doc/members/member[@name='T:{0}']";
		private const string MethodExpression = "/doc/members/member[@name='M:{0}']";
		private const string PropertyExpression = "/doc/members/member[@name='P:{0}']";
		private const string FieldExpression = "/doc/members/member[@name='F:{0}']";
		private const string ParameterExpression = "param[@name='{0}']";

		/// <summary>
		/// Initializes a new instance of the <see cref="MultipleXmlDocumentationProvider" /> class.
		/// </summary>
		/// <param name="documentPaths">The physical paths to XML documents.</param>
		/// <exception cref="System.ArgumentNullException">documentPaths</exception>
		public MultipleXmlDocumentationProvider(params string[] documentPaths)
		{
			if (documentPaths == null)
			{
				throw new ArgumentNullException("documentPaths");
			}

			this.documentNavigators = documentPaths.Select(path => new XPathDocument(path)).Select(xpath => xpath.CreateNavigator());
		}

		/// <summary>
		/// Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpControllerDescriptor" />.
		/// </summary>
		/// <param name="controllerDescriptor">The controller descriptor.</param>
		/// <returns>The documentation for the controller.</returns>
		public string GetDocumentation(HttpControllerDescriptor controllerDescriptor)
		{
			var typeNode = this.GetTypeNode(controllerDescriptor.ControllerType);

			var s = new List<string>
						{
							GetTagValueForController(typeNode, "summary"),
							GetTagValueForController(typeNode, "remarks")
						};

			// Add message if controller requires authorization
			if (Attribute.IsDefined(controllerDescriptor.ControllerType, typeof(AuthorizeAttribute)))
			{
				s.Add("<p><i class='fa fa-lock'></i> Requires authorization!</p>");
			}

			return string.Join("", s.Where(x => !string.IsNullOrEmpty(x)));
		}

		/// <summary>
		/// Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpActionDescriptor" />.
		/// </summary>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>The documentation for the action.</returns>
		public virtual string GetDocumentation(HttpActionDescriptor actionDescriptor)
		{
			var methodNode = this.GetMethodNode(actionDescriptor);

			var s = new List<string>
						{
							GetTagValueForAction(methodNode, "summary"), 
							GetTagValueForAction(methodNode, "remarks")
						};

			// Add message if action requires authorization
			if (actionDescriptor.GetCustomAttributes<AuthorizeAttribute>().Any())
			{
				s.Add("<p><i class='fa fa-lock'></i> Requires authorization!</p>");
			}

			return string.Join("", s.Where(x => !string.IsNullOrEmpty(x)));
		}

		/// <summary>
		/// Gets the documentation based on <see cref="T:System.Web.Http.Controllers.HttpParameterDescriptor" />.
		/// </summary>
		/// <param name="parameterDescriptor">The parameter descriptor.</param>
		/// <returns>The documentation for the parameter.</returns>
		public virtual string GetDocumentation(HttpParameterDescriptor parameterDescriptor)
		{
			var reflectedParameterDescriptor = parameterDescriptor as ReflectedHttpParameterDescriptor;

			if (reflectedParameterDescriptor != null)
			{
				var methodNode = this.GetMethodNode(reflectedParameterDescriptor.ActionDescriptor);

				if (methodNode != null)
				{
					var parameterName = reflectedParameterDescriptor.ParameterInfo.Name;
					var parameterNode = methodNode.SelectSingleNode(String.Format(CultureInfo.InvariantCulture, ParameterExpression, parameterName));

					if (parameterNode != null)
					{
						return parameterNode.Value.Trim();
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the documentation based on <see cref="T:MemberInfo"/>.
		/// </summary>
		/// <param name="member">The member.</param>
		/// <returns>The documentation for the member.</returns>
		public string GetDocumentation(MemberInfo member)
		{
			var memberName = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetTypeName(member.DeclaringType), member.Name);
			var expression = member.MemberType == MemberTypes.Field ? FieldExpression : PropertyExpression;
			var selectExpression = String.Format(CultureInfo.InvariantCulture, expression, memberName);

			foreach (var documentNavigator in this.documentNavigators)
			{
				var n = documentNavigator.SelectSingleNode(selectExpression);

				if (n != null)
				{
					var s = new[]
						{
							GetTagValue(n, "summary"),
							GetTagValue(n, "returns")
						};

					return string.Join("<br/>", s.Where(x => !string.IsNullOrEmpty(x)));
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the documentation for the specified type.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The documentation for the type.</returns>
		public string GetDocumentation(Type type)
		{
			var typeNode = this.GetTypeNode(type);

			var s = new[]
						{
							GetTagValue(typeNode, "summary"),
							GetTagValue(typeNode, "returns")
						};

			return string.Join("<br/>", s.Where(x => !string.IsNullOrEmpty(x)));
		}

		/// <summary>
		/// Gets the response documentation.
		/// </summary>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>A html-formatted string for the action response.</returns>
		public string GetResponseDocumentation(HttpActionDescriptor actionDescriptor)
		{
			var methodNode = this.GetMethodNode(actionDescriptor);

			var s = new[]
						{
							GetTagValue(methodNode, "returns")
						};

			return string.Join("<br/>", s.Where(x => !string.IsNullOrEmpty(x)));
		}

		/// <summary>
		/// Gets the method node.
		/// </summary>
		/// <param name="actionDescriptor">The action descriptor.</param>
		/// <returns>A xpath navigator.</returns>
		private XPathNavigator GetMethodNode(HttpActionDescriptor actionDescriptor)
		{
			var reflectedActionDescriptor = actionDescriptor as ReflectedHttpActionDescriptor;

			if (reflectedActionDescriptor != null)
			{
				var selectExpression = string.Format(CultureInfo.InvariantCulture, MethodExpression, GetMemberName(reflectedActionDescriptor.MethodInfo));

				foreach (var documentNavigator in this.documentNavigators)
				{
					var n = documentNavigator.SelectSingleNode(selectExpression);

					if (n != null)
					{
						return n;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the type node.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>A xpath navigator.</returns>
		private XPathNavigator GetTypeNode(Type type)
		{
			var controllerTypeName = GetTypeName(type);
			var selectExpression = String.Format(CultureInfo.InvariantCulture, TypeExpression, controllerTypeName);

			foreach (var documentNavigator in this.documentNavigators)
			{
				var n = documentNavigator.SelectSingleNode(selectExpression);

				if (n != null)
				{
					return n;
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the member name.
		/// </summary>
		/// <param name="method">The method.</param>
		/// <returns>The member name.</returns>
		private static string GetMemberName(MethodInfo method)
		{
			var name = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", GetTypeName(method.DeclaringType), method.Name);
			var parameters = method.GetParameters();

			if (parameters.Length != 0)
			{
				var parameterTypeNames = parameters.Select(param => GetTypeName(param.ParameterType)).ToArray();
				name += String.Format(CultureInfo.InvariantCulture, "({0})", String.Join(",", parameterTypeNames));
			}

			return name;
		}

		/// <summary>
		/// Gets the type name.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>The type name.</returns>
		private static string GetTypeName(Type type)
		{
			var name = type.FullName;

			if (type.IsGenericType)
			{
				// Format the generic type name to something like: Generic{System.Int32,System.String}
				var genericType = type.GetGenericTypeDefinition();
				var genericArguments = type.GetGenericArguments();
				var genericTypeName = genericType.FullName;

				// Trim the generic parameter counts from the name
				genericTypeName = genericTypeName.Substring(0, genericTypeName.IndexOf('`'));
				var argumentTypeNames = genericArguments.Select(t => GetTypeName(t)).ToArray();

				name = String.Format(CultureInfo.InvariantCulture, "{0}{{{1}}}", genericTypeName, String.Join(",", argumentTypeNames));
			}

			if (type.IsNested)
			{
				// Changing the nested type name from OuterType+InnerType to OuterType.InnerType to match the XML documentation syntax.
				name = name.Replace("+", ".");
			}

			return name;
		}

		/// <summary>
		/// Gets the tag value for a controller.
		/// </summary>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="tagName">The tag name.</param>
		/// <returns>A html-formatted representation of the tag value.</returns>
		private static string GetTagValueForController(XPathNavigator parentNode, string tagName)
		{
			if (parentNode != null)
			{
				var node = parentNode.SelectSingleNode(tagName);

				if (node != null)
				{
					var v = node.InnerXml.Replace("\r", "").Replace("\n", "").Trim();

					if (string.IsNullOrEmpty(v))
					{
						return null;
					}

					// Surround <remarks> in <em>
					if (tagName.ToLower() == "remarks")
					{
						v = "<em>" + v + "</em>";
					}

					// Convert xml doc tags to html tags
					v = ConvertDocTagsToHtml(v);

					if (v.Contains("<p>"))
					{
						if (v.StartsWith("<p>") && v.EndsWith("</p>"))
						{
							return v;
						}

						// Insert paragraph around text before first p
						if (!v.StartsWith("<p>"))
						{
							v = "<p>" + v.Insert(v.IndexOf("<p>"), "</p>");
						}

						// Insert paragraph around text after last p
						if (!v.EndsWith("</p>"))
						{
							v = v.Insert(v.LastIndexOf("</p>"), "<p>") + "</p>";
						}

						return v;
					}

					return "<p>" + v + "</p>";
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the tag value for a controller.
		/// </summary>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="tagName">The tag name.</param>
		/// <returns>A html-formatted representation of the tag value.</returns>
		private static string GetTagValueForAction(XPathNavigator parentNode, string tagName)
		{
			if (parentNode != null)
			{
				var node = parentNode.SelectSingleNode(tagName);

				if (node != null)
				{
					var v = node.InnerXml.Trim();

					if (string.IsNullOrEmpty(v))
					{
						return null;
					}

					// Surround <remarks> in <em>
					if (tagName.ToLower() == "remarks")
					{
						v = "<em>" + v + "</em>";
					}

					// Convert xml doc tags to html tags
					v = ConvertDocTagsToHtml(v);

					if (v.Contains("<p>"))
					{
						if (v.StartsWith("<p>") && v.EndsWith("</p>"))
						{
							return v;
						}

						// Insert paragraph around text before first p
						if (!v.StartsWith("<p>"))
						{
							v = "<p>" + v.Insert(v.IndexOf("<p>"), "</p>");
						}

						// Insert paragraph around text after last p
						if (!v.EndsWith("</p>"))
						{
							v = v.Insert(v.LastIndexOf("</p>"), "<p>") + "</p>";
						}

						return v;
					}

					return "<p>" + v + "</p>";
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the tag value.
		/// </summary>
		/// <param name="parentNode">The parent node.</param>
		/// <param name="tagName">The tag name.</param>
		/// <returns>A html-formatted representation of the tag value.</returns>
		private static string GetTagValue(XPathNavigator parentNode, string tagName)
		{
			if (parentNode != null)
			{
				var node = parentNode.SelectSingleNode(tagName);

				if (node != null)
				{
					var v = node.InnerXml.Trim();

					if (string.IsNullOrEmpty(v))
					{
						return null;
					}

					// Convert xml doc tags to html tags
					v = ConvertDocTagsToHtml(v);

					return v;
				}
			}

			return null;
		}

		/// <summary>
		/// Converts the document tags to HTML.
		/// </summary>
		/// <param name="i">The input xml.</param>
		/// <returns>A HTML-formatted string.</returns>
		private static string ConvertDocTagsToHtml(string i)
		{
			// Convert <para> to <p>
			i = i.Replace("<para>", "<p>").Replace("</para>", "</p>");

			// Convert <code> to <pre>
			i = i.Replace("<code>", "<pre>").Replace("</code>", "</pre>");

			// Convert <c> to <code>
			i = i.Replace("<c>", "<code>").Replace("</c>", "</code>");

			// Convert <example> to <samp>
			i = i.Replace("<example>", "<samp>").Replace("</example>", "</samp>");

			// Convert <exception cref=""></exception> to <code><a href=""></a></code>
			i = Regex.Replace(i, @"(.*)<exception cref=""T:(.*\.)([^""]+)""\s*>(.*)</exception>(.*)", @"$1<code><a href=""/Help/ResourceModel?modelName=$3"">$4</a></code>$5");

			// Convert <paramref name=""/> to <code/>
			i = Regex.Replace(i, @"(.*)<paramref name=""([^""]+)""\s*/>(.*)", @"$1<code>$2</code>$3");

			// Convert <see cref=""/> to <code><a href=""/></code>
			i = Regex.Replace(i, @"(.*)<see cref=""T:(.*\.)([^""]+)""\s*/>(.*)", @"$1<code><a href=""/Help/ResourceModel?modelName=$3"">$3</a></code>$4");

			// Convert <seealso cref=""/> to <code><a href=""/></code>
			i = Regex.Replace(i, @"(.*)<seealso cref=""T:(.*\.)([^""]+)""\s*/>(.*)", @"$1<code><a href=""/Help/ResourceModel?modelName=$3"">$3</a></code>$4");

			// Convert <typeparamref name=""/> to <code/>
			i = Regex.Replace(i, @"(.*)<typeparamref name=""([^""]+)""\s*/>(.*)", @"$1<code>$2</code>$3");

			return i;
		}
	}
}
