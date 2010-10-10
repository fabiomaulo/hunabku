using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace HunabKu.Web
{
	public enum PropertyType
	{
		ValueType,
		Collection,
		ParamObject
	}

	public class Property
	{
		internal Property(string propertyName, PropertyType propertyType)
		{
			PropertyName = propertyName;
			PropertyType = propertyType;
			
		}

		internal Property(string propertyName, PropertyType propertyType, object value)
		{
			PropertyName = propertyName;
			PropertyType = propertyType;
			Value = value;
		}

		public object Value{get; set;}
		public string PropertyName { get; private set; }
		public PropertyType PropertyType { get; set; }
		public string[] AsCollection
		{
			get { return (string[]) Value; }
		}

		public bool Equals(Property other)
		{
			if (ReferenceEquals(null, other))
			{
				return false;
			}
			if (ReferenceEquals(this, other))
			{
				return true;
			}
			return Equals(other.PropertyName, PropertyName);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Property);
		}

		public override int GetHashCode()
		{
			return (PropertyName != null ? PropertyName.GetHashCode() : 0);
		}
	}

	public class ParamObject
	{
		public ParamObject(Property owner)
		{
			Owner = owner;
			Properties = new HashSet<Property>();
		}

		public ParamObject(Property owner, HashSet<Property> properties)
		{
			Owner = owner;
			Properties = properties;
		}

		public Property Owner { get; private set; }
		public HashSet<Property> Properties { get; private set; }
	}

	public class QueryStringToGraph
	{
		public ParamObject Parse(NameValueCollection queryString)
		{
			var result = new ParamObject(null);
			var keys = queryString.AllKeys;
			if (keys != null && keys.Length > 0)
			{
				foreach (var key in keys)
				{
					var parts = key.Split('.');
					if(parts.Length == 1)
					{
						result.Properties.Add(NewValueProp(key, queryString.GetValues(key)));
					}
					else
					{
						ParamObject owner = result;
						foreach (var part in parts.Take(parts.Length-1))
						{
							var newProp = new Property(part, PropertyType.ParamObject);
							var prop = owner.Properties.FirstOrDefault(x => x.Equals(newProp)) ?? newProp;
							owner.Properties.Add(prop);
							if(prop.Value == null)
							{
								prop.Value = new ParamObject(prop);
							}
							owner = (ParamObject)prop.Value;
						}
						owner.Properties.Add(NewValueProp(parts[parts.Length - 1], queryString.GetValues(key)));
					}
				}
			}
			return result;
		}

		private Property NewValueProp(string propertyName, string[] value)
		{
			return value.Length == 1 ? new Property(propertyName, PropertyType.ValueType, value[0]) : new Property(propertyName, PropertyType.Collection, value);
		}
	}
}