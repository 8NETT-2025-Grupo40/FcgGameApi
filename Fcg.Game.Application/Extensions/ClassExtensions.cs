using System.Linq.Expressions;

namespace Fcg.Game.Application.Extensions
{
	public static class ClassExtensions
	{
		public static string GetPropertyName<TModel, TProperty>(Expression<Func<TModel, TProperty>> property)
		{
			MemberExpression memberExpression = (MemberExpression)property.Body;

			return memberExpression.Member.Name;
		}
	}
}
