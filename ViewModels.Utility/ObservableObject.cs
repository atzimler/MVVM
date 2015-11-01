using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Utility
{
    public abstract class ObservableObject<T> : INotifyPropertyChanged
    {
        #region Public Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Private Methods
        // TODO: This might be better with nameof, but first code coverage and unit tests must be applied.
        private MemberExpression GetMemberExpression(Expression<Func<T, object>> property)
        {
            if (property == null || property.Body == null)
            {
                return null;
            }

            if (property.Body is MemberExpression)
            {
                return property.Body as MemberExpression;
            }

            if (property.Body is UnaryExpression)
            {
                UnaryExpression unaryExpression = property.Body as UnaryExpression;
                if (unaryExpression.Operand is MemberExpression)
                {
                    return unaryExpression.Operand as MemberExpression;
                }
            }

            return null;
        }
        #endregion
        
        #region Protected Methods
        protected virtual void OnPropertyChanged(Expression<Func<T, object>> property)
        {
            var memberExp = GetMemberExpression(property);
            if (memberExp == null)
            {
                return;
            }

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(memberExp.Member.Name));
            }
        }

        protected SuspendPropertyChangedEvent SuspendPropertyChangedEvent(PropertyChangedEventHandler eventHandler)
        {
            PropertyChanged -= eventHandler;
            return new SuspendPropertyChangedEvent(() => PropertyChanged += eventHandler);
        }
        #endregion

        #region Public Methods
        public virtual string PropertyName(Expression<Func<T, object>> property)
        {
            var memberExp = GetMemberExpression(property);
            if (memberExp == null)
            {
                return null;
            }

            return memberExp.Member.Name;
        }
        #endregion
    }
}
